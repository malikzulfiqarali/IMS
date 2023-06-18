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
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
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
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
                {
                    connection.Open();



                    foreach (DataGridViewRow row in crvDataGridView.Rows)
                    {

                        if (row.IsNewRow)
                            continue;
                        SqlCommand cmd = new SqlCommand("SP_INSERT_VOUCHER", connection);
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
                        int result = cmd.ExecuteNonQuery();

                    }


                    string creditTotalValue = creditTotalTextBox.Text ?? null;
                    string column1 = crvDataGridView.Rows[0].Cells["Code"].Value.ToString() ?? null;
                    string column2 = crvDataGridView.Rows[0].Cells["Description"].Value.ToString() ?? null;
                    string column3 = crvDataGridView.Rows[0].Cells["Amount"].Value.ToString() ?? null;


                    if (string.IsNullOrEmpty(creditTotalValue) && string.IsNullOrEmpty(column1) && string.IsNullOrEmpty(column2) && string.IsNullOrEmpty(column3))
                    //if(crvDataGridView==null)
                    {
                        MessageBox.Show("Please enter records in DataGridView", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        SqlCommand command = new SqlCommand("[SP_INSERT_VOUCHER_DEBIT]", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@VoucherCode", crvTextBox.Text);
                        command.Parameters.AddWithValue("@VoucherType", crvLabel.Text.Trim());
                        command.Parameters.AddWithValue("@VoucherDate", dateDateTimePicker.Value);
                        command.Parameters.AddWithValue("@Narration", narrationTextBox.Text.Trim());
                        command.Parameters.AddWithValue("@VoucherCategoryID", cashCodeTextBox.Text.Trim());
                        command.Parameters.AddWithValue("@VoucherCategory", categoryComboBox.SelectedItem.ToString().Trim());
                        command.Parameters.AddWithValue("@Description", cashLabel.Text.Trim());
                        command.Parameters.AddWithValue("@Debit", Convert.ToDecimal(creditTotalTextBox.Text));
                        int success = command.ExecuteNonQuery();



                        if (success > 0)
                        {
                            MessageBox.Show("Rocord saved to database successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            clearButton.PerformClick();
                            MaxNumberVoucherCode();
                            connection.Close();
                        }
                        else
                        {
                            MessageBox.Show("Rocord is not saved to database successfully", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            connection.Close();

                        }

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
        private int getDebitTotal()
        {
            int Total = 0;
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
        }

        private void crvDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (crvDataGridView.Columns[e.ColumnIndex].Name == "RemoveColumnButton")
                {
                    crvDataGridView.Rows.RemoveAt(e.RowIndex);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Please remove this field from remove button" + ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            crvDataGridView.CurrentRow.Cells["Code"].Value = FetchDataForm.SetCode;
            crvDataGridView.CurrentRow.Cells["Description"].Value = FetchDataForm.SetName;
            creditTotalTextBox.Text = getCreditTotal().ToString();

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
            try
            {
                foreach (DataGridViewRow row in crvDataGridView.Rows)
                {

                    if (row.IsNewRow) continue;
                    SqlCommand cmd = new SqlCommand("SP_UPDATE_VOUCHER", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@VoucherID", Convert.ToInt32(row.Cells["ID"].Value));
                    cmd.Parameters.AddWithValue("@VoucherCategoryID", row.Cells["Code"].Value);
                    cmd.Parameters.AddWithValue("@Description", row.Cells["Description"].Value ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@VoucherCategoryCode", row.Cells["PartyCode"].Value ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Credit", row.Cells["Amount"].Value ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Remarks", row.Cells["Remarks"].Value ?? DBNull.Value);
                    connection.Open();
                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        success = true;
                        connection.Close();
                    }
                    else
                    {
                        success = false;
                        connection.Close();
                    }
                }
                if (success == true)
                {

                    SqlCommand command = new SqlCommand("SP_UPDATE_VOUCHER_DEBIT", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@VoucherCode", crvTextBox.Text.Trim());
                    command.Parameters.AddWithValue("@VoucherType", crvLabel.Text.Trim());
                    command.Parameters.AddWithValue("@Narration", narrationTextBox.Text.Trim());
                    command.Parameters.AddWithValue("@VoucherCategoryID", Convert.ToInt32(cashCodeTextBox.Text.Trim()));
                    command.Parameters.AddWithValue("@Description", cashLabel.Text.Trim());
                    command.Parameters.AddWithValue("@VoucherCategory", categoryComboBox.SelectedItem);
                    command.Parameters.AddWithValue("@Debit", creditTotalTextBox.Text.Trim());
                    connection.Open();
                    int resultDebit = command.ExecuteNonQuery();
                    if (resultDebit > 0)
                    {
                        success = true;
                    }
                    else
                    {
                        success = false;
                    }
                }
                if (success == true)
                {
                    MessageBox.Show("Records are updated successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Records are not updated successfully", "failure", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
        //private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
        //    {
        //        DataGridViewRow row = crvDataGridView.Rows[e.RowIndex];
        //        DataGridViewCell cell = crvDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];
        //        row.Cells[e.ColumnIndex].Selected = true;
        //        row.Selected = true;
        //    }
        //}
        private object previousValue;

        private void dataGridView1_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                DataGridViewCell currentCell = crvDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];

                // Store the value of the current cell
                previousValue = currentCell.Value;
            }
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                DataGridViewCell currentCell = crvDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];

                // Restore the value of the previous cell
                currentCell.Value = previousValue;
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
    

