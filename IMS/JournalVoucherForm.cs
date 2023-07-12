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
    public partial class JournalVoucherForm : Form
    {
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString);
        public JournalVoucherForm()
        {
            InitializeComponent();
        }

        private void JournalVoucherForm_Load(object sender, EventArgs e)
        {
            ClearAllStuff();
            MaxNumberVoucherCode();
            
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            bool success = false;
            if (jrvTextBox.Text.Trim()==string.Empty)
            {
                MessageBox.Show("This field is required","failure",MessageBoxButtons.OK,MessageBoxIcon.Information);
                jrvTextBox.Focus();
                return;
            }
            if (categoryComboBox.SelectedIndex==-1)
            {
                MessageBox.Show("Please select an item from category combobox", "failure", MessageBoxButtons.OK, MessageBoxIcon.Information);
                categoryComboBox.Focus();
                return;
            }
            if (narrationTextBox.Text.Trim()==string.Empty)
            {
                MessageBox.Show("Narration field is required", "failure", MessageBoxButtons.OK, MessageBoxIcon.Information);
                narrationTextBox.Focus();
                return;
            }
            if (jrvDataGridView.Rows.Count==-1)
            {
                MessageBox.Show("Please enters correct particulars in voucher detail", "failure", MessageBoxButtons.OK, MessageBoxIcon.Information);
                jrvDataGridView.Focus();
                return;
            }
            try
            {
                foreach (DataGridViewRow row in jrvDataGridView.Rows)
                {
                    if (row.IsNewRow)
                        continue;
                    SqlCommand cmd = new SqlCommand("[SP_INSERT_JOURNAL_VOUCHER]", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@VoucherCode", jrvTextBox.Text.Trim());
                    cmd.Parameters.AddWithValue("@VoucherType", jrvLabel.Text.Trim());
                    cmd.Parameters.AddWithValue("@VoucherDate", dateDateTimePicker.Value);
                    cmd.Parameters.AddWithValue("@Narration", narrationTextBox.Text.Trim());
                    cmd.Parameters.AddWithValue("@VoucherCategoryID",row.Cells["Code"].Value??DBNull.Value);
                    cmd.Parameters.AddWithValue("@VoucherCategory",categoryComboBox.SelectedItem);
                    cmd.Parameters.AddWithValue("@VoucherCategoryCode",row.Cells["PartyCode"].Value??DBNull.Value);
                    cmd.Parameters.AddWithValue("@Description",row.Cells["Description"].Value??DBNull.Value);
                    cmd.Parameters.AddWithValue("@Debit",row.Cells["Debit"].Value??DBNull.Value);
                    cmd.Parameters.AddWithValue("@Credit",row.Cells["Credit"].Value??DBNull.Value);
                    cmd.Parameters.AddWithValue("@Remarks", row.Cells["Remarks"].Value??DBNull.Value);
                    if (debitTextBox.Text.Trim()==creditTextBox.Text.Trim())
                    {
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
                    else
                    {
                        MessageBox.Show("Debit Total must be equal to Credit Total","Failure",MessageBoxButtons.OK,MessageBoxIcon.Information);
                        return;
                    }
                   
                }
                if (success==true)
                {
                    MessageBox.Show("Records are inserted successfully","Success",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    clearButton.PerformClick();
                    
                }
                else
                {
                    MessageBox.Show("Records are not inserted something happen wrong", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void jrvDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            FetchDataForm fetchDataForm = new FetchDataForm();
            fetchDataForm.ShowDialog();
        }

        private void jrvDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                FetchDataForm fetchDataForm = new FetchDataForm();
                fetchDataForm.ShowDialog();
                e.Handled = true;

            }
        }

        private void jrvDataGridView_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            jrvDataGridView.CurrentRow.Cells["Code"].Value = FetchDataForm.SetCode;
            jrvDataGridView.CurrentRow.Cells["Description"].Value = FetchDataForm.SetName;
            debitTextBox.Text = getDebitTotal().ToString();
            creditTextBox.Text = getCreditTotal().ToString();
        }

        private void jrvDataGridView_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
            FetchDataForm.SetCode = 0;
            FetchDataForm.SetName = string.Empty;
        }
        private void MaxNumberVoucherCode()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter("SELECT MAX(VoucherCode) FROM TransactionTable WHERE VoucherType='" + jrvLabel.Text.Trim() + "'", connection))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        int maxNumber = Convert.ToInt32(dt.Rows[0][0]);
                        int incrementNumber = maxNumber + 1;
                        jrvTextBox.Text = incrementNumber.ToString().Trim();

                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            ClearAllStuff();
            MaxNumberVoucherCode();
        }
        private void ClearAllStuff()
        {
            foreach (TextBox textBox in this.Controls.OfType<TextBox>())
            {
                textBox.Clear();
            }
            foreach (ComboBox comboBox in this.Controls.OfType<ComboBox>())
            {
                comboBox.SelectedIndex = -1;
            }
            jrvDataGridView.Rows.Clear();
        }

        private void jrvDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (jrvDataGridView.Columns[e.ColumnIndex].Name == "RemoveColumnButton")
                {
                    jrvDataGridView.Rows.RemoveAt(e.RowIndex);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Please remove this field from remove button" + ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void jrvDataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (jrvDataGridView.CurrentCell.ColumnIndex == jrvDataGridView.Columns["Debit"].Index || jrvDataGridView.CurrentCell.ColumnIndex==jrvDataGridView.Columns["Credit"].Index)
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
        private decimal getCreditTotal()
        {
            decimal Total = 0;
            foreach (DataGridViewRow row in jrvDataGridView.Rows)
            {
                Total += Convert.ToDecimal(row.Cells["Credit"].Value);
            }
            return Total;
        }
        private decimal getDebitTotal()
        {
            decimal Total = 0;
            foreach (DataGridViewRow row in jrvDataGridView.Rows)
            {
                Total += Convert.ToDecimal(row.Cells["Debit"].Value);
            }
            return Total;
        }

        private void jrvTextBox_Leave(object sender, EventArgs e)
        {
            try
            {
                connection.Open();
                SqlDataAdapter da = new SqlDataAdapter("Select * from TransactionTable where VoucherCode='" + jrvTextBox.Text.Trim() + "' and VoucherType='"+jrvLabel.Text.Trim()+"'", connection);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    dateDateTimePicker.Value = (DateTime)dt.Rows[0]["VoucherDate"];
                    narrationTextBox.Text = dt.Rows[0]["Narration"].ToString();
                    categoryComboBox.SelectedItem = dt.Rows[0]["VoucherCategory"];
                    jrvDataGridView.Rows.Clear();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        
                        jrvDataGridView.Rows.Add();
                        jrvDataGridView.Rows[i].Cells["Code"].Value = dt.Rows[i]["VoucherCategoryID"];
                        jrvDataGridView.Rows[i].Cells["Description"].Value = dt.Rows[i]["Description"];
                        jrvDataGridView.Rows[i].Cells["PartyCode"].Value = dt.Rows[i]["VoucherCategoryCode"];
                        jrvDataGridView.Rows[i].Cells["Debit"].Value = dt.Rows[i]["Debit"]==DBNull.Value? "0": dt.Rows[i]["Debit"];
                        jrvDataGridView.Rows[i].Cells["Credit"].Value = dt.Rows[i]["Credit"]==DBNull.Value? "0": dt.Rows[i]["Credit"];
                        jrvDataGridView.Rows[i].Cells["Remarks"].Value = dt.Rows[i]["Remarks"];

                    }

                    creditTextBox.Text= getCreditTotal().ToString();
                    debitTextBox.Text = getDebitTotal().ToString();
                }
                //else
                //{
                //    MessageBox.Show("Data not found","Failure",MessageBoxButtons.OK,MessageBoxIcon.Information);
                //}

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

        private void previousButton_Click(object sender, EventArgs e)
        {
            int voucherNumber = Convert.ToInt32(jrvTextBox.Text.Trim());
            int newNumber = voucherNumber - 1;
            jrvTextBox.Text = newNumber.ToString();
            jrvTextBox_Leave(sender, e);
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

        private void nextButton_Click(object sender, EventArgs e)
        {
            int voucherNumber = Convert.ToInt32(jrvTextBox.Text.Trim());
            int newNumber = voucherNumber + 1;
            jrvTextBox.Text = newNumber.ToString();
            jrvTextBox_Leave(sender, e);
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
    }
}
