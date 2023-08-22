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
        string stockPurchaseCode = "10013";
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
                string query = $@"select distinct max(PurchaseCode) from [dbo].[Purchase]";
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
                connection.Close();
            }
        }

        private void Purchase_Load(object sender, EventArgs e)
        {
            ClearAllStuff();
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
            narrationTextBox.Text = "Stock Received";
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
                decimal PurchasedQty =string.IsNullOrEmpty(saleQtyTextBox.Text)?Convert.ToDecimal( 0): Convert.ToDecimal(saleQtyTextBox.Text);
                decimal Price = string.IsNullOrEmpty(priceTextBox.Text)?Convert.ToDecimal(0) :Convert.ToDecimal(priceTextBox.Text);
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
            connection.Open();
            string Number = purchaseCodeTextBox.Text.Trim();
            string query1 = "select COUNT(*) from Purchase where PurchaseCode=@PurchaseCode";
            SqlCommand cmd1 = new SqlCommand(query1, connection);
            cmd1.Parameters.AddWithValue("@PurchaseCode", Number);
            int count = (int)cmd1.ExecuteScalar();
            connection.Close();

            if (count > 0)
            {
                DataTable dataTable = (DataTable)purchaseDataGridView.DataSource;
                DataRow newRow = dataTable.NewRow();
                newRow["ProductID"] = productIdTextBox.Text.Trim();
                newRow["Product"] = productTextBox.Text.Trim();
                newRow["Quantity"] = currentStockTextBox.Text.Trim();
                newRow["PurchasedQty"] = saleQtyTextBox.Text.Trim();
                newRow["Price"] = priceTextBox.Text.Trim();
                newRow["Amount"] = totalAmountTextBox.Text.Trim();
                dataTable.Rows.Add(newRow);
                grandTotalTextBox.Text = GetGrandTotal().ToString();
                return;
            }
            else
            {
                purchaseDataGridView.DataSource = null;

                purchaseDataGridView.Rows.Add(productIdTextBox.Text.Trim(), productTextBox.Text.Trim(), currentStockTextBox.Text.Trim(), saleQtyTextBox.Text.Trim(), priceTextBox.Text.Trim(), totalAmountTextBox.Text.Trim());
                // UpdateStockInProductTable();
                productIdTextBox.Text = string.Empty;
                productTextBox.Text = string.Empty;
                currentStockTextBox.Text = string.Empty;
                saleQtyTextBox.Text = string.Empty;
                priceTextBox.Text = string.Empty;
                totalAmountTextBox.Text = string.Empty;
                grandTotalTextBox.Text = GetGrandTotal().ToString();
            }

            
            
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

        private void saveButton_Click(object sender, EventArgs e)
        {
            
            string description = "Stock Purchases";
            string category = "Purchases";
            string voucherCatCode = purchaseLabel.Text+" "+ purchaseCodeTextBox.Text;

            SqlTransaction sqlTransaction = null;
            if (string.IsNullOrEmpty(purchaseCodeTextBox.Text))
            {
                MessageBox.Show("This filed is required","Information",MessageBoxButtons.OK,MessageBoxIcon.Information);
                purchaseCodeTextBox.Focus();
                return;
            }
            if (string.IsNullOrEmpty(companyCodeTextBox.Text))
            {
                MessageBox.Show("Please select company", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                companyCodeTextBox.Focus();
                return;
            }
            if (string.IsNullOrEmpty(narrationTextBox.Text))
            {
                MessageBox.Show("Please give narration", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                narrationTextBox.Focus();
                return;
            }
            if (purchaseDataGridView.Rows.Count==0)
            {
                MessageBox.Show("Please give some detail or add products for further processing", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                purchaseDataGridView.Focus();
                return;
            }
            try
            {
                connection.Open();
                sqlTransaction = connection.BeginTransaction();
                string query1 = $@"INSERT INTO Purchase (PurchaseCode,Date,CompanyID,ProductID,PurchaseQty,PurchasePrice,PurchaseAmount)
                                                        VALUES
                                                        (@PurchaseCode,@Date,@CompanyID,@ProductID,@PurchaseQty,@PurchasePrice,@PurchaseAmount)";
                foreach (DataGridViewRow row in purchaseDataGridView.Rows)
                {
                    SqlCommand cmd1 = new SqlCommand(query1, connection, sqlTransaction);
                    cmd1.Parameters.AddWithValue("@PurchaseCode", purchaseCodeTextBox.Text.Trim());
                    cmd1.Parameters.AddWithValue("@Date", purchaseDateTimePicker.Value);
                    cmd1.Parameters.AddWithValue("@CompanyID", companyCodeTextBox.Text.Trim()); 
                    cmd1.Parameters.AddWithValue("@ProductID", row.Cells["ProductID"].Value); 
                    cmd1.Parameters.AddWithValue("@PurchaseQty", row.Cells["PurchaseQty"].Value); 
                    cmd1.Parameters.AddWithValue("@PurchasePrice", row.Cells["Price"].Value); 
                    cmd1.Parameters.AddWithValue("@PurchaseAmount", row.Cells["TotalAmount"].Value);
                    cmd1.ExecuteNonQuery();
                    
                }

                string query2 = $@"INSERT INTO Stock (PurchaseID,Date,ProductID,CompanyID,PurchasedQuantity)
                                                      VALUES
                                                    (@PurchaseID,@Date,@ProductID,@CompanyID,@PurchasedQuantity)";
                foreach (DataGridViewRow row in purchaseDataGridView.Rows)
                {
                    SqlCommand cmd2 = new SqlCommand(query2,connection,sqlTransaction);
                    cmd2.Parameters.AddWithValue("@PurchaseID", purchaseCodeTextBox.Text.Trim());
                    cmd2.Parameters.AddWithValue("@Date", purchaseDateTimePicker.Value);
                    cmd2.Parameters.AddWithValue("@ProductID", row.Cells["ProductID"].Value);
                    cmd2.Parameters.AddWithValue("@CompanyID", companyCodeTextBox.Text.Trim());
                    cmd2.Parameters.AddWithValue("@PurchasedQuantity", row.Cells["PurchaseQty"].Value);
                    cmd2.ExecuteNonQuery();
                }

                string query3 = $@"INSERT INTO TransactionTable (VoucherCode,VoucherType,VoucherDate,Narration,VoucherCategoryID,VoucherCategory,VoucherCategoryCode,Description,Debit)
                                                                VALUES
                                                                (@VoucherCode,@VoucherType,@VoucherDate,@Narration,@VoucherCategoryID,@VoucherCategory,@VoucherCategoryCode,@Description,@Debit)";
                SqlCommand cmd3 = new SqlCommand(query3,connection,sqlTransaction);
                cmd3.Parameters.AddWithValue("@VoucherCode", purchaseCodeTextBox.Text.Trim());
                cmd3.Parameters.AddWithValue("@VoucherType", purchaseLabel.Text.Trim());
                cmd3.Parameters.AddWithValue("@VoucherDate", purchaseDateTimePicker.Value);
                cmd3.Parameters.AddWithValue("@Narration", narrationTextBox.Text.Trim());
                cmd3.Parameters.AddWithValue("@VoucherCategoryID", stockPurchaseCode);
                cmd3.Parameters.AddWithValue("@VoucherCategory", category);
                cmd3.Parameters.AddWithValue("@VoucherCategoryCode", voucherCatCode);
                cmd3.Parameters.AddWithValue("@Description", description);
                cmd3.Parameters.AddWithValue("@Debit", grandTotalTextBox.Text);
                cmd3.ExecuteNonQuery();

                string query4 = $@"INSERT INTO TransactionTable (VoucherCode,VoucherType,VoucherDate,Narration,VoucherCategoryID,VoucherCategory,VoucherCategoryCode,Description,Credit)
                                                                VALUES
                                                                (@VoucherCode,@VoucherType,@VoucherDate,@Narration,@VoucherCategoryID,@VoucherCategory,@VoucherCategoryCode,@Description,@Credit)";
                SqlCommand cmd4 = new SqlCommand(query4, connection, sqlTransaction);
                cmd4.Parameters.AddWithValue("@VoucherCode", purchaseCodeTextBox.Text.Trim());
                cmd4.Parameters.AddWithValue("@VoucherType", purchaseLabel.Text.Trim());
                cmd4.Parameters.AddWithValue("@VoucherDate", purchaseDateTimePicker.Value);
                cmd4.Parameters.AddWithValue("@Narration", narrationTextBox.Text.Trim());
                cmd4.Parameters.AddWithValue("@VoucherCategoryID", companyCodeTextBox.Text);
                cmd4.Parameters.AddWithValue("@VoucherCategory", category);
                cmd4.Parameters.AddWithValue("@VoucherCategoryCode", voucherCatCode);
                cmd4.Parameters.AddWithValue("@Description", companyNameLabel.Text);
                cmd4.Parameters.AddWithValue("@Credit", grandTotalTextBox.Text);
                cmd4.ExecuteNonQuery();

                // for update Stock in Product Table on save Button



                foreach (DataGridViewRow row in purchaseDataGridView.Rows)
                {
                    int currentQty =Convert.ToInt32( row.Cells["CurrentStock"].Value);
                    int purchasedQty = Convert.ToInt32(row.Cells["PurchaseQty"].Value);
                    int updatedQty = currentQty + purchasedQty;
                    string query5 = $@"UPDATE Product SET Quantity=@Quantity where ProductID=@ProductID ";
                    SqlCommand cmd5 = new SqlCommand(query5, connection, sqlTransaction);
                    cmd5.Parameters.AddWithValue("@Quantity", updatedQty); 
                    cmd5.Parameters.AddWithValue("@ProductID", row.Cells["ProductID"].Value);
                    cmd5.ExecuteNonQuery();
                }


                sqlTransaction.Commit();

                MessageBox.Show("Records are saved successfully","Success",MessageBoxButtons.OK,MessageBoxIcon.Information);
                connection.Close();
                addNewButton.PerformClick();
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                sqlTransaction.Rollback();
                connection.Close();
            }
            finally
            {
                connection.Close();
            }
        }
        private void ClearAllStuff()
        {
            foreach (TextBox textBox in this.Controls.OfType<TextBox>())
            {
                textBox.Clear();
            }
            purchaseDataGridView.DataSource = null;
            purchaseDataGridView.Refresh();
            purchaseDataGridView.Rows.Clear();
        }

        private void addNewButton_Click(object sender, EventArgs e)
        {
            ClearAllStuff();
            GetMaxNumber();
            saveButton.Enabled = true;
            updateButton.Enabled = false;
            addButton.Enabled = false;
            purchaseDateTimePicker.Value = DateTime.Now;
        }

        private void purchaseCodeTextBox_Leave(object sender, EventArgs e)
        {
            try
            {
                connection.Open();
                string query1 = $@"select pr.ProductID as ProductID,pr.ProductDescription as Product, pr.Quantity as Quantity,p.PurchaseQty as PurchasedQty,p.PurchasePrice as Price,p.PurchaseAmount as Amount,p.PurchaseID as PurchaseID  from Purchase p 
                                join Product pr on p.ProductID=pr.ProductID
                                where p.PurchaseCode=@PCode";
                SqlCommand cmd1 = new SqlCommand(query1,connection);
                cmd1.Parameters.AddWithValue("@PCode", purchaseCodeTextBox.Text.Trim());
                SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                DataTable dt1 = new DataTable();
                da1.Fill(dt1);
                purchaseDataGridView.DataSource = null;
                purchaseDataGridView.AutoGenerateColumns = false;
                purchaseDataGridView.DataSource = dt1;
                connection.Close();

                connection.Open();
                string query2 = $@"select v.VID,v.Company,t.VoucherDate,t.Narration,t.Credit from VendorDetail v 
                                    join TransactionTable t on v.VID=t.VoucherCategoryID
                                    where t.VoucherCode=@Vcode";
                SqlCommand cmd2 = new SqlCommand(query2,connection);
                cmd2.Parameters.AddWithValue("@Vcode",purchaseCodeTextBox.Text);
                SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                DataTable dt2 = new DataTable();
                da2.Fill(dt2);
                if (dt2.Rows.Count>0)
                {
                    companyCodeTextBox.Text = dt2.Rows[0]["VID"].ToString();
                    companyNameLabel.Text = dt2.Rows[0]["Company"].ToString();
                    purchaseDateTimePicker.Value = (DateTime)dt2.Rows[0]["VoucherDate"];
                    narrationTextBox.Text = dt2.Rows[0]["Narration"].ToString();
                    grandTotalTextBox.Text = GetGrandTotal().ToString(); 
                }
                connection.Close();


                connection.Open();
                string Number = purchaseCodeTextBox.Text.Trim();
                string query3 = "select COUNT(*) from Purchase where PurchaseCode=@PurchaseCode";
                SqlCommand cmd3 = new SqlCommand(query3, connection);
                cmd3.Parameters.AddWithValue("@PurchaseCode", Number);
                int count = (int)cmd3.ExecuteScalar();
                
                if (count > 0)
                {
                    saveButton.Enabled = false;
                    updateButton.Enabled = true;
                }

                connection.Close();
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

        private void previousButton_Click(object sender, EventArgs e)
        {
            int voucherNumber = Convert.ToInt32(purchaseCodeTextBox.Text.Trim());
            int newNumber = voucherNumber - 1;
            purchaseCodeTextBox.Text = newNumber.ToString();
            purchaseCodeTextBox_Leave(sender, e);
            saveButton.Enabled = false;
            updateButton.Enabled = true;
        }

        private void nextButton_Click(object sender, EventArgs e)
        {
            int voucherNumber = Convert.ToInt32(purchaseCodeTextBox.Text.Trim());
            int newNumber = voucherNumber + 1;
            purchaseCodeTextBox.Text = newNumber.ToString();
            purchaseCodeTextBox_Leave(sender, e);
            saveButton.Enabled = false;
            updateButton.Enabled = true;
        }
        
        private void purchaseDataGridView_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //if (e.RowIndex >= 0)
            //{
            //   int rowIndex = e.RowIndex;
               
            //}
        }

        private void purchaseDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (purchaseDateTimePicker.Value.Date!=DateTime.Now.Date)
            {
                MessageBox.Show("You cannot delete previous date record","Information",MessageBoxButtons.OK,MessageBoxIcon.Information);
                return;
            }
            if (e.KeyCode==Keys.Delete)
            {
                purchaseDataGridView.RefreshEdit();
                purchaseDataGridView.RowsRemoved += PurchaseDataGridView_RowsRemoved;
                




                UpdateBackQuantityInProductTable();
                object PurchaseID = purchaseDataGridView.CurrentRow.Cells["PurchaseID"].Value == null || purchaseDataGridView.CurrentRow.Cells["PurchaseID"].Value == DBNull.Value || string.IsNullOrEmpty(purchaseDataGridView.CurrentRow.Cells["PurchaseID"].Value.ToString()) ? Convert.ToInt32(0) : Convert.ToInt32(purchaseDataGridView.CurrentRow.Cells["PurchaseID"].Value);
                int ID = Convert.ToInt32(PurchaseID);
                connection.Open();
                int id1 = Convert.ToInt32(purchaseDataGridView.CurrentRow.Cells["PurchaseID"].Value);
                string deleteQuery1 = $@"delete from Purchase where PurchaseID=@PurchaseID";
                SqlCommand cmd1 = new SqlCommand(deleteQuery1,connection);
                cmd1.Parameters.AddWithValue("@PurchaseID",id1);

                if (ID!=0)
                {
                    cmd1.ExecuteNonQuery();
                    
                }

                connection.Close();
                
                
               


                connection.Open();
                string deleteQuery2 = $@"delete from Stock where ProductID=@ProductID and PurchaseID=@PurchaseID";
                SqlCommand cmd2 = new SqlCommand(deleteQuery2,connection);
                int id2 = Convert.ToInt32(purchaseDataGridView.CurrentRow.Cells["ProductID"].Value);
                cmd2.Parameters.AddWithValue("@ProductID",id2);
                cmd2.Parameters.AddWithValue("@PurchaseID",Convert.ToInt32( purchaseCodeTextBox.Text));
                if (ID!=0)
                {
                    cmd2.ExecuteNonQuery();
                }
                connection.Close();

                purchaseDataGridView.RefreshEdit();
               



            }
            
        }
        private void UpdateBackQuantityInProductTable()
        {
            connection.Open();
            try
            {

                string query1 = "SELECT ProductID,Quantity FROM Product WHERE ProductID=@ProductID";
                string query2 = "UPDATE Product SET Quantity=@Quantity WHERE ProductID=@ProductID";


                SqlCommand command = new SqlCommand(query1, connection);
                command.Parameters.AddWithValue("@ProductID", Convert.ToInt32(purchaseDataGridView.CurrentRow.Cells["ProductID"].Value));
                SqlDataAdapter da = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                da.Fill(dt);


                int id = Convert.ToInt32(dt.Rows[0][0]);
                int qty = Convert.ToInt32(dt.Rows[0][1]);
                int decreasedQty = qty - Convert.ToInt32(purchaseDataGridView.CurrentRow.Cells["PurchaseQty"].Value);

                SqlCommand cmd = new SqlCommand(query2, connection);
                cmd.Parameters.AddWithValue("@Quantity", decreasedQty);
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
        private void UpdateTransactionTableOnDeletion()
        {
            try
            {


                
                connection.Open();
                string updateQuery1 = $@"update TransactionTable set Debit=@Debit where VoucherCode=@VCode and VoucherType=@VType and VoucherCategoryID=@VCID";
                SqlCommand cmd1 = new SqlCommand(updateQuery1,connection);
                cmd1.Parameters.AddWithValue("@VCode", purchaseCodeTextBox.Text);
                cmd1.Parameters.AddWithValue("@VType", purchaseLabel.Text);
                cmd1.Parameters.AddWithValue("@Debit", GetGrandTotal().ToString());
                cmd1.Parameters.AddWithValue("@VCID", stockPurchaseCode);
                cmd1.ExecuteNonQuery();
                connection.Close();

                connection.Open();
                string updateQuery2 = $@"update TransactionTable set Credit=@Credit where VoucherCode=@VCode and VoucherType=@VType and VoucherCategoryID=@VCID ";
                SqlCommand cmd2 = new SqlCommand(updateQuery2, connection);
                cmd2.Parameters.AddWithValue("@VCode", purchaseCodeTextBox.Text);
                cmd2.Parameters.AddWithValue("@VType", purchaseLabel.Text);
                cmd2.Parameters.AddWithValue("@Credit", GetGrandTotal().ToString());
                cmd2.Parameters.AddWithValue("@VCID", companyCodeTextBox.Text);
                cmd2.ExecuteNonQuery();
                connection.Close();


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

        private void PurchaseDataGridView_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            purchaseDataGridView.Refresh();
            GetGrandTotal();
            grandTotalTextBox.Text = GetGrandTotal().ToString();
            UpdateTransactionTableOnDeletion();
        }

        private void purchaseDataGridView_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
           // grandTotalTextBox.Text = GetGrandTotal().ToString();
        }
    }

}
