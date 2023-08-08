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
            productDataGridView.DataSource = null;
            productDataGridView.Rows.Clear();
        }

        private void addNewButton_Click(object sender, EventArgs e)
        {
            

            if (productDataGridView.Rows.Count != 0 )
            {
                UpdateBackQuantityInProductTable(); 
            }
            ClearAllData();
            GetMaxNumber();
            AddButtonEnabled();
            invoiceDateTimePicker.Value = DateTime.Now;
            saveButton.Enabled = true;
            updateButton.Enabled = false;
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
            saveButton.Enabled = true;
            updateButton.Enabled = false;
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
            try
            {
                if (saleQtyTextBox.Text.Trim() != string.Empty && priceTextBox.Text.Trim() != string.Empty)
                {
                    totalAmountTextBox.Text = (Convert.ToDecimal(saleQtyTextBox.Text.Trim()) * Convert.ToDecimal(priceTextBox.Text.Trim())).ToString();
                }
                if (Convert.ToInt32(currentStockTextBox.Text.Trim()) >= Convert.ToInt32(saleQtyTextBox.Text.Trim()))
                {
                    saleQtyTextBox.BackColor = Color.White;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
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
            if (invoiceDateTimePicker.Value.Date!=DateTime.Now.Date)
            {
                MessageBox.Show("You cannot Add Records to the Previous date","Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                addNewButton.PerformClick();
                customerCodeTextBox.Focus();
                return;
            }
            int currentNumber =Convert.ToInt32( saleNumberTextBox.Text);
            int saleNumber = 0;

            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter("Select distinct MAX(SaleCode) from Sale",connection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                saleNumber =Convert.ToInt32( dt.Rows[0][0]);
            }
            connection.Close();

            if (saleNumber >= currentNumber && Convert.ToInt32(currentStockTextBox.Text.Trim()) >= Convert.ToInt32(saleQtyTextBox.Text.Trim())  )
            {
                // Assuming your DataGridView is bound to a DataTable (for example)
                DataTable dataTable = (DataTable)productDataGridView.DataSource;
                DataRow newRow = dataTable.NewRow();
                newRow["PID"] = productIdTextBox.Text.Trim();
                newRow["Product"] = productTextBox.Text.Trim();
                newRow["CurrentStock"] = currentStockTextBox.Text.Trim();
                newRow["SoldStock"] = saleQtyTextBox.Text.Trim();
                newRow["Rate"] = priceTextBox.Text.Trim();
                newRow["Amount"] = totalAmountTextBox.Text.Trim();

                // Set values for the new row, e.g., newRow["Column1"] = value1; newRow["Column2"] = value2;
                dataTable.Rows.Add(newRow);
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
                productDataGridView.Refresh();
                return;
            }

            if (currentNumber > saleNumber && Convert.ToInt32( currentStockTextBox.Text.Trim()) >=Convert.ToInt32( saleQtyTextBox.Text.Trim()))
            {
                productDataGridView.DataSource = null;
                productDataGridView.Rows.Add(productIdTextBox.Text.Trim(), productTextBox.Text.Trim(), currentStockTextBox.Text.Trim(), saleQtyTextBox.Text.Trim(), priceTextBox.Text.Trim(), totalAmountTextBox.Text.Trim() );
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

                if (invoiceDateTimePicker.Value.Date!=DateTime.Now.Date)
            {
                MessageBox.Show("You cannot Remove records from previous date", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                addNewButton.PerformClick();
                customerCodeTextBox.Focus();
                return;
            }

                object SaleID = productDataGridView.Rows[e.RowIndex].Cells["SaleID"].Value == null || productDataGridView.Rows[e.RowIndex].Cells["SaleID"].Value == DBNull.Value || string.IsNullOrEmpty(productDataGridView.Rows[e.RowIndex].Cells["SaleID"].Value.ToString()) ? Convert.ToInt32(0) : Convert.ToInt32(productDataGridView.Rows[e.RowIndex].Cells["SaleID"].Value);
                int ID = Convert.ToInt32(SaleID);
               
                    if (productDataGridView.Columns[e.ColumnIndex].HeaderText == "Remove")
                    {
                    if (ID == 0)
                    {
                        
                            UpdateBackQuantityInProductTableWithoutForLoop();
                            productDataGridView.Rows.RemoveAt(e.RowIndex);
                            productIdTextBox.Text = string.Empty;
                            productTextBox.Text = string.Empty;
                            currentStockTextBox.Text = string.Empty;
                            saleQtyTextBox.Text = string.Empty;
                            priceTextBox.Text = string.Empty;
                            totalAmountTextBox.Text = string.Empty;
                            this.ControlBox = false;
                            closeButton.Enabled = false;
                            addNewButton.Enabled = false;
                            updateButton.Enabled = true;
                            totalTextBox.Text = GetTotal().ToString();
                            AdvanceAmountAndBalaneAmountCalculation();
                            InstallmentCalculation();

                    } 
                    }
                    if (ID!=0)
                    {
                        

                            UpdateBackQuantityInProductTableWithoutForLoop();

                            connection.Open();
                            int id1 = Convert.ToInt32(productDataGridView.Rows[e.RowIndex].Cells["SaleID"].Value);
                            string query1 = $@"DELETE FROM Sale WHERE SaleID=@SaleID";
                            SqlCommand cmd1 = new SqlCommand(query1, connection);
                            cmd1.Parameters.AddWithValue("@SaleID", id1);
                            cmd1.ExecuteNonQuery();
                            connection.Close();


                            connection.Open();
                            int id2 = Convert.ToInt32(productDataGridView.Rows[e.RowIndex].Cells["PID"].Value);
                            string PID = saleNumberTextBox.Text.ToString().Trim();
                            string query2 = $@"DELETE FROM Stock WHERE ProductID=@ProductId and SaleID=@SaleID ";
                            SqlCommand cmd2 = new SqlCommand(query2, connection);
                            cmd2.Parameters.AddWithValue("@ProductID", id2);
                            cmd2.Parameters.AddWithValue("@SaleID", PID);
                            cmd2.ExecuteNonQuery();
                            connection.Close();


                            productDataGridView.Rows.RemoveAt(e.RowIndex);
                            productIdTextBox.Text = string.Empty;
                            productTextBox.Text = string.Empty;
                            currentStockTextBox.Text = string.Empty;
                            saleQtyTextBox.Text = string.Empty;
                            priceTextBox.Text = string.Empty;
                            totalAmountTextBox.Text = string.Empty;
                            this.ControlBox = false;
                            closeButton.Enabled = false;
                            addNewButton.Enabled = false;
                            updateButton.Enabled = true;
                            totalTextBox.Text = GetTotal().ToString();
                            AdvanceAmountAndBalaneAmountCalculation();
                            InstallmentCalculation();


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
                connection.Close();
            }
           
        }

        private void UpdateBackQuantityInProductTableWithoutForLoop()
        {
            connection.Open();
            try
            {
               
                    string query1 = "SELECT ProductID,Quantity FROM Product WHERE ProductID=@ProductID";
                    string query2 = "UPDATE Product SET Quantity=@Quantity WHERE ProductID=@ProductID";


                    SqlCommand command = new SqlCommand(query1, connection);
                    command.Parameters.AddWithValue("@ProductID", Convert.ToInt32(productDataGridView.CurrentRow.Cells["PID"].Value));
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    da.Fill(dt);


                    int id = Convert.ToInt32(dt.Rows[0][0]);
                    int qty = Convert.ToInt32(dt.Rows[0][1]);
                    int increasedQty = qty + Convert.ToInt32(productDataGridView.CurrentRow.Cells["SaleQty"].Value);

                    SqlCommand cmd = new SqlCommand(query2, connection);
                    cmd.Parameters.AddWithValue("@Quantity", increasedQty);
                    cmd.Parameters.AddWithValue("@ProductID", id);
                    cmd.ExecuteNonQuery();


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                connection.Close();
            }
            finally
            {
                connection.Close();
            }
        }

        private void UpdateBackQuantityInProductTable()
        {
            connection.Open();
            try
            {
                for (int i = 0; i < productDataGridView.Rows.Count; i++)
                {
                    string query1 = "SELECT ProductID,Quantity FROM Product WHERE ProductID=@ProductID";
                    string query2 = "UPDATE Product SET Quantity=@Quantity WHERE ProductID=@ProductID";
                    
                    
                    SqlCommand command = new SqlCommand(query1, connection);
                    command.Parameters.AddWithValue("@ProductID", Convert.ToInt32(productDataGridView.Rows[i].Cells["PID"].Value));
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    

                    int id = Convert.ToInt32(dt.Rows[0][0]);
                    int qty = Convert.ToInt32(dt.Rows[0][1]);
                    int increasedQty = qty + Convert.ToInt32(productDataGridView.Rows[i].Cells["SaleQty"].Value);
                    
                    SqlCommand cmd = new SqlCommand(query2, connection);
                    cmd.Parameters.AddWithValue("@Quantity", increasedQty);
                    cmd.Parameters.AddWithValue("@ProductID", id);
                    cmd.ExecuteNonQuery();
                    
                }

            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.Message);
                connection.Close();
            }
            finally
            {
                connection.Close();
            }
        }

        private void advanceTextBox_Leave(object sender, EventArgs e)
        {
            AdvanceAmountAndBalaneAmountCalculation();
            monthTextBox.Text = "1";
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
            try
            {
                

                if (balanceTextBox.Text != string.Empty && (Convert.ToInt32( (monthTextBox.Text))!=0 || !string.IsNullOrEmpty(monthTextBox.Text.Trim())))
                {
                    installmentTextBox.Text = (Math.Round((Convert.ToDecimal(balanceTextBox.Text.Trim())) / Convert.ToDecimal(monthTextBox.Text.Trim()), 0)).ToString();
                }
               
            }
            catch (Exception ex)
            {

                MessageBox.Show("Month could not be empty and "+ex.Message+"","Information",MessageBoxButtons.OK,MessageBoxIcon.Information);
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
            if (invoiceDateTimePicker.Value.Date!=DateTime.Now.Date)
            {
                MessageBox.Show("You cannot Insert Previous Date Record", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                customerCodeTextBox.Focus();
                return;
            }
            if (saleCategoryComboBox.SelectedIndex==2)
            {
                if (advanceTextBox.Text==string.Empty)
                {
                    MessageBox.Show("Please enter advance amount and number of months","Information",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    advanceTextBox.Focus();
                    return;
                }
            }
            

            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();
                string voucherCatCode = saleNumber.Text.Trim() + " " + saleNumberTextBox.Text.Trim();
                int VCodeCash = 10002;
                string Description = "Cash";
                string creditDescription = customerNameTextBox.Text.Trim();
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

                if (saleCategoryComboBox.SelectedIndex==2)
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
                if(saleCategoryComboBox.SelectedIndex==0)
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
                if (saleCategoryComboBox.SelectedIndex==1)
                {
                    string query7 = $@"INSERT INTO TransactionTable (VoucherCode,VoucherType,VoucherDate,VoucherCategoryID,VoucherCategory,VoucherCategoryCode,Description,Debit)VALUES
                                                                    (@VoucherCode,@VoucherType,@VoucherDate,@VoucherCategoryID,@VoucherCategory,@VoucherCategoryCode,@Description,@Debit) ";
                    SqlCommand cmd7 = new SqlCommand(query7, connection, transaction);
                    cmd7.Parameters.AddWithValue("@VoucherCode", Convert.ToInt32(saleNumberTextBox.Text.Trim()));
                    cmd7.Parameters.AddWithValue("@VoucherType", saleNumber.Text.Trim());
                    cmd7.Parameters.AddWithValue("@VoucherDate", invoiceDateTimePicker.Value);
                    cmd7.Parameters.AddWithValue("@VoucherCategoryID", customerCodeTextBox.Text.Trim());
                    cmd7.Parameters.AddWithValue("@VoucherCategory", saleCategoryComboBox.SelectedItem);
                    cmd7.Parameters.AddWithValue("@VoucherCategoryCode", voucherCatCode);
                    cmd7.Parameters.AddWithValue("@Description", creditDescription);
                    cmd7.Parameters.AddWithValue("@Debit", totalTextBox.Text.Trim() == string.Empty ? (object)DBNull.Value : Convert.ToDecimal(totalTextBox.Text.Trim()));
                    cmd7.ExecuteNonQuery();

                    SqlCommand cmd8 = new SqlCommand("INSERT INTO [dbo].[TransactionTable] (VoucherCode,VoucherType,VoucherDate,VoucherCategoryID,VoucherCategory,VoucherCategoryCode,Description,Credit)VALUES(@VoucherCode,@VoucherType,@VoucherDate,@VoucherCategoryID,@VoucherCategory,@VoucherCategoryCode,@Description,@Credit)", connection, transaction);
                    cmd8.Parameters.AddWithValue("@VoucherCode", Convert.ToInt32(saleNumberTextBox.Text.Trim()));
                    cmd8.Parameters.AddWithValue("@VoucherType", saleNumber.Text.Trim());
                    cmd8.Parameters.AddWithValue("@VoucherDate", invoiceDateTimePicker.Value);
                    cmd8.Parameters.AddWithValue("@VoucherCategoryID", salesRevenue);
                    cmd8.Parameters.AddWithValue("@VoucherCategory", saleCategoryComboBox.SelectedItem);
                    cmd8.Parameters.AddWithValue("@VoucherCategoryCode", voucherCatCode);
                    cmd8.Parameters.AddWithValue("@Description", salesRevenueDescription);
                    cmd8.Parameters.AddWithValue("@Credit", totalTextBox.Text.Trim() == string.Empty ? (object)DBNull.Value : Convert.ToDecimal(totalTextBox.Text.Trim()));
                    cmd8.ExecuteNonQuery();
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

        private void saleNumberTextBox_Leave(object sender, EventArgs e)
        {
            productDataGridView.DataSource = null;
            productDataGridView.Rows.Clear();
            connection.Open();
            SqlTransaction transaction = null;
            int ID=0;
            try
            {
                
                SqlDataAdapter da4 = new SqlDataAdapter("select distinct max(SaleCode) from Sale", connection);
                DataTable dt4 = new DataTable();
                da4.Fill(dt4);
                int maxNumber = Convert.ToInt32(dt4.Rows[0][0]);
                int invoiceNumber = Convert.ToInt32( saleNumberTextBox.Text.Trim());
                if (maxNumber>=invoiceNumber)
                {
                    saveButton.Enabled = false;
                }

                if (!string.IsNullOrEmpty(saleNumberTextBox.Text.Trim()))
                {
                    
                    transaction = connection.BeginTransaction();
                    string query = "select distinct (SaleCode),Date,CustomerID,SaleCategory from Sale where SaleCode=@SaleCode";
                    SqlCommand cmd = new SqlCommand(query,connection,transaction);
                    cmd.Parameters.AddWithValue("@SaleCode",  saleNumberTextBox.Text.Trim());
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count>0)
                    {
                        invoiceDateTimePicker.Value = (DateTime)dt.Rows[0][1];
                        ID = Convert.ToInt32( dt.Rows[0][2]);
                        saleCategoryComboBox.SelectedItem = dt.Rows[0][3];

                    }
                    else
                    {
                        connection.Close();
                        addNewButton.PerformClick();
                        return;
                        
                    }
                    
                    SqlCommand cmd1 = new SqlCommand("[dbo].[SP_SEARCH_DATA_BASED_ON_ID]", connection,transaction);
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.AddWithValue("@CustomerID",ID);
                    cmd1.Parameters.AddWithValue("@VID",ID);
                    cmd1.Parameters.AddWithValue("@EID",ID);
                    SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                    DataTable dt1 = new DataTable();
                    da1.Fill(dt1);
                    if (dt1.Rows.Count>0)
                    {
                        customerCodeTextBox.Text= dt1.Rows[0][0].ToString();
                        customerNameTextBox.Text = dt1.Rows[0][1].ToString();
                        fatherTextBox.Text = dt1.Rows[0][2].ToString();
                        cnicTextBox.Text = dt1.Rows[0][3].ToString();
                        mobileTextBox.Text = dt1.Rows[0][4].ToString();
                        addressTextBox.Text = dt1.Rows[0][5].ToString();
                    }
                    SqlCommand cmd2 = new SqlCommand("select s.SaleID,s.ProductID as PID,p.ProductDescription as Product,p.Quantity as CurrentStock,s.Qty as SoldStock,s.Price as Rate,SaleAmount as Amount from Sale s join Product p on s.ProductID=p.ProductID where SaleCode=@SaleCode", connection,transaction);
                    cmd2.Parameters.AddWithValue("@SaleCode", saleNumberTextBox.Text.Trim());
                    SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                    DataTable dt2 = new DataTable();
                    da2.Fill(dt2);
                    productDataGridView.Rows.Clear();
                    productDataGridView.AutoGenerateColumns = false;
                    productDataGridView.DataSource = null;
                    productDataGridView.DataSource = dt2;
                    SqlCommand cmd3 = new SqlCommand("SELECT DISTINCT TotalInvoiceAmount,Advance,BalanceAmount,Months,InstallmentAmount FROM Sale where SaleCode=@SaleCode", connection,transaction);
                    cmd3.Parameters.AddWithValue("@SaleCode", saleNumberTextBox.Text.Trim());
                    SqlDataAdapter da3 = new SqlDataAdapter(cmd3);
                    DataTable dt3 = new DataTable();
                    da3.Fill(dt3);
                    if (dt3.Rows.Count>0)
                    {
                        foreach (DataRow row in dt3.Rows)
                        {
                            decimal? TotalInvoiceValue = row.Field<decimal?>("TotalInvoiceAmount");
                            totalTextBox.Text = TotalInvoiceValue.HasValue ? TotalInvoiceValue.Value.ToString() :"";
                            decimal? Advance = row.Field<decimal?>("Advance");
                            advanceTextBox.Text = Advance.HasValue ? Advance.Value.ToString() : "";
                            decimal? Balance = row.Field<decimal?>("BalanceAmount");
                            balanceTextBox.Text = Balance.HasValue ? Balance.Value.ToString() : "";
                            int? Month = row.Field<int?>("Months");
                            monthTextBox.Text = Month.HasValue ? Month.Value.ToString() : "";
                            decimal? Installment = row.Field<decimal?>("InstallmentAmount");
                            installmentTextBox.Text = Installment.HasValue ? Installment.Value.ToString() : "";
                        }
                        //totalTextBox.Text = Convert.ToDecimal(dt3.Rows[0][0]).ToString();
                        //advanceTextBox.Text = Convert.ToDecimal( dt3.Rows[0][1]).ToString();
                        //balanceTextBox.Text = Convert.ToDecimal( dt3.Rows[0][2]).ToString();
                        //installmentTextBox.Text = Convert.ToDecimal( dt3.Rows[0][3]).ToString();
                    }


                }
                transaction.Commit();
                connection.Close();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                transaction.Rollback();
                connection.Close();
            }
            finally
            {
                connection.Close();
            }
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            SqlTransaction transaction = null;
            if (string.IsNullOrWhiteSpace(saleNumberTextBox.Text))
            {
                MessageBox.Show("Sale Invoice Number must not be Empty", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                saleNumberTextBox.Focus();
                return;
            }
            if (saleCategoryComboBox.SelectedIndex == -1)
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
            if (productDataGridView.Rows.Count == 0)
            {
               DialogResult result= MessageBox.Show("Please enter any Product for further Processing", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (result==DialogResult.OK)
                {
                    productDataGridView.DataSource = null;
                }
                productIdTextBox.Focus();
                return;
            }
            if (invoiceDateTimePicker.Value.Date!=DateTime.Now.Date)
            {
                MessageBox.Show("You cannot Update Previous Date", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                addNewButton.PerformClick();
                customerCodeTextBox.Focus();
                return;
            }
            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();
                string voucherCatCode = saleNumber.Text.Trim() + " " + saleNumberTextBox.Text.Trim();
                int VCodeCash = 10002;
                string Description = "Cash";
                string creditDescription = customerNameTextBox.Text.Trim();
                int installmentSaleCode = 10017;
                string installmentSaleDescription = "Installment Sales Revenue";
                int salesRevenue = 10004;
                string salesRevenueDescription = "Sales Revenue";


                foreach (DataGridViewRow row in productDataGridView.Rows)
                {
                    

                    object SaleID = row.Cells["SaleID"].Value==null || row.Cells["SaleID"].Value==DBNull.Value|| string.IsNullOrEmpty( row.Cells["SaleID"].Value.ToString()) ?Convert.ToInt32(0) : Convert.ToInt32(row.Cells["SaleID"].Value) ;
                    int ID = Convert.ToInt32(SaleID);
                    if (ID!=0)
                    {
                        string query1 = $@" UPDATE Sale SET 
                                   SaleCode = @SaleCode,Date = @Date,
                                   CustomerID = @CustomerID,ProductID = @ProductID,
                                   SaleCategory = @SaleCategory,Qty = @Qty,
                                   Price =@Price,SaleAmount = @SaleAmount,
                                   TotalInvoiceAmount = @TotalInvoiceAmount,
                                   Advance = @Advance,BalanceAmount = @BalanceAmount,
                                   Months = @Months,InstallmentAmount = @InstallmentAmount where SaleID=@SaleID";
                                   
                        SqlCommand cmd1 = new SqlCommand(query1, connection, transaction);
                        cmd1.Parameters.AddWithValue("@SaleID",ID);
                        cmd1.Parameters.AddWithValue("@SaleCode", saleNumberTextBox.Text.Trim());
                        cmd1.Parameters.AddWithValue("@Date", invoiceDateTimePicker.Value);
                        cmd1.Parameters.AddWithValue("@CustomerID", Convert.ToInt32(customerCodeTextBox.Text.Trim()));
                        cmd1.Parameters.AddWithValue("@ProductID", Convert.ToInt32(row.Cells["PID"].Value));
                        cmd1.Parameters.AddWithValue("@SaleCategory", saleCategoryComboBox.SelectedItem);
                        cmd1.Parameters.AddWithValue("@Qty", Convert.ToInt32(row.Cells["SaleQty"].Value));
                        cmd1.Parameters.AddWithValue("@Price", Convert.ToDecimal(row.Cells["Rate"].Value));
                        cmd1.Parameters.AddWithValue("@SaleAmount", Convert.ToDecimal(row.Cells["Amount"].Value));
                        cmd1.Parameters.AddWithValue("@TotalInvoiceAmount", Convert.ToDecimal(totalTextBox.Text.Trim()));
                        cmd1.Parameters.AddWithValue("@Advance", advanceTextBox.Text.Trim() == string.Empty ? (object)DBNull.Value : Convert.ToDecimal(advanceTextBox.Text.Trim()));
                        cmd1.Parameters.AddWithValue("@BalanceAmount", balanceTextBox.Text.Trim() == string.Empty ? (object)DBNull.Value : Convert.ToDecimal(balanceTextBox.Text.Trim()));
                        cmd1.Parameters.AddWithValue("@Months", monthTextBox.Text.Trim() == string.Empty ? (object)DBNull.Value : Convert.ToInt32(monthTextBox.Text.Trim()));
                        cmd1.Parameters.AddWithValue("@InstallmentAmount", installmentTextBox.Text.Trim() == string.Empty ? (object)DBNull.Value : Convert.ToDecimal(installmentTextBox.Text.Trim()));
                        cmd1.ExecuteNonQuery(); 
                    }
                    if (ID==0)
                    {
                        string query2 = $@"INSERT INTO Sale (SaleCode,Date,CustomerID,ProductID,SaleCategory,Qty,Price,SaleAmount,TotalInvoiceAmount,Advance,BalanceAmount,Months,InstallmentAmount) VALUES
                                                            (@SaleCode,@Date,@CustomerID,@ProductID,@SaleCategory,@Qty,@Price,@SaleAmount,@TotalInvoiceAmount,@Advance,@BalanceAmount,@Months,@InstallmentAmount)";
                        SqlCommand cmd2 = new SqlCommand(query2,connection,transaction);
                        cmd2.Parameters.AddWithValue("@SaleCode", saleNumberTextBox.Text.Trim());
                        cmd2.Parameters.AddWithValue("@Date", invoiceDateTimePicker.Value);
                        cmd2.Parameters.AddWithValue("@CustomerID", Convert.ToInt32(customerCodeTextBox.Text.Trim()));
                        cmd2.Parameters.AddWithValue("@ProductID", Convert.ToInt32(row.Cells["PID"].Value));
                        cmd2.Parameters.AddWithValue("@SaleCategory", saleCategoryComboBox.SelectedItem);
                        cmd2.Parameters.AddWithValue("@Qty", Convert.ToInt32(row.Cells["SaleQty"].Value));
                        cmd2.Parameters.AddWithValue("@Price", Convert.ToDecimal(row.Cells["Rate"].Value));
                        cmd2.Parameters.AddWithValue("@SaleAmount", Convert.ToDecimal(row.Cells["Amount"].Value));
                        cmd2.Parameters.AddWithValue("@TotalInvoiceAmount", Convert.ToDecimal(totalTextBox.Text.Trim()));
                        cmd2.Parameters.AddWithValue("@Advance", advanceTextBox.Text.Trim() == string.Empty ? (object)DBNull.Value : Convert.ToDecimal(advanceTextBox.Text.Trim()));
                        cmd2.Parameters.AddWithValue("@BalanceAmount", balanceTextBox.Text.Trim() == string.Empty ? (object)DBNull.Value : Convert.ToDecimal(balanceTextBox.Text.Trim()));
                        cmd2.Parameters.AddWithValue("@Months", monthTextBox.Text.Trim() == string.Empty ? (object)DBNull.Value : Convert.ToInt32(monthTextBox.Text.Trim()));
                        cmd2.Parameters.AddWithValue("@InstallmentAmount", installmentTextBox.Text.Trim() == string.Empty ? (object)DBNull.Value : Convert.ToDecimal(installmentTextBox.Text.Trim()));
                        cmd2.ExecuteNonQuery();
                    }
                }

                foreach (DataGridViewRow row in productDataGridView.Rows)
                {
                    object SaleID = row.Cells["SaleID"].Value == null || row.Cells["SaleID"].Value == DBNull.Value || string.IsNullOrEmpty(row.Cells["SaleID"].Value.ToString()) ? Convert.ToInt32(0) : Convert.ToInt32(row.Cells["SaleID"].Value);
                    int ID = Convert.ToInt32(SaleID);

                    if (ID==0)
                    {
                        string query3 = $@"INSERT INTO [dbo].[Stock] (Date,SaleID,ProductID,CustomerID,SoldQuantity)VALUES
                                                                     (@Date,@SaleID,@ProductID,@CustomerID,@SoldQuantity)";
                        SqlCommand cmd3 = new SqlCommand(query3, connection, transaction);
                        cmd3.Parameters.AddWithValue("@Date", invoiceDateTimePicker.Value);
                        cmd3.Parameters.AddWithValue("@SaleID", Convert.ToInt32(saleNumberTextBox.Text.Trim()));
                        cmd3.Parameters.AddWithValue("@ProductID",Convert.ToInt32( row.Cells["PID"].Value));
                        cmd3.Parameters.AddWithValue("@CustomerID", Convert.ToInt32(customerCodeTextBox.Text.Trim()));
                        cmd3.Parameters.AddWithValue("@SoldQuantity", row.Cells["SaleQty"].Value);
                        cmd3.ExecuteNonQuery(); 
                    }
                    if (ID!=0)
                    {
                        string query4 = $@"UPDATE Stock SET Date=@Date,SaleID=@SaleID,ProductID=@ProductID,
                                                            CustomerID=@CustomerID,SoldQuantity=@SoldQuantity WHERE SaleID=@SaleID and ProductID=@ProductID";
                        SqlCommand cmd4 = new SqlCommand(query4, connection, transaction);
                        cmd4.Parameters.AddWithValue("@Date", invoiceDateTimePicker.Value);
                        cmd4.Parameters.AddWithValue("@SaleID", Convert.ToInt32(saleNumberTextBox.Text.Trim()));
                        cmd4.Parameters.AddWithValue("@ProductID",Convert.ToInt32( row.Cells["PID"].Value));
                        cmd4.Parameters.AddWithValue("@CustomerID", Convert.ToInt32(customerCodeTextBox.Text.Trim()));
                        cmd4.Parameters.AddWithValue("@SoldQuantity", row.Cells["SaleQty"].Value);
                        cmd4.ExecuteNonQuery();
                    }
                }
                if (saleCategoryComboBox.SelectedIndex == 2)
                {
                    string query5 = $@"UPDATE TransactionTable SET VoucherCode=@VoucherCode,
                                                                   VoucherType=@VoucherType,
                                                                   VoucherDate=@VoucherDate,
                                                                   Description=@Description,
                                                                   VoucherCategoryID=@VoucherCategoryID,
                                                                   VoucherCategory=@VoucherCategory,
                                                                   VoucherCategoryCode=@VoucherCategoryCode,
                                                                   Debit=@Debit WHERE VoucherCode=@VoucherCode AND VoucherType=@VoucherType AND VoucherCategoryID=@VoucherCategoryID ";
                    SqlCommand cmd5 = new SqlCommand(query5, connection, transaction);
                    cmd5.Parameters.AddWithValue("@VoucherCode", Convert.ToInt32(saleNumberTextBox.Text.Trim()));
                    cmd5.Parameters.AddWithValue("@VoucherType", saleNumber.Text.Trim());
                    cmd5.Parameters.AddWithValue("@VoucherDate", invoiceDateTimePicker.Value);
                    cmd5.Parameters.AddWithValue("@Description", Description);
                    cmd5.Parameters.AddWithValue("@VoucherCategoryID", VCodeCash);
                    cmd5.Parameters.AddWithValue("@VoucherCategory", saleCategoryComboBox.SelectedItem);
                    cmd5.Parameters.AddWithValue("@VoucherCategoryCode", voucherCatCode);
                    cmd5.Parameters.AddWithValue("@Debit", advanceTextBox.Text.Trim() == string.Empty ? (object)DBNull.Value : Convert.ToDecimal(advanceTextBox.Text.Trim()));
                    cmd5.ExecuteNonQuery();

                    string query6 = $@"UPDATE TransactionTable SET VoucherCode=@VoucherCode,VoucherType=@VoucherType,
                                                                   VoucherDate=@VoucherDate,Description=@Description,
                                                                   VoucherCategoryID=@VoucherCategoryID,VoucherCategory=@VoucherCategory,
                                                                   VoucherCategoryCode=@VoucherCategoryCode,Debit=@Debit WHERE VoucherCode=@VoucherCode AND VoucherType=@VoucherType AND VoucherCategoryID=@VoucherCategoryID ";
                    SqlCommand cmd6 = new SqlCommand(query6, connection, transaction);
                    cmd6.Parameters.AddWithValue("@VoucherCode", Convert.ToInt32(saleNumberTextBox.Text.Trim()));
                    cmd6.Parameters.AddWithValue("@VoucherType", saleNumber.Text.Trim());
                    cmd6.Parameters.AddWithValue("@VoucherDate", invoiceDateTimePicker.Value);
                    cmd6.Parameters.AddWithValue("@VoucherCategoryID", Convert.ToInt32(customerCodeTextBox.Text.Trim()));
                    cmd6.Parameters.AddWithValue("@VoucherCategory", saleCategoryComboBox.SelectedItem);
                    cmd6.Parameters.AddWithValue("@VoucherCategoryCode", voucherCatCode);
                    cmd6.Parameters.AddWithValue("@Description", customerNameTextBox.Text.Trim());
                    cmd6.Parameters.AddWithValue("@Debit", balanceTextBox.Text.Trim() == string.Empty ? (object)DBNull.Value : Convert.ToDecimal(balanceTextBox.Text.Trim()));
                    cmd6.ExecuteNonQuery();

                    string query7 = $@"UPDATE TransactionTable SET VoucherCode=@VoucherCode,VoucherType=@VoucherType,
                                                                   VoucherDate=@VoucherDate,Description=@Description,
                                                                   VoucherCategoryID=@VoucherCategoryID,VoucherCategory=@VoucherCategory,
                                                                   VoucherCategoryCode=@VoucherCategoryCode,Credit=@Credit WHERE VoucherCode=@VoucherCode AND VoucherType=@VoucherType AND VoucherCategoryID=@VoucherCategoryID ";
                    SqlCommand cmd7 = new SqlCommand(query7, connection, transaction);
                    cmd7.Parameters.AddWithValue("@VoucherCode", Convert.ToInt32(saleNumberTextBox.Text.Trim()));
                    cmd7.Parameters.AddWithValue("@VoucherType", saleNumber.Text.Trim());
                    cmd7.Parameters.AddWithValue("@VoucherDate", invoiceDateTimePicker.Value);
                    cmd7.Parameters.AddWithValue("@VoucherCategoryID", installmentSaleCode);
                    cmd7.Parameters.AddWithValue("@VoucherCategory", saleCategoryComboBox.SelectedItem);
                    cmd7.Parameters.AddWithValue("@VoucherCategoryCode", voucherCatCode);
                    cmd7.Parameters.AddWithValue("@Description", installmentSaleDescription);
                    cmd7.Parameters.AddWithValue("@Credit", totalTextBox.Text.Trim() == string.Empty ? (object)DBNull.Value : Convert.ToDecimal(totalTextBox.Text.Trim()));
                    cmd7.ExecuteNonQuery();


                }
                if (saleCategoryComboBox.SelectedIndex == 0)
                {
                    string query8= $@"UPDATE TransactionTable SET VoucherCode=@VoucherCode,VoucherType=@VoucherType,
                                                                   VoucherDate=@VoucherDate,Description=@Description,
                                                                   VoucherCategoryID=@VoucherCategoryID,VoucherCategory=@VoucherCategory,
                                                                   VoucherCategoryCode=@VoucherCategoryCode,Debit=@Debit WHERE VoucherCode=@VoucherCode AND VoucherType=@VoucherType AND VoucherCategoryID=@VoucherCategoryID ";
                    SqlCommand cmd8 = new SqlCommand(query8, connection, transaction);
                    cmd8.Parameters.AddWithValue("@VoucherCode", Convert.ToInt32(saleNumberTextBox.Text.Trim()));
                    cmd8.Parameters.AddWithValue("@VoucherType", saleNumber.Text.Trim());
                    cmd8.Parameters.AddWithValue("@VoucherDate", invoiceDateTimePicker.Value);
                    cmd8.Parameters.AddWithValue("@VoucherCategoryID", VCodeCash);
                    cmd8.Parameters.AddWithValue("@VoucherCategory", saleCategoryComboBox.SelectedItem);
                    cmd8.Parameters.AddWithValue("@VoucherCategoryCode", voucherCatCode);
                    cmd8.Parameters.AddWithValue("@Description", Description);
                    cmd8.Parameters.AddWithValue("@Debit", totalTextBox.Text.Trim() == string.Empty ? (object)DBNull.Value : Convert.ToDecimal(totalTextBox.Text.Trim()));
                    cmd8.ExecuteNonQuery();

                    string query9 = $@"UPDATE TransactionTable SET VoucherCode=@VoucherCode,VoucherType=@VoucherType,
                                                                   VoucherDate=@VoucherDate,Description=@Description,
                                                                   VoucherCategoryID=@VoucherCategoryID,VoucherCategory=@VoucherCategory,
                                                                   VoucherCategoryCode=@VoucherCategoryCode,Credit=@Credit WHERE VoucherCode=@VoucherCode AND VoucherType=@VoucherType AND VoucherCategoryID=@VoucherCategoryID ";
                    SqlCommand cmd9 = new SqlCommand(query9, connection, transaction);
                    cmd9.Parameters.AddWithValue("@VoucherCode", Convert.ToInt32(saleNumberTextBox.Text.Trim()));
                    cmd9.Parameters.AddWithValue("@VoucherType", saleNumber.Text.Trim());
                    cmd9.Parameters.AddWithValue("@VoucherDate", invoiceDateTimePicker.Value);
                    cmd9.Parameters.AddWithValue("@VoucherCategoryID", salesRevenue);
                    cmd9.Parameters.AddWithValue("@VoucherCategory", saleCategoryComboBox.SelectedItem);
                    cmd9.Parameters.AddWithValue("@VoucherCategoryCode", voucherCatCode);
                    cmd9.Parameters.AddWithValue("@Description", salesRevenueDescription);
                    cmd9.Parameters.AddWithValue("@Credit", totalTextBox.Text.Trim() == string.Empty ? (object)DBNull.Value : Convert.ToDecimal(totalTextBox.Text.Trim()));
                    cmd9.ExecuteNonQuery();
                }
                if (saleCategoryComboBox.SelectedIndex == 1)
                {
                    string query10 = $@"UPDATE TransactionTable SET VoucherCode=@VoucherCode,VoucherType=@VoucherType,
                                                                   VoucherDate=@VoucherDate,Description=@Description,
                                                                   VoucherCategoryID=@VoucherCategoryID,VoucherCategory=@VoucherCategory,
                                                                   VoucherCategoryCode=@VoucherCategoryCode,Debit=@Debit WHERE VoucherCode=@VoucherCode AND VoucherType=@VoucherType AND VoucherCategoryID=@VoucherCategoryID ";
                    SqlCommand cmd10 = new SqlCommand(query10, connection, transaction);
                    cmd10.Parameters.AddWithValue("@VoucherCode", Convert.ToInt32(saleNumberTextBox.Text.Trim()));
                    cmd10.Parameters.AddWithValue("@VoucherType", saleNumber.Text.Trim());
                    cmd10.Parameters.AddWithValue("@VoucherDate", invoiceDateTimePicker.Value);
                    cmd10.Parameters.AddWithValue("@VoucherCategoryID", customerCodeTextBox.Text.Trim());
                    cmd10.Parameters.AddWithValue("@VoucherCategory", saleCategoryComboBox.SelectedItem);
                    cmd10.Parameters.AddWithValue("@VoucherCategoryCode", voucherCatCode);
                    cmd10.Parameters.AddWithValue("@Description", creditDescription);
                    cmd10.Parameters.AddWithValue("@Debit", totalTextBox.Text.Trim() == string.Empty ? (object)DBNull.Value : Convert.ToDecimal(totalTextBox.Text.Trim()));
                    cmd10.ExecuteNonQuery();

                    string query11 = $@"UPDATE TransactionTable SET VoucherCode=@VoucherCode,VoucherType=@VoucherType,
                                                                   VoucherDate=@VoucherDate,Description=@Description,
                                                                   VoucherCategoryID=@VoucherCategoryID,VoucherCategory=@VoucherCategory,
                                                                   VoucherCategoryCode=@VoucherCategoryCode,Credit=@Credit WHERE VoucherCode=@VoucherCode AND VoucherType=@VoucherType AND VoucherCategoryID=@VoucherCategoryID ";
                    SqlCommand cmd11 = new SqlCommand(query11, connection, transaction);
                    cmd11.Parameters.AddWithValue("@VoucherCode", Convert.ToInt32(saleNumberTextBox.Text.Trim()));
                    cmd11.Parameters.AddWithValue("@VoucherType", saleNumber.Text.Trim());
                    cmd11.Parameters.AddWithValue("@VoucherDate", invoiceDateTimePicker.Value);
                    cmd11.Parameters.AddWithValue("@VoucherCategoryID", salesRevenue);
                    cmd11.Parameters.AddWithValue("@VoucherCategory", saleCategoryComboBox.SelectedItem);
                    cmd11.Parameters.AddWithValue("@VoucherCategoryCode", voucherCatCode);
                    cmd11.Parameters.AddWithValue("@Description", salesRevenueDescription);
                    cmd11.Parameters.AddWithValue("@Credit", totalTextBox.Text.Trim() == string.Empty ? (object)DBNull.Value : Convert.ToDecimal(totalTextBox.Text.Trim()));
                    cmd11.ExecuteNonQuery();
                }


                transaction.Commit();
                MessageBox.Show("The Transaction is successfull", "Sucess", MessageBoxButtons.OK, MessageBoxIcon.Information);
                connection.Close();
                
                this.ControlBox = true;
                saveButton.Enabled = true;
                addNewButton.Enabled = true;
                closeButton.Enabled = true;
                updateButton.Enabled = false;
                addNewButton.PerformClick();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                transaction.Rollback();
                connection.Close();
            }
            finally
            {
                connection.Close();
            }


        
    }
        private int LastIncrementedNumber()
        {
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter("select distinct max(SaleCode) from Sale", connection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            int maxNumber = Convert.ToInt32(dt.Rows[0][0]);
            int newNumber = maxNumber + 1;
            return newNumber;
        }

        private void previousButton_Click(object sender, EventArgs e)
        {
            int voucherNumber = Convert.ToInt32(saleNumberTextBox.Text.Trim());
            int newNumber = voucherNumber - 1;
            saleNumberTextBox.Text = newNumber.ToString();
            saleNumberTextBox_Leave(sender, e);
            saveButton.Enabled = false;
            updateButton.Enabled = true;
        }

        private void nextButton_Click(object sender, EventArgs e)
        {
            int voucherNumber = Convert.ToInt32(saleNumberTextBox.Text.Trim());
            int newNumber = voucherNumber + 1;
            saleNumberTextBox.Text = newNumber.ToString();
            saleNumberTextBox_Leave(sender, e);
            saveButton.Enabled = false;
            updateButton.Enabled = true;
        }
    }
}
