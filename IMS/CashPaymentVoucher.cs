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
    public partial class CashPaymentVoucher : Form
    {
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString);
        public CashPaymentVoucher()
        {
            InitializeComponent();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private decimal getCreditTotal()
        {
            decimal Total = 0;
            foreach (DataGridViewRow row in cpvDataGridView.Rows)
            {
                Total += Convert.ToDecimal(row.Cells["Amount"].Value);
            }
            return Total;
        }


        private void saveButton_Click(object sender, EventArgs e)
        {
            bool success = false;
            if (cpvTextBox.Text.Trim()==string.Empty)
            {
                MessageBox.Show("This field is required","Failure",MessageBoxButtons.OK,MessageBoxIcon.Information);
                cpvTextBox.Focus();
                return;
            }
            if (categoryComboBox.SelectedIndex==-1)
            {
                MessageBox.Show("Please select an item from combobox","Failure",MessageBoxButtons.OK,MessageBoxIcon.Information);
                categoryComboBox.Focus();
                return;
            }
            if (narrationTextBox.Text.Trim()==string.Empty)
            {
                MessageBox.Show("Narration is required", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Information);
                narrationTextBox.Focus();
                return;
            }
            if (cpvDataGridView.Rows.Count==0)
            {
                MessageBox.Show("Cannot save an empty Voucher","Failure", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cpvDataGridView.Focus();
                return;
            }
            try
            {
                
                using (SqlConnection connection=new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
                {
                    
                        foreach (DataGridViewRow row in cpvDataGridView.Rows)
                        {
                            if (row.IsNewRow)
                            continue;

                        SqlCommand cmd = new SqlCommand("[SP_INSERT_CASH_PAYMENT_VOUCHER]", connection);

                        cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@VoucherCode",cpvTextBox.Text.Trim());
                            cmd.Parameters.AddWithValue("@VoucherType", cpvLabel.Text.Trim());
                            cmd.Parameters.AddWithValue("@VoucherDate", dateDateTimePicker.Value);
                            cmd.Parameters.AddWithValue("@Narration",narrationTextBox.Text.Trim());
                            cmd.Parameters.AddWithValue("@VoucherCategoryID",row.Cells["Code"].Value);
                            cmd.Parameters.AddWithValue("@VoucherCategory",categoryComboBox.SelectedItem);
                            cmd.Parameters.AddWithValue("@VoucherCategoryCode",row.Cells["PartyCode"].Value??DBNull.Value);
                            cmd.Parameters.AddWithValue("@Description",row.Cells["Description"].Value??DBNull.Value);
                            cmd.Parameters.AddWithValue("@Debit",row.Cells["Amount"].Value);
                            cmd.Parameters.AddWithValue("@Remarks",row.Cells["Remarks"].Value??DBNull.Value);
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
                                
                            }
                            

                        }

                    
                    if (success==true)
                    {
                        using (SqlCommand cmd=new SqlCommand("SP_INSERT_CASH_PAYMENT_VOUCHER_CREDIT",connection))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@VoucherCode",cpvTextBox.Text.Trim());
                            cmd.Parameters.AddWithValue("@VoucherType", cpvLabel.Text.Trim());
                            cmd.Parameters.AddWithValue("@VoucherDate", dateDateTimePicker.Value);
                            cmd.Parameters.AddWithValue("@Narration", narrationTextBox.Text.Trim());
                            cmd.Parameters.AddWithValue("@VoucherCategoryID", cashCodeTextBox.Text.Trim());
                            cmd.Parameters.AddWithValue("@VoucherCategory", categoryComboBox.SelectedItem);
                            cmd.Parameters.AddWithValue("@Description",cashLabel.Text.Trim());
                            cmd.Parameters.AddWithValue("@Credit", creditTotalTextBox.Text.Trim());
                            connection.Open();
                            int result = cmd.ExecuteNonQuery();
                            if (result > 0)
                            {
                                success = true;
                            }
                            else
                            {
                                success = false;
                            }
                        }
                    }
                
                }
                if (success == true)
                {
                    MessageBox.Show("Records are inserted successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearAllStuff();
                    MaxNumberVoucherCode();
                    cashCodeTextBox.Text = "10002";
                }
                else
                {
                    MessageBox.Show("Records are not inserted successfully Please Provide Complete Particulars Something is missing", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void CashPaymentVoucher_Load(object sender, EventArgs e)
        {
            MaxNumberVoucherCode();
            creditTotalTextBox.Text = getCreditTotal().ToString();
        }

        private void cpvDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            FetchDataForm fetchDataForm = new FetchDataForm();
            fetchDataForm.ShowDialog();
        }

        private void cpvDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                FetchDataForm fetchDataForm = new FetchDataForm();
                fetchDataForm.ShowDialog();
                e.Handled = true;

            }
        }

        private void cpvDataGridView_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            cpvDataGridView.CurrentRow.Cells["Code"].Value = FetchDataForm.SetCode;
            cpvDataGridView.CurrentRow.Cells["Description"].Value = FetchDataForm.SetName;
            creditTotalTextBox.Text = getCreditTotal().ToString();
        }

        private void cpvDataGridView_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
            FetchDataForm.SetCode = 0;
            FetchDataForm.SetName = string.Empty;
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            ClearAllStuff();
            MaxNumberVoucherCode();
            cashCodeTextBox.Text = "10002";
        }
        private void MaxNumberVoucherCode()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter("SELECT MAX(VoucherCode) FROM TransactionTable WHERE VoucherType='" + cpvLabel.Text.Trim() + "'", connection))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        int maxNumber = Convert.ToInt32(dt.Rows[0][0]);
                        int incrementNumber = maxNumber + 1;
                        cpvTextBox.Text = incrementNumber.ToString().Trim();

                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
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
            cpvDataGridView.Rows.Clear();
        }

        private void CashPaymentVoucher_Activated(object sender, EventArgs e)
        {
           
        }

        private void cpvDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void cpvDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (cpvDataGridView.Columns[e.ColumnIndex].Name == "RemoveColumnButton")
                {
                    cpvDataGridView.Rows.RemoveAt(e.RowIndex);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Please remove this field from remove button" + ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void cpvTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void cpvTextBox_Leave(object sender, EventArgs e)
        {
            try
            {
                connection.Open();
                SqlDataAdapter da = new SqlDataAdapter("Select * from TransactionTable where VoucherCode='"+cpvTextBox.Text.Trim()+ "' and VoucherType='"+cpvLabel.Text.Trim()+ "' and VoucherCategoryID!='"+cashCodeTextBox.Text.Trim()+"'", connection);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    dateDateTimePicker.Value = (DateTime)dt.Rows[0]["VoucherDate"];
                    narrationTextBox.Text = dt.Rows[0]["Narration"].ToString();
                    categoryComboBox.SelectedItem = dt.Rows[0]["VoucherCategory"];
                    cpvDataGridView.Rows.Clear();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        cpvDataGridView.Rows.Add();
                        cpvDataGridView.Rows[i].Cells["Code"].Value = dt.Rows[i]["VoucherCategoryID"];
                        cpvDataGridView.Rows[i].Cells["Description"].Value = dt.Rows[i]["Description"];
                        cpvDataGridView.Rows[i].Cells["PartyCode"].Value = dt.Rows[i]["VoucherCategoryCode"];
                        cpvDataGridView.Rows[i].Cells["Amount"].Value = dt.Rows[i]["Debit"];
                        cpvDataGridView.Rows[i].Cells["Remarks"].Value = dt.Rows[i]["Remarks"];

                    }

                    creditTotalTextBox.Text = getCreditTotal().ToString();
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
        }

        private void cpvDataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (cpvDataGridView.CurrentCell.ColumnIndex == cpvDataGridView.Columns["Amount"].Index)
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
            // Allow only numerical values, backspace, and decimal separator (dot)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
