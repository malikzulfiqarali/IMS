using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IMS.Properties;
using System.Data.SqlClient;
using System.Configuration;

namespace IMS
{
    public partial class CashReceiptVoucher : Form
    {
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString);



        public CashReceiptVoucher()
        {
            InitializeComponent();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            foreach (TextBox textBox in this.Controls.OfType<TextBox>())
            {

                {
                    textBox.Clear();
                }

            }
            foreach (ComboBox comboBox in this.Controls.OfType<ComboBox>())
            {
                comboBox.SelectedIndex = -1;
            }
            crvDataGridView.Rows.Clear();
            MaxNumberVoucherCode();
            cashCodeTextBox.Text = "10002";
            saveButton.Enabled = true;
            updateButton.Enabled = true;
            creditTotalTextBox.Text = string.Empty;
            updateButton.Enabled = false;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            SqlTransaction sqlTransaction = null;
            if (crvTextBox.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Cash Receipt Voucher Field is require", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                crvTextBox.Focus();
                return;
            }
            if (categoryComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Please select item from Category Field", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                categoryComboBox.Focus();
                return;

            }
            if (narrationTextBox.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Narration field is required", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                narrationTextBox.Focus();
                return;

            }
            if (cashCodeTextBox.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Cash Field is required. You must enter cash Code which is 10002", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cashCodeTextBox.Focus();
                return;
            }
            if (crvDataGridView.Rows.Count == 0)
            {
                MessageBox.Show("Cannot save empty voucher", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                crvDataGridView.Focus();
                return;
            }

            try
            {
                    connection.Open();

                sqlTransaction = connection.BeginTransaction();
                

                    foreach (DataGridViewRow row in crvDataGridView.Rows)
                    {

                        if (row.IsNewRow)
                            continue;
                        SqlCommand cmd = new SqlCommand("SP_INSERT_VOUCHER", connection,sqlTransaction);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@VoucherCode", crvTextBox.Text);
                        cmd.Parameters.AddWithValue("@VoucherType", crvLabel.Text.Trim());
                        cmd.Parameters.AddWithValue("@VoucherDate", dateDateTimePicker.Value);
                        cmd.Parameters.AddWithValue("@Narration", narrationTextBox.Text.Trim());
                        cmd.Parameters.AddWithValue("@VoucherCategoryID", row.Cells["Code"].Value);
                        cmd.Parameters.AddWithValue("@VoucherCategory", categoryComboBox.SelectedItem.ToString().Trim());
                        cmd.Parameters.AddWithValue("@VoucherCategoryCode", row.Cells["PartyCode"].Value ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Description", row.Cells["Description"].Value ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Credit", Convert.ToDecimal(row.Cells["Amount"].Value));
                        cmd.Parameters.AddWithValue("@Remarks", row.Cells["Remarks"].Value ?? DBNull.Value);
                        cmd.ExecuteNonQuery();

                    }
                   

                    
                        decimal Total = getCreditTotal();

                if (Total!=0 && crvDataGridView.Rows.Count!=0)
                {
                    SqlCommand command = new SqlCommand("[SP_INSERT_VOUCHER_DEBIT]", connection, sqlTransaction);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@VoucherCode", crvTextBox.Text);
                    command.Parameters.AddWithValue("@VoucherType", crvLabel.Text.Trim());
                    command.Parameters.AddWithValue("@VoucherDate", dateDateTimePicker.Value);
                    command.Parameters.AddWithValue("@Narration", narrationTextBox.Text.Trim());
                    command.Parameters.AddWithValue("@VoucherCategoryID", cashCodeTextBox.Text.Trim());
                    command.Parameters.AddWithValue("@VoucherCategory", categoryComboBox.SelectedItem.ToString().Trim());
                    command.Parameters.AddWithValue("@Description", cashLabel.Text.Trim());
                    command.Parameters.AddWithValue("@Debit", Total);
                    command.ExecuteNonQuery();

                    sqlTransaction.Commit();
                    MessageBox.Show("Rocord saved to database successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clearButton.PerformClick();
                    MaxNumberVoucherCode();
                    connection.Close();
                }
                else
                {
                    MessageBox.Show("Empty voucher cannot be saved","Information",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    clearButton.PerformClick();
                    connection.Close();
                    
                }
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                sqlTransaction.Rollback();
            }
            finally
            {
                connection.Close();
            }
        }


        private void crvDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            FetchDataForm fetchDataForm = new FetchDataForm();
            fetchDataForm.ShowDialog();
        }
        private void crvDataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {


            //crvDataGridView.CurrentRow.Cells["Code"].Value = FetchDataForm.SetCode;
            //crvDataGridView.CurrentRow.Cells["Description"].Value = FetchDataForm.SetName;
            //creditTotalTextBox.Text = getCreditTotal().ToString();




        }
        private decimal getDebitTotal()
        {
            decimal Total = 0;
            foreach (DataGridViewRow row in crvDataGridView.Rows)
            {
                Total += Convert.ToInt32(row.Cells["Debit"].Value);
            }
            return Total;
        }
        private decimal getCreditTotal()
        {
            decimal Total = 0;
            foreach (DataGridViewRow row in crvDataGridView.Rows)
            {
                Total += Convert.ToDecimal(row.Cells["Amount"].Value);
            }
            return Total;
        }
        private void MaxNumberVoucherCode()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter("SELECT DISTINCT MAX(VoucherCode) FROM TransactionTable WHERE VoucherType='" + crvLabel.Text.Trim() + "'", connection))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        int maxNumber = Convert.ToInt32(dt.Rows[0][0]);
                        int incrementNumber = maxNumber + 1;
                        crvTextBox.Text = incrementNumber.ToString().Trim();

                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void CashReceiptVoucher_Load(object sender, EventArgs e)
        {
            MaxNumberVoucherCode();
            updateButton.Enabled = false;
            saveButton.Enabled = true;
        }

        private void crvDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            SqlTransaction sqlTransaction = null;

            connection.Open();
            try
            {

                if (crvDataGridView.Columns[e.ColumnIndex].Name == "RemoveColumnButton")
                {
                    if (dateDateTimePicker.Value.Date == DateTime.Now.Date)
                    {
                        sqlTransaction = connection.BeginTransaction();
                        int id = Convert.ToInt32(crvDataGridView.Rows[e.RowIndex].Cells["ID"].Value);
                        string query = "DELETE from TransactionTable WHERE VoucherID=@VoucherID ";
                        SqlCommand cmd = new SqlCommand(query, connection, sqlTransaction);
                        cmd.Parameters.AddWithValue("@VoucherID", id);
                        cmd.ExecuteNonQuery();

                        crvDataGridView.Rows.RemoveAt(e.RowIndex);

                        decimal Total = getCreditTotal();
                        string query1 = "UPDATE TransactionTable SET Narration=@Narration,VoucherCategory=@VoucherCategory,Description=@Description,Debit=@Debit where VoucherType=@VoucherType and VoucherCode=@VoucherCode and VoucherCategoryID=@VoucherCategoryID";
                        SqlCommand cmd1 = new SqlCommand(query1, connection, sqlTransaction);
                        cmd1.Parameters.AddWithValue("@Narration", narrationTextBox.Text.Trim());
                        cmd1.Parameters.AddWithValue("@VoucherCategory", categoryComboBox.SelectedItem);
                        cmd1.Parameters.AddWithValue("@Description", cashLabel.Text.Trim());
                        cmd1.Parameters.AddWithValue("@Debit", Total);
                        cmd1.Parameters.AddWithValue("@VoucherType", crvLabel.Text.Trim());
                        cmd1.Parameters.AddWithValue("@VoucherCode", crvTextBox.Text.Trim());
                        cmd1.Parameters.AddWithValue("@VoucherCategoryID", cashCodeTextBox.Text.Trim());
                        cmd1.ExecuteNonQuery();
                        sqlTransaction.Commit();
                    }
                    else
                    {
                        MessageBox.Show("You cannot remove previous date data", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Please remove this field from remove button" + ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                sqlTransaction.Rollback();
                connection.Close();

            }
            finally
            {
                connection.Close();
            }


        }

        private void crvDataGridView_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (e.KeyChar==(char)Keys.Enter)
            //{
            //    FetchDataForm fetchDataForm = new FetchDataForm();
            //    fetchDataForm.ShowDialog();
            //}
        }

        private void crvTextBox_TextChanged(object sender, EventArgs e)
        {
            saveButton.Enabled = false;
            updateButton.Enabled = false;
        }

        private void crvTextBox_Leave(object sender, EventArgs e)
        {
            try
            {
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM TransactionTable WHERE VoucherCategoryID <> '" + 10002 + "' and VoucherType='" + crvLabel.Text.Trim() + "'and VoucherCode='" + crvTextBox.Text.Trim() + "'", connection);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    dateDateTimePicker.Value = (DateTime)dt.Rows[0]["VoucherDate"];
                    narrationTextBox.Text = dt.Rows[0]["Narration"].ToString();
                    categoryComboBox.SelectedItem = dt.Rows[0]["VoucherCategory"];
                    crvDataGridView.Rows.Clear();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        crvDataGridView.Rows.Add();
                        crvDataGridView.Rows[i].Cells["Code"].Value = dt.Rows[i]["VoucherCategoryID"];
                        crvDataGridView.Rows[i].Cells["Description"].Value = dt.Rows[i]["Description"];
                        crvDataGridView.Rows[i].Cells["PartyCode"].Value = dt.Rows[i]["VoucherCategoryCode"];
                        crvDataGridView.Rows[i].Cells["Amount"].Value = dt.Rows[i]["Credit"];
                        crvDataGridView.Rows[i].Cells["Remarks"].Value = dt.Rows[i]["Remarks"];
                        crvDataGridView.Rows[i].Cells["ID"].Value = dt.Rows[i]["VoucherID"];

                    }

                    creditTotalTextBox.Text = getCreditTotal().ToString();
                }
                else
                {
                    //MessageBox.Show("Requested Record Does not Exist","Message",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    clearButton.PerformClick();
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void nextButton_Click(object sender, EventArgs e)
        {
            int voucherNumber = Convert.ToInt32(crvTextBox.Text.Trim());
            int newNumber = voucherNumber + 1;
            crvTextBox.Text = newNumber.ToString();
            crvTextBox_Leave(sender, e);
            saveButton.Enabled = false;
            if (dateDateTimePicker.Value.Date == System.DateTime.Today.Date)
            {
                updateButton.Enabled = true;
            }
            else
            {
                updateButton.Enabled = false;
            }

        }

        private void previousButton_Click(object sender, EventArgs e)
        {
            int voucherNumber = Convert.ToInt32(crvTextBox.Text.Trim());
            int newNumber = voucherNumber - 1;
            crvTextBox.Text = newNumber.ToString();
            crvTextBox_Leave(sender, e);
            saveButton.Enabled = false;
            if (dateDateTimePicker.Value.Date == System.DateTime.Today.Date)
            {
                updateButton.Enabled = true;
            }
            else
            {
                updateButton.Enabled = false;

            }
        }

        private void crvDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                FetchDataForm fetchDataForm = new FetchDataForm();
                fetchDataForm.ShowDialog();
                e.Handled = true;

            }
        }

        private void crvDataGridView_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (crvDataGridView.CurrentRow.Cells["Code"].Value==null || Convert.ToInt32(crvDataGridView.CurrentRow.Cells["Code"].Value.ToString().Trim())==0)
            {
                crvDataGridView.CurrentRow.Cells["Code"].Value = FetchDataForm.SetCode;
                crvDataGridView.CurrentRow.Cells["Description"].Value = FetchDataForm.SetName;
                creditTotalTextBox.Text = getCreditTotal().ToString();

            }
        }
        private void updateButton_Click(object sender, EventArgs e)
        {
            bool success = false;
            if (crvTextBox.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Cash Receipt Voucher Field is require", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                crvTextBox.Focus();
                return;
            }
            if (categoryComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Please select item from Category Field", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                categoryComboBox.Focus();
                return;

            }
            if (narrationTextBox.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Narration field is required", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                narrationTextBox.Focus();
                return;

            }
            if (cashCodeTextBox.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Cash Field is required. You must enter cash Code which is 10002", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cashCodeTextBox.Focus();
                return;
            }

            int cashCode = Convert.ToInt32(cashCodeTextBox.Text.Trim());
            connection.Open();

            SqlTransaction transaction = null;
            decimal Total = getCreditTotal();
            try
            {
                transaction = connection.BeginTransaction();
                foreach (DataGridViewRow row in crvDataGridView.Rows)
                {

                    if (!row.IsNewRow)
                    {
                        int ID = Convert.ToInt32(row.Cells["ID"].Value);


                        if (ID != 0)
                        {
                            string narration = narrationTextBox.Text.Trim();
                            int VCode = Convert.ToInt32(row.Cells["Code"].Value);
                            string VCat = categoryComboBox.SelectedItem.ToString();
                            string VCatCode = row.Cells["PartyCode"].Value.ToString();
                            string Description = row.Cells["Description"].Value.ToString();
                            decimal amount = Convert.ToDecimal(row.Cells["Amount"].Value);
                            string Remarks = row.Cells["Remarks"].Value.ToString();

                            string query = "UPDATE TransactionTable SET  Narration=@Narration,VoucherCategoryID=@VoucherCategoryID,VoucherCategory=@VoucherCategory,VoucherCategoryCode=@VoucherCategoryCode,Description=@Description,Credit=@Credit,Remarks=@Remarks where VoucherID=@VoucherID and VoucherType=@VoucherType and VoucherCode=@VoucherCode and VoucherCategoryID!='" + cashCode + "'";
                            SqlCommand cmd = new SqlCommand(query, connection, transaction);
                            cmd.Parameters.AddWithValue("@VoucherID", ID);
                            cmd.Parameters.AddWithValue("@Narration", narration);
                            cmd.Parameters.AddWithValue("@VoucherDate", dateDateTimePicker.Value);
                            cmd.Parameters.AddWithValue("@VoucherCategoryID", VCode);
                            cmd.Parameters.AddWithValue("@VoucherCategory", VCat);
                            cmd.Parameters.AddWithValue("@VoucherCategoryCode", VCatCode);
                            cmd.Parameters.AddWithValue("@Description", Description);
                            cmd.Parameters.AddWithValue("@Credit", amount);
                            cmd.Parameters.AddWithValue("@Remarks", Remarks);
                            cmd.Parameters.AddWithValue("@VoucherType", crvLabel.Text.Trim());
                            cmd.Parameters.AddWithValue("@VoucherCode", crvTextBox.Text.Trim());
                            cmd.ExecuteNonQuery();
                        }


                        if (ID == 0)
                        {


                            string query2 = "insert into TransactionTable (Narration,VoucherDate,VoucherCategoryID,VoucherCategory,VoucherCategoryCode,Description,Credit,Remarks,VoucherType,VoucherCode)values(@Narration,@VoucherDate,@VoucherCategoryID,@VoucherCategory,@VoucherCategoryCode,@Description,@Credit,@Remarks,@VoucherType,@VoucherCode)";
                            SqlCommand cmd2 = new SqlCommand(query2, connection, transaction);
                            cmd2.Parameters.AddWithValue("@Narration", narrationTextBox.Text.Trim());
                            cmd2.Parameters.AddWithValue("@VoucherDate", dateDateTimePicker.Value);
                            cmd2.Parameters.AddWithValue("@VoucherCategoryID", Convert.ToInt32(row.Cells["Code"].Value));
                            cmd2.Parameters.AddWithValue("@VoucherCategory", categoryComboBox.SelectedItem.ToString());
                            cmd2.Parameters.AddWithValue("@VoucherCategoryCode", row.Cells["PartyCode"].Value ?? DBNull.Value);
                            cmd2.Parameters.AddWithValue("@Description", row.Cells["Description"].Value ?? DBNull.Value);
                            cmd2.Parameters.AddWithValue("@Credit", Convert.ToDecimal(row.Cells["Amount"].Value));
                            cmd2.Parameters.AddWithValue("@Remarks", row.Cells["Remarks"].Value ?? DBNull.Value);
                            cmd2.Parameters.AddWithValue("@VoucherType", crvLabel.Text.Trim());
                            cmd2.Parameters.AddWithValue("@VoucherCode", crvTextBox.Text.Trim());
                            cmd2.ExecuteNonQuery();


                        }

                    }
                }
                string query1 = "UPDATE TransactionTable SET Narration=@Narration,VoucherCategory=@VoucherCategory,Description=@Description,Debit=@Debit where VoucherType=@VoucherType and VoucherCode=@VoucherCode and VoucherCategoryID=@VoucherCategoryID";
                SqlCommand cmd1 = new SqlCommand(query1, connection, transaction);
                cmd1.Parameters.AddWithValue("@Narration", narrationTextBox.Text.Trim());
                cmd1.Parameters.AddWithValue("@VoucherCategory", categoryComboBox.SelectedItem);
                cmd1.Parameters.AddWithValue("@Description", cashLabel.Text.Trim());
                cmd1.Parameters.AddWithValue("@Debit", Total);
                cmd1.Parameters.AddWithValue("@VoucherType", crvLabel.Text.Trim());
                cmd1.Parameters.AddWithValue("@VoucherCode", crvTextBox.Text.Trim());
                cmd1.Parameters.AddWithValue("@VoucherCategoryID", cashCodeTextBox.Text.Trim());

                cmd1.ExecuteNonQuery();
                transaction.Commit();
                MessageBox.Show("Records are updated successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                connection.Close();
                clearButton.PerformClick();




            }

            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                transaction.Rollback();
            }
            finally
            {
                connection.Close();
            }
        }

        private void crvDataGridView_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.RowIndex > -1)
            //{
            //    DataGridViewCell currentCell = crvDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];

            //    // Restore the value of the previous cell
            //    currentCell.Value = previousValue;
            //}

        }

        private void crvDataGridView_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
            FetchDataForm.SetCode = 0;
            FetchDataForm.SetName = string.Empty;
        }

        private void crvDataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (crvDataGridView.CurrentCell.ColumnIndex == crvDataGridView.Columns["Amount"].Index)
            {
                TextBox textBox = e.Control as TextBox;

                if (textBox != null)
                {
                    // Attach an event handler to the TextBox control
                    textBox.KeyPress += NumericOnlyKeyPress;
                }
            }
        }

        private void NumericOnlyKeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void crvDataGridView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            //if (!string.IsNullOrEmpty(e.FormattedValue?.ToString()))
            //{
            //    // Cancel the event to prevent the default action
            //    e.Cancel = true;
            //        crvDataGridView.CellValidating += crvDataGridView_CellValidating;


            //}
        }
    }
}
    

