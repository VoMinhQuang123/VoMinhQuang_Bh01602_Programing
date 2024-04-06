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
        }
        // Example method where you want to assign the values
        private void btnGetBill_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string customerType = txtTypeCustomer.Text.Trim().ToLower();
            string member = txtMembers.Text.Trim();
            string lastWater = txtLastMonthWater.Text.Trim();
            string thisWater = txtThisMonthWater.Text.Trim();
            bool first = false;
            bool second = false;
            if (customerType.Equals("household customer"))
            {
                if (name != "" && checkName1(name))
                {
                    if (member != "" && int.TryParse(member, out int memberValue))
                    {
                        if(memberValue > 0)
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
                else if(name == "")
                {
                    MessageBox.Show("The customer name cannot be left blank!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Please enter a valid name!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (customerType.Equals("administrative agency") || customerType.Equals("production units") || customerType.Equals("business services"))
            {
                if (name != "" && !checkName2(name))
                {
                    MessageBox.Show("Please enter a valid name!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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
                if (lastWater != "" && thisWater != "" &&  int.TryParse(lastWater, out int lastWaterValue) && int.TryParse(thisWater, out int thisWaterValue))
                {
                    if(thisWaterValue >= lastWaterValue)
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
                lsvMessage.Enabled = true;
                lsvMessage.Items.Clear();
                lsvMessage.View = View.Details;
                lsvMessage.Columns.Add("Property", 200);
                lsvMessage.Columns.Add("Value", 200);

                // Tạo các mục trong ListView
                ListViewItem waterUsageItem = new ListViewItem("Water Usage");
                waterUsageItem.SubItems.Add(billGet.Item1.ToString());
                lsvMessage.Items.Add(waterUsageItem);

                ListViewItem waterChargeItem = new ListViewItem("Water Charge");
                waterChargeItem.SubItems.Add(billGet.Item2.ToString());
                lsvMessage.Items.Add(waterChargeItem);

                ListViewItem vatItem = new ListViewItem("VAT");
                vatItem.SubItems.Add(billGet.Item3.ToString());
                lsvMessage.Items.Add(vatItem);

                ListViewItem totalBillItem = new ListViewItem("Total Bill");
                totalBillItem.SubItems.Add(billGet.Item4.ToString());
                lsvMessage.Items.Add(totalBillItem);
            }
        }
        static bool checkName1(string customerName)
        {
            return Regex.IsMatch(customerName, @"^[a-zA-Z\s]*$");
        }
        static bool checkName2(string customerName)
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
            if (customerType.Equals("household customer"))
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
            else if (customerType.Equals("administrative agency"))
            {
                waterUser = thisWaterValue - lastWaterValue;
                waterMoney = waterUser * 9955;
                VAT = waterMoney * 0.1;
                totalBillHouseholdCustomer = waterMoney + VAT;
                return (waterUser, waterMoney, VAT, totalBillHouseholdCustomer);
            }
            else if (customerType.Equals("production units"))
            {
                waterUser = thisWaterValue - lastWaterValue;
                waterMoney = waterUser * 11615;
                VAT = waterMoney * 0.1;
                totalBillHouseholdCustomer = waterMoney + VAT;
                return (waterUser, waterMoney, VAT, totalBillHouseholdCustomer);
            }
            else if (customerType.Equals("business services"))
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
            if(customerType.Equals("household customer"))
            {
                txtMembers.Enabled = true;
            }
            else
            {
                txtMembers.Enabled = false;
                txtMembers.Text = null;
            }
        }
    }
}
