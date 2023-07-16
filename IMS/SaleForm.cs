using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using IMS.Properties;

namespace IMS
{
    public partial class SaleForm : Form
    {
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString);
        public SaleForm()
        {
            InitializeComponent();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void productIdTextBox_KeyUp(object sender, KeyEventArgs e)
        {
          
        }

        private void customerCodeTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                FetchCustomers fetchCustomers = new FetchCustomers();
                fetchCustomers.ShowDialog();
            }
        }

        private void customerCodeTextBox_Leave(object sender, EventArgs e)
        {
            customerCodeTextBox.Text = FetchCustomers.SetCode.ToString();
            customerNameTextBox.Text = FetchCustomers.SetName;
            fatherTextBox.Text = FetchCustomers.SetFatherName;
            cnicTextBox.Text = FetchCustomers.SetCNIC;
            mobileTextBox.Text = FetchCustomers.SetMobile;
            addressTextBox.Text = FetchCustomers.SetAddress;
        }
        private void ClearAllData()
        {
            foreach (TextBox textBox in this.Controls.OfType<TextBox>())
            {
                textBox.Clear();
            }
            foreach (ComboBox comboBox in this.Controls.OfType<ComboBox>())
            {
                comboBox.SelectedIndex = -1;
            }
            productDataGridView.Rows.Clear();
        }

        private void addNewButton_Click(object sender, EventArgs e)
        {
            ClearAllData();
            GetMaxNumber();
        }
        private void GetMaxNumber()
        {
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter("select distinct max(SaleCode) from Sale",connection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            int maxNumber = Convert.ToInt32(dt.Rows[0][0]);
            int newNumber = maxNumber + 1;
            saleNumberTextBox.Text = newNumber.ToString();
            connection.Close();
        }

        private void SaleForm_Load(object sender, EventArgs e)
        {
            ClearAllData();
            GetMaxNumber();
        }
    }
}
