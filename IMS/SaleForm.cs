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
            if (productDataGridView.Rows.Count!=0)
            {
                UpdateBackQuantityInProductTable(); 
            }
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

            if (productDataGridView.Rows.Count!=0)
            {
                UpdateBackQuantityInProductTable(); 
            }
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

            if (Convert.ToInt32( currentStockTextBox.Text.Trim()) >=Convert.ToInt32( saleQtyTextBox.Text.Trim()))
            {
                productDataGridView.Rows.Add(productIdTextBox.Text.Trim(), productTextBox.Text.Trim(), currentStockTextBox.Text.Trim(), saleQtyTextBox.Text.Trim(), priceTextBox.Text.Trim(), totalAmountTextBox.Text.Trim());
                UpdateStockInProductTable();
                productIdTextBox.Text = string.Empty;
                productTextBox.Text = string.Empty;
                currentStockTextBox.Text = string.Empty;
                saleQtyTextBox.Text = string.Empty;
                saleQtyTextBox.BackColor = Color.White;
                priceTextBox.Text = string.Empty;
                totalAmountTextBox.Text = string.Empty;
                totalTextBox.Text = GetTotal().ToString();
                AdvanceAmountAndBalaneAmountCalculation();
                InstallmentCalculation(); 
            }
            else
            {
                MessageBox.Show("Sale Quantity cannot be greater than Current Stock","Information",MessageBoxButtons.OK,MessageBoxIcon.Information);
                saleQtyTextBox.BackColor = Color.Aqua;
                saleQtyTextBox.Focus();
            }
           
            
            

        }

        private void UpdateStockInProductTable()
        {
            if (!string.IsNullOrEmpty(productIdTextBox.Text.Trim()))
            {
                SqlTransaction transaction1 = null;
                try
                {
                    int reducedQuantity = Convert.ToInt32(currentStockTextBox.Text.Trim()) - Convert.ToInt32(saleQtyTextBox.Text.Trim());
                    connection.Open();
                    transaction1 = connection.BeginTransaction();
                    string query = "UPDATE Product SET Quantity=@Quantity WHERE ProductID=@ProductID";
                    SqlCommand command = new SqlCommand(query, connection, transaction1);
                    command.Parameters.AddWithValue("@Quantity", reducedQuantity);
                    command.Parameters.AddWithValue("@ProductID", Convert.ToInt32(productIdTextBox.Text.Trim()));
                    command.ExecuteNonQuery();
                    transaction1.Commit();
                    MessageBox.Show("Quantity sold is reduced in stock","Information",MessageBoxButtons.OK,MessageBoxIcon.Information);
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
                    UpdateBackQuantityInProductTable();
                    productDataGridView.Rows.RemoveAt(e.RowIndex);
                    if (e.RowIndex > 0)
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
                        balanceTextBox.Text = string.Empty;
                        installmentTextBox.Text = string.Empty;
                    }
                }
                
            }
            catch (Exception ex)
            {

                MessageBox.Show("Please remove this field from remove button" + ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
           
        }

        private void UpdateBackQuantityInProductTable()
        {
            SqlTransaction transaction2 = null;
            try
            {
                string query1 = "SELECT ProductID,Quantity FROM Product WHERE ProductID=@ProductID";
                string query2 = "UPDATE Product SET Quantity=@Quantity WHERE ProductID=@ProductID";
                connection.Open();
                transaction2 = connection.BeginTransaction();
                SqlCommand command = new SqlCommand(query1,connection,transaction2);
                command.Parameters.AddWithValue("@ProductID", Convert.ToInt32(productDataGridView.CurrentRow.Cells["PID"].Value));
                SqlDataAdapter da = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                da.Fill(dt);
                int id = Convert.ToInt32( dt.Rows[0][0]);
                int qty = Convert.ToInt32(dt.Rows[0][1]);
                int increasedQty = qty + Convert.ToInt32( productDataGridView.CurrentRow.Cells["SaleQty"].Value);
                SqlCommand cmd = new SqlCommand(query2,connection,transaction2);
                cmd.Parameters.AddWithValue("@Quantity", increasedQty);
                cmd.Parameters.AddWithValue("@ProductID",id);
                cmd.ExecuteNonQuery();
                transaction2.Commit();

                MessageBox.Show("Removed quantity is added back","Information",MessageBoxButtons.OK,MessageBoxIcon.Information);



            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                transaction2.Rollback();
            }
            finally
            {
                connection.Close();
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
            CheckTextBoxes();

        }

        private void AddButtonEnabled()
        {
            //if (productIdTextBox.Text.Trim() != String.Empty && saleQtyTextBox.Text.Trim() != string.Empty && priceTextBox.Text.Trim() != string.Empty && totalAmountTextBox.Text.Trim() != string.Empty)
            //{
            //    addButton.Enabled = true;
            //}
        }

        private void totalAmountTextBox_TextChanged(object sender, EventArgs e)
        {
            CheckTextBoxes();
        }
        private void CheckTextBoxes()
        {
            if (!string.IsNullOrWhiteSpace(productIdTextBox.Text) && !string.IsNullOrWhiteSpace(saleQtyTextBox.Text) && !string.IsNullOrWhiteSpace(priceTextBox.Text) && !string.IsNullOrWhiteSpace(productIdTextBox.Text) && !string.IsNullOrWhiteSpace(totalAmountTextBox.Text) && !string.IsNullOrEmpty(customerCodeTextBox.Text) && !string.IsNullOrWhiteSpace(customerNameTextBox.Text))
            {
                addButton.Enabled = true;
            }
            else
            {
                addButton.Enabled = false;
            }
        }

        private void saleQtyTextBox_TextChanged(object sender, EventArgs e)
        {
            CheckTextBoxes();
        }

        private void priceTextBox_TextChanged(object sender, EventArgs e)
        {
            CheckTextBoxes();
        }

        private void customerCodeTextBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            FetchCustomers fetchCustomers = new FetchCustomers();
            fetchCustomers.ShowDialog();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            SqlTransaction transaction = null;
            if (string.IsNullOrWhiteSpace(saleNumberTextBox.Text))
            {
                MessageBox.Show("Sale Invoice Number must not be Empty","Information",MessageBoxButtons.OK,MessageBoxIcon.Information);
                saleNumberTextBox.Focus();
                return;
            }
            if (saleCategoryComboBox.SelectedIndex==-1)
            {
                MessageBox.Show("Sale Category must be selected", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                saleCategoryComboBox.Focus();
                return;

            }
            if (string.IsNullOrEmpty(customerCodeTextBox.Text))
            {
                MessageBox.Show("Customer Code must be entered", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                customerCodeTextBox.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(customerNameTextBox.Text))
            {
                MessageBox.Show("Please Enter Customer Name", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                customerNameTextBox.Focus();
                return; 
            }
            if (string.IsNullOrWhiteSpace(mobileTextBox.Text))
            {
                MessageBox.Show("Please Enter Customer Mobile Number", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                mobileTextBox.Focus();
                return;
            }
            if (productDataGridView.Rows.Count==0)
            {
                MessageBox.Show("Please enter any Product for further Processing", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                productIdTextBox.Focus();
                return;
            }
            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();
                string voucherCatCode = saleNumber.Text.Trim() + " " + saleNumberTextBox.Text.Trim();
                int VCodeCash = 10002;
                string Description = "Cash";
                int installmentSaleCode = 10017;
                string installmentSaleDescription = "Installment Sales Revenue";
                int salesRevenue = 10004;
                string salesRevenueDescription = "Sales Revenue";


                foreach (DataGridViewRow row in productDataGridView.Rows)
                {
                    SqlCommand cmd = new SqlCommand("SP_INSERT_SALE", connection,transaction);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SaleCode", saleNumberTextBox.Text.Trim());
                    cmd.Parameters.AddWithValue("@Date", invoiceDateTimePicker.Value);
                    cmd.Parameters.AddWithValue("@CustomerID", Convert.ToInt32( customerCodeTextBox.Text.Trim()));
                    cmd.Parameters.AddWithValue("@ProductID", Convert.ToInt32( row.Cells["PID"].Value)); 
                    cmd.Parameters.AddWithValue("@SaleCategory", saleCategoryComboBox.SelectedItem); 
                    cmd.Parameters.AddWithValue("@Qty", Convert.ToInt32( row.Cells["SaleQty"].Value)); 
                    cmd.Parameters.AddWithValue("@Price", Convert.ToDecimal( row.Cells["Rate"].Value)); 
                    cmd.Parameters.AddWithValue("@SaleAmount", Convert.ToDecimal( row.Cells["Amount"].Value)); 
                    cmd.Parameters.AddWithValue("@TotalInvoiceAmount", Convert.ToDecimal( totalTextBox.Text.Trim())); 
                    cmd.Parameters.AddWithValue("@Advance",   advanceTextBox.Text.Trim()==string.Empty?(object)DBNull.Value : Convert.ToDecimal (advanceTextBox.Text.Trim())); 
                    cmd.Parameters.AddWithValue("@BalanceAmount",  balanceTextBox.Text.Trim()==string.Empty?(object)DBNull.Value: Convert.ToDecimal ( balanceTextBox.Text.Trim())); 
                    cmd.Parameters.AddWithValue("@Months",    monthTextBox.Text.Trim()==string.Empty?(object)DBNull.Value: Convert.ToInt32( monthTextBox.Text.Trim())); 
                    cmd.Parameters.AddWithValue("@InstallmentAmount", installmentTextBox.Text.Trim()==string.Empty?(object)DBNull.Value:Convert.ToDecimal( installmentTextBox.Text.Trim()));
                    cmd.ExecuteNonQuery();
                }
               
                foreach (DataGridViewRow row in productDataGridView.Rows)
                {
                    SqlCommand cmd2 = new SqlCommand("INSERT INTO [dbo].[Stock] (Date,SaleID,ProductID,CustomerID,SoldQuantity)VALUES(@Date,@SaleID,@ProductID,@CustomerID,@SoldQuantity)", connection, transaction);
                    cmd2.Parameters.AddWithValue("@Date", invoiceDateTimePicker.Value);
                    cmd2.Parameters.AddWithValue("@SaleID", Convert.ToInt32(saleNumberTextBox.Text.Trim()));
                    cmd2.Parameters.AddWithValue("@ProductID", row.Cells["PID"].Value);
                    cmd2.Parameters.AddWithValue("@CustomerID", Convert.ToInt32(customerCodeTextBox.Text.Trim()));
                    cmd2.Parameters.AddWithValue("@SoldQuantity",row.Cells["SaleQty"].Value );
                    cmd2.ExecuteNonQuery(); 
                }

                if (!string.IsNullOrWhiteSpace(advanceTextBox.Text.Trim()))
                {
                    
                    SqlCommand cmd3 = new SqlCommand("INSERT INTO [dbo].[TransactionTable] (VoucherCode,VoucherType,VoucherDate,Description,VoucherCategoryID,VoucherCategory,VoucherCategoryCode,Debit)VALUES(@VoucherCode,@VoucherType,@VoucherDate,@Description,@VoucherCategoryID,@VoucherCategory,@VoucherCategoryCode,@Debit)", connection, transaction);
                    cmd3.Parameters.AddWithValue("@VoucherCode", Convert.ToInt32(saleNumberTextBox.Text.Trim()));
                    cmd3.Parameters.AddWithValue("@VoucherType",saleNumber.Text.Trim());
                    cmd3.Parameters.AddWithValue("@VoucherDate",invoiceDateTimePicker.Value);
                    cmd3.Parameters.AddWithValue("@Description", Description);
                    cmd3.Parameters.AddWithValue("@VoucherCategoryID", VCodeCash);
                    cmd3.Parameters.AddWithValue("@VoucherCategory", saleCategoryComboBox.SelectedItem);
                    cmd3.Parameters.AddWithValue("@VoucherCategoryCode",voucherCatCode);
                    cmd3.Parameters.AddWithValue("@Debit", advanceTextBox.Text.Trim()==string.Empty?(object)DBNull.Value: Convert.ToDecimal(advanceTextBox.Text.Trim()));
                    cmd3.ExecuteNonQuery();
                    SqlCommand cmd4 = new SqlCommand("INSERT INTO [dbo].[TransactionTable] (VoucherCode,VoucherType,VoucherDate,Description,VoucherCategoryID,VoucherCategory,VoucherCategoryCode,Debit)VALUES(@VoucherCode,@VoucherType,@VoucherDate,@Description,@VoucherCategoryID,@VoucherCategory,@VoucherCategoryCode,@Debit)", connection, transaction);
                    cmd4.Parameters.AddWithValue("@VoucherCode", Convert.ToInt32(saleNumberTextBox.Text.Trim()));
                    cmd4.Parameters.AddWithValue("@VoucherType", saleNumber.Text.Trim());
                    cmd4.Parameters.AddWithValue("@VoucherDate", invoiceDateTimePicker.Value);
                    cmd4.Parameters.AddWithValue("@VoucherCategoryID", Convert.ToInt32(customerCodeTextBox.Text.Trim()));
                    cmd4.Parameters.AddWithValue("@VoucherCategory", saleCategoryComboBox.SelectedItem);
                    cmd4.Parameters.AddWithValue("@VoucherCategoryCode", voucherCatCode);
                    cmd4.Parameters.AddWithValue("@Description", customerNameTextBox.Text.Trim());
                    cmd4.Parameters.AddWithValue("@Debit", balanceTextBox.Text.Trim() == string.Empty ? (object)DBNull.Value : Convert.ToDecimal(balanceTextBox.Text.Trim()));
                    cmd4.ExecuteNonQuery();

                    SqlCommand cmd5 = new SqlCommand("INSERT INTO [dbo].[TransactionTable] (VoucherCode,VoucherType,VoucherDate,Description,VoucherCategoryID,VoucherCategory,VoucherCategoryCode,Credit)VALUES(@VoucherCode,@VoucherType,@VoucherDate,@Description,@VoucherCategoryID,@VoucherCategory,@VoucherCategoryCode,@Credit)", connection, transaction);
                    cmd5.Parameters.AddWithValue("@VoucherCode", Convert.ToInt32(saleNumberTextBox.Text.Trim()));
                    cmd5.Parameters.AddWithValue("@VoucherType", saleNumber.Text.Trim());
                    cmd5.Parameters.AddWithValue("@VoucherDate", invoiceDateTimePicker.Value);
                    cmd5.Parameters.AddWithValue("@VoucherCategoryID", installmentSaleCode);
                    cmd5.Parameters.AddWithValue("@VoucherCategory", saleCategoryComboBox.SelectedItem);
                    cmd5.Parameters.AddWithValue("@VoucherCategoryCode", voucherCatCode);
                    cmd5.Parameters.AddWithValue("@Description", installmentSaleDescription);
                    cmd5.Parameters.AddWithValue("@Credit", totalTextBox.Text.Trim() == string.Empty ? (object)DBNull.Value : Convert.ToDecimal(totalTextBox.Text.Trim()));
                    cmd5.ExecuteNonQuery();


                }
                else
                {
                    SqlCommand cmd1 = new SqlCommand("[SP_INSERT_SALE_VOUCHER]", connection, transaction);
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.AddWithValue("@VoucherCode", Convert.ToInt32(saleNumberTextBox.Text.Trim()));
                    cmd1.Parameters.AddWithValue("@VoucherType", saleNumber.Text.Trim());
                    cmd1.Parameters.AddWithValue("@VoucherDate", invoiceDateTimePicker.Value);
                    cmd1.Parameters.AddWithValue("@VoucherCategoryID", VCodeCash);
                    cmd1.Parameters.AddWithValue("@VoucherCategory", saleCategoryComboBox.SelectedItem);
                    cmd1.Parameters.AddWithValue("@VoucherCategoryCode", voucherCatCode);
                    cmd1.Parameters.AddWithValue("@Description", Description);
                    cmd1.Parameters.AddWithValue("@Debit", totalTextBox.Text.Trim() == string.Empty ? (object)DBNull.Value : Convert.ToDecimal(totalTextBox.Text.Trim()));
                    cmd1.ExecuteNonQuery();

                    SqlCommand cmd6 = new SqlCommand("INSERT INTO [dbo].[TransactionTable] (VoucherCode,VoucherType,VoucherDate,VoucherCategoryID,VoucherCategory,VoucherCategoryCode,Description,Credit)VALUES(@VoucherCode,@VoucherType,@VoucherDate,@VoucherCategoryID,@VoucherCategory,@VoucherCategoryCode,@Description,@Credit)", connection, transaction);
                    cmd6.Parameters.AddWithValue("@VoucherCode", Convert.ToInt32(saleNumberTextBox.Text.Trim()));
                    cmd6.Parameters.AddWithValue("@VoucherType", saleNumber.Text.Trim());
                    cmd6.Parameters.AddWithValue("@VoucherDate", invoiceDateTimePicker.Value);
                    cmd6.Parameters.AddWithValue("@VoucherCategoryID", salesRevenue);
                    cmd6.Parameters.AddWithValue("@VoucherCategory", saleCategoryComboBox.SelectedItem);
                    cmd6.Parameters.AddWithValue("@VoucherCategoryCode", voucherCatCode);
                    cmd6.Parameters.AddWithValue("@Description", salesRevenueDescription);
                    cmd6.Parameters.AddWithValue("@Credit", totalTextBox.Text.Trim() == string.Empty ? (object)DBNull.Value : Convert.ToDecimal(totalTextBox.Text.Trim()));
                    cmd6.ExecuteNonQuery();
                }
                transaction.Commit();
                MessageBox.Show("The Transaction is successfull","Sucess",MessageBoxButtons.OK,MessageBoxIcon.Information);
                connection.Close();
                addNewButton.PerformClick();
            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.Message,"Failure",MessageBoxButtons.OK,MessageBoxIcon.Error);
                transaction.Rollback();
            }
            finally
            {
                connection.Close();
            }


        }

        private void customerCodeTextBox_TextChanged(object sender, EventArgs e)
        {
            CheckTextBoxes();
        }

        private void customerNameTextBox_TextChanged(object sender, EventArgs e)
        {
            CheckTextBoxes();
        }

        private void saleCategoryComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (saleCategoryComboBox.SelectedIndex==0)
            {
                advanceTextBox.ReadOnly=true;
                monthTextBox.ReadOnly=true;

            }
            if(saleCategoryComboBox.SelectedIndex==1)
            {
                advanceTextBox.ReadOnly = true;
                monthTextBox.ReadOnly = true;
            }
            if (saleCategoryComboBox.SelectedIndex==2)
            {
                advanceTextBox.ReadOnly = false;
                monthTextBox.ReadOnly = false;
            }
        }

        private void saleQtyTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void priceTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void advanceTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void monthTextBox_TextChanged(object sender, EventArgs e)
        {
            //if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            //{
            //    e.Handled = true;
            //}
        }

        private void monthTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
