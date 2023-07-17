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
            AddButtonEnabled();
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
            addButton.Enabled = false; 
            ClearAllData();
            GetMaxNumber();
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

        private void saleQtyTextBox_Enter(object sender, EventArgs e)
        {
        }

        private void saleQtyTextBox_Leave(object sender, EventArgs e)
        {
            if (saleQtyTextBox.Text.Trim()!=string.Empty && priceTextBox.Text.Trim()!=string.Empty)
            {
                totalAmountTextBox.Text = (Convert.ToDecimal(saleQtyTextBox.Text.Trim()) * Convert.ToDecimal(priceTextBox.Text.Trim())).ToString(); 
            }

        }

        private void priceTextBox_Leave(object sender, EventArgs e)
        {
            if (saleQtyTextBox.Text.Trim() != string.Empty && priceTextBox.Text.Trim() != string.Empty )
            {
                totalAmountTextBox.Text = (Convert.ToDecimal (saleQtyTextBox.Text.Trim()) * Convert.ToDecimal(priceTextBox.Text.Trim())).ToString(); 
            }

        }

        private void addButton_Click(object sender, EventArgs e)
        {
            productDataGridView.Rows.Add(productIdTextBox.Text.Trim(),productTextBox.Text.Trim(),currentStockTextBox.Text.Trim(),saleQtyTextBox.Text.Trim(),priceTextBox.Text.Trim(),totalAmountTextBox.Text.Trim());
            productIdTextBox.Text = string.Empty;
            productTextBox.Text = string.Empty;
            currentStockTextBox.Text = string.Empty;
            saleQtyTextBox.Text = string.Empty;
            priceTextBox.Text = string.Empty;
            totalAmountTextBox.Text = string.Empty;
            totalTextBox.Text = GetTotal().ToString();
            AdvanceAmountAndBalaneAmountCalculation();
            InstallmentCalculation();
           
            
            

        }
        private decimal GetTotal()
        {
            decimal Total = 0;
            foreach (DataGridViewRow row in productDataGridView.Rows)
            {
              Total += Convert.ToDecimal( row.Cells["Amount"].Value);
            }
            return Math.Round( Total,0);
        }

        private void productDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (productDataGridView.Columns[e.ColumnIndex].HeaderText == "Remove")
                {
                    productDataGridView.Rows.RemoveAt(e.RowIndex);
                }
                if (e.RowIndex>0)
                {
                    totalTextBox.Text = GetTotal().ToString();
                    AdvanceAmountAndBalaneAmountCalculation();
                    InstallmentCalculation();

                }
                else
                {
                    totalTextBox.Text = string.Empty;
                    advanceTextBox.Text = string.Empty;
                    monthTextBox.Text = string.Empty;
                    balanceTextBox.Text=string.Empty;
                    installmentTextBox.Text=string.Empty;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Please remove this field from remove button" + ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
           
        }

        private void advanceTextBox_Leave(object sender, EventArgs e)
        {
            AdvanceAmountAndBalaneAmountCalculation();
        }

        private void AdvanceAmountAndBalaneAmountCalculation()
        {
            if (totalTextBox.Text.Trim() != string.Empty && advanceTextBox.Text.Trim() != string.Empty)
            {
                balanceTextBox.Text = (Convert.ToDecimal(totalTextBox.Text.Trim()) - Convert.ToDecimal(advanceTextBox.Text.Trim())).ToString();
            }
        }

        private void monthTextBox_Leave(object sender, EventArgs e)
        {
            InstallmentCalculation();
        }

        private void InstallmentCalculation()
        {
            if (balanceTextBox.Text != string.Empty)
            {
                installmentTextBox.Text = (Math.Round((Convert.ToDecimal(balanceTextBox.Text.Trim())) / Convert.ToDecimal(monthTextBox.Text.Trim()),0)).ToString();
            }
        }

        private void productIdTextBox_TextChanged(object sender, EventArgs e)
        {
            AddButtonEnabled();

        }

        private void AddButtonEnabled()
        {
            if (productIdTextBox.Text.Trim() != String.Empty && saleQtyTextBox.Text.Trim() != string.Empty && priceTextBox.Text.Trim() != string.Empty && totalAmountTextBox.Text.Trim() != string.Empty)
            {
                addButton.Enabled = true;
            }
        }

        private void totalAmountTextBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
