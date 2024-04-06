using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.DataFormats;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WinFormsApp10
{
    public partial class Funtion : Form
    {
        public Funtion()
        {
            InitializeComponent();
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to exit the program?!", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
        private void Funtion_Load(object sender, EventArgs e)
        {

        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            txtName.Text = null;
            txtMembers.Text = null;
            txtTypeCustomer.Text = null;
            txtLastMonthWater.Text = null;
            txtThisMonthWater.Text = null;
            txtMessage.Text = null;
            lsvMessage.Items.Clear();
        }
        private void btnGetBill_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string customerType = txtTypeCustomer.Text.Trim();
            string member = txtMembers.Text.Trim();
            string lastWater = txtLastMonthWater.Text.Trim();
            string thisWater = txtThisMonthWater.Text.Trim();
            bool first = false;
            bool second = false;
            if (customerType.Equals("Household Customer"))
            {
                if (name != "" && checkName1(name)) //   Object-Oriented
                {
                    if (member != "" && int.TryParse(member, out int memberValue))
                    {
                        if (memberValue > 0)
                        {
                            first = true;
                        }
                        else
                        {
                            MessageBox.Show("Number of members must be greater than zero!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else if (member == "")
                    {
                        MessageBox.Show("Number of family members cannot be left blank!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("The number of family members must be an integer!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (name == "")
                {
                    MessageBox.Show("The customer name cannot be left blank!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Please enter a valid name!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (customerType.Equals("Administrative Agency") || customerType.Equals("Production Units") || customerType.Equals("Business Services"))
            {
                if (name != "" && !checkName2(name))
                {
                    MessageBox.Show("Please enter a valid name!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } // 	Object-Oriented
                else if (name == "")
                {
                    MessageBox.Show("The customer name cannot be left blank!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    first = true;
                }
            }
            else if (customerType == "")
            {
                MessageBox.Show("The customer type cannot be left blank!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } 
            else
            {
                MessageBox.Show("Please select the correct customer type!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (first == true)
            {
                if (lastWater != "" && thisWater != "" && int.TryParse(lastWater, out int lastWaterValue) && int.TryParse(thisWater, out int thisWaterValue))
                {
                    if (thisWaterValue >= lastWaterValue)
                    {
                        second = true;
                    }
                    else
                    {
                        MessageBox.Show("This month's water number must be greater than or equal to the previous month's water number!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (lastWater == "" || thisWater == "")
                {
                    MessageBox.Show("Please do not leave the water quantity empty!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Please Enter the water quantity as a valid number!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            if (second == true)
            {
                var billGet = Calculator(customerType, member, lastWater, thisWater);
                txtMessage.Enabled = true;
                txtMessage.Text = $"Customer name:" + $"   {name}\r\n";
                txtMessage.Text += $"Customer Type:" + $"   {customerType}\r\n";
                lsvMessage.Enabled = true;
                lsvMessage.Items.Clear();
                // Create an invoice using listview
                lsvMessage.View = View.Details;
                lsvMessage.Columns.Add("Property", 150);
                lsvMessage.Columns.Add("Value", 200);
                
                ListViewItem nameCustomer = new ListViewItem("");
                ListViewItem waterUsageItem = new ListViewItem("Water Usage");
                ListViewItem waterChargeItem = new ListViewItem("Water Charge");
                ListViewItem vatItem = new ListViewItem("VAT");
                ListViewItem totalBillItem = new ListViewItem("Total Bill");
                
                waterUsageItem.SubItems.Add(billGet.Item1.ToString() + "  m3");
                waterChargeItem.SubItems.Add(billGet.Item2.ToString("N2") + "  VND");
                vatItem.SubItems.Add(billGet.Item3.ToString("N2") + "  VND");
                totalBillItem.SubItems.Add(billGet.Item4.ToString("N2") + "  VND");
                
                lsvMessage.Items.Add(waterUsageItem);
                lsvMessage.Items.Add(waterChargeItem);
                lsvMessage.Items.Add(vatItem);
                lsvMessage.Items.Add(totalBillItem);
                btnPay.Enabled = true;
            }
        }
        static bool checkName1(string customerName)
        {
            return Regex.IsMatch(customerName, @"^[a-zA-Z\s]*$");
        } // “name” check condition if the customer is Household Customer
        static bool checkName2(string customerName)  // “name” checks the condition if the customer is Administrative Agency, Production Units, Business Services
        {
            return Regex.IsMatch(customerName, @"^[a-zA-Z0-9\s]*$");
        }
        private (int, double, double, double) Calculator(string customerType, string member, string lastWater, string thisWater)
        {
            int lastWaterValue = Convert.ToInt32(lastWater);
            int thisWaterValue = Convert.ToInt32(thisWater);
            int waterUser;
            double waterMoney;
            double VAT;
            double totalBillHouseholdCustomer;
            if (customerType.Equals("Household Customer"))
            {
                int memberValue = Convert.ToInt32(member);
                waterUser = thisWaterValue - lastWaterValue;
                double average = waterUser / memberValue;
                if (average >= 0 && average < 10)
                {
                    waterMoney = waterUser * 5973;
                }
                else if (average >= 10 && average < 20)
                {
                    waterMoney = waterUser * 7052;
                }
                else if (average >= 20 && average < 30)
                {
                    waterMoney = waterUser * 8699;
                }
                else
                {
                    waterMoney = waterUser * 22068;
                }
                VAT = waterMoney * 0.1;
                totalBillHouseholdCustomer = waterMoney + VAT;
                return (waterUser, waterMoney, VAT, totalBillHouseholdCustomer);
            }
            else if (customerType.Equals("Administrative Agency"))
            {
                waterUser = thisWaterValue - lastWaterValue;
                waterMoney = waterUser * 9955;
                VAT = waterMoney * 0.1;
                totalBillHouseholdCustomer = waterMoney + VAT;
                return (waterUser, waterMoney, VAT, totalBillHouseholdCustomer);
            }
            else if (customerType.Equals("Production Units"))
            {
                waterUser = thisWaterValue - lastWaterValue;
                waterMoney = waterUser * 11615;
                VAT = waterMoney * 0.1;
                totalBillHouseholdCustomer = waterMoney + VAT;
                return (waterUser, waterMoney, VAT, totalBillHouseholdCustomer);
            }
            else if (customerType.Equals("Business Services"))
            {
                waterUser = thisWaterValue - lastWaterValue;
                waterMoney = waterUser * 22068;
                VAT = waterMoney * 0.1;
                totalBillHouseholdCustomer = waterMoney + VAT;
                return (waterUser, waterMoney, VAT, totalBillHouseholdCustomer);
            }
            return (-1, -1, -1, -1);
        }
        private void btnReturn_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login login = new Login();
            login.ShowDialog();
        }
        private void txtTypeCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            string customerType = txtTypeCustomer.Text.Trim().ToLower();
            if (customerType.Equals("household customer"))
            {
                txtMembers.Enabled = true;
            }
        }
        private void btnPay_Click(object sender, EventArgs e)
        {
            this.Hide();
            PayPage payPage = new PayPage();
            payPage.ShowDialog();
        }
    }
}
