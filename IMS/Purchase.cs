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
    public partial class Purchase : Form
    {
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString);
        public Purchase()
        {
            InitializeComponent();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void GetMaxNumber()
        {
            try
            {
                connection.Open();
                string query = $@"select distinct max(PuchaseCode) from [dbo].[Purchase]";
                SqlDataAdapter da = new SqlDataAdapter(query,connection);
                DataTable dt = new DataTable();
                da.Fill(dt);
                int maxNumber = Convert.ToInt32(dt.Rows[0][0]);
                int newNumber = maxNumber + 1;
                purchaseCodeTextBox.Text = newNumber.ToString();
                connection.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void Purchase_Load(object sender, EventArgs e)
        {
            GetMaxNumber();
            saveButton.Enabled = true;
            updateButton.Enabled = false;
            addButton.Enabled = false;
        }

        private void companyCodeTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                FetchCompany fetchCompany = new FetchCompany();
                fetchCompany.ShowDialog();
            }
        }

        private void companyCodeTextBox_DoubleClick(object sender, EventArgs e)
        {
            FetchCompany fetchCompany = new FetchCompany();
            fetchCompany.ShowDialog();
        }

        private void companyCodeTextBox_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(companyCodeTextBox.Text))
            {
                companyCodeTextBox.Text = FetchCompany.SetCode.ToString();
                companyNameLabel.Text = FetchCompany.SetName;
            }
        }
        private void CheckTextBoxes()
        {
            if (!string.IsNullOrWhiteSpace(productIdTextBox.Text) && !string.IsNullOrWhiteSpace(saleQtyTextBox.Text) && !string.IsNullOrWhiteSpace(priceTextBox.Text) && !string.IsNullOrWhiteSpace(productIdTextBox.Text) && !string.IsNullOrWhiteSpace(totalAmountTextBox.Text) && !string.IsNullOrEmpty(companyCodeTextBox.Text) && !string.IsNullOrWhiteSpace(companyNameLabel.Text))
            {
                addButton.Enabled = true;
            }
            else
            {
                addButton.Enabled = false;
            }
        }

        private void productIdTextBox_TextChanged(object sender, EventArgs e)
        {
            CheckTextBoxes();
        }

        private void saleQtyTextBox_TextChanged(object sender, EventArgs e)
        {
            CheckTextBoxes();
        }

        private void priceTextBox_TextChanged(object sender, EventArgs e)
        {
            CheckTextBoxes();
        }

        private void totalAmountTextBox_TextChanged(object sender, EventArgs e)
        {
            CheckTextBoxes();
        }

        private void companyCodeTextBox_TextChanged(object sender, EventArgs e)
        {
            CheckTextBoxes();
        }

        private void companyNameLabel_Click(object sender, EventArgs e)
        {

        }

        private void companyNameLabel_TextChanged(object sender, EventArgs e)
        {
            CheckTextBoxes();
        }

        private void productIdTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                FetchProduct fetchProduct = new FetchProduct();
                fetchProduct.ShowDialog(); 
            }
        }

        private void productIdTextBox_DoubleClick(object sender, EventArgs e)
        {
            FetchProduct fetchProduct = new FetchProduct();
            fetchProduct.ShowDialog();
        }

        private void productIdTextBox_Leave(object sender, EventArgs e)
        {
            productIdTextBox.Text = FetchProduct.SetCode.ToString();
            productTextBox.Text = FetchProduct.SetDescription;
            currentStockTextBox.Text = FetchProduct.SetQuantity.ToString();
            priceTextBox.Text = FetchProduct.SetPrice.ToString();
        }
    }
}
