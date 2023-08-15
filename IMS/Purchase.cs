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
            purchaseDateTimePicker.Value = System.DateTime.Now;
            
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

        private void saleQtyTextBox_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(saleQtyTextBox.Text))
            {
                decimal PurchasedQty = Convert.ToDecimal(saleQtyTextBox.Text);
                decimal Price = Convert.ToDecimal(priceTextBox.Text);
                totalAmountTextBox.Text = (PurchasedQty * Price).ToString();
            }
        }

        private void priceTextBox_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(priceTextBox.Text))
            {
                decimal PurchasedQty = Convert.ToDecimal(saleQtyTextBox.Text);
                decimal Price = Convert.ToDecimal(priceTextBox.Text);
                totalAmountTextBox.Text = (PurchasedQty * Price).ToString();
            }
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            if (purchaseDateTimePicker.Value.Date!=DateTime.Now.Date)
            {
                MessageBox.Show("You cannot save or update previous date Records","Information",MessageBoxButtons.OK,MessageBoxIcon.Information);
                companyCodeTextBox.Focus();
                return;
            }
            purchaseDataGridView.DataSource = null;
            purchaseDataGridView.Rows.Add(productIdTextBox.Text.Trim(), productTextBox.Text.Trim(), currentStockTextBox.Text.Trim(), saleQtyTextBox.Text.Trim(), priceTextBox.Text.Trim(), totalAmountTextBox.Text.Trim());
            UpdateStockInProductTable();
            productIdTextBox.Text = string.Empty;
            productTextBox.Text = string.Empty;
            currentStockTextBox.Text = string.Empty;
            saleQtyTextBox.Text = string.Empty;
            priceTextBox.Text = string.Empty;
            totalAmountTextBox.Text = string.Empty;
            grandTotalTextBox.Text = GetGrandTotal().ToString();
            
        }


        private void UpdateStockInProductTable()
        {
            if (!string.IsNullOrEmpty(productIdTextBox.Text.Trim()))
            {
                SqlTransaction transaction1 = null;
                try
                {
                    int increasedQuantity = Convert.ToInt32(currentStockTextBox.Text.Trim()) + Convert.ToInt32(saleQtyTextBox.Text.Trim());
                    connection.Open();
                    transaction1 = connection.BeginTransaction();
                    string query = "UPDATE Product SET Quantity=@Quantity WHERE ProductID=@ProductID";
                    SqlCommand command = new SqlCommand(query, connection, transaction1);
                    command.Parameters.AddWithValue("@Quantity", increasedQuantity);
                    command.Parameters.AddWithValue("@ProductID", Convert.ToInt32(productIdTextBox.Text.Trim()));
                    command.ExecuteNonQuery();
                    transaction1.Commit();
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                    transaction1.Rollback();
                    connection.Close();
                }
                finally
                {
                    connection.Close();
                }

            }
        }
        
        private decimal GetGrandTotal()
        {
            decimal Total = 0;
            foreach (DataGridViewRow row in purchaseDataGridView.Rows)
            {
                Total +=Convert.ToDecimal( row.Cells["TotalAmount"].Value);
            }
            return Math.Round( Total,0);
        }
	
    }

}
