﻿using System;
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
            cpvDataGridView.AllowUserToAddRows = true;
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
                            decimal Total = getCreditTotal();
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@VoucherCode",cpvTextBox.Text.Trim());
                            cmd.Parameters.AddWithValue("@VoucherType", cpvLabel.Text.Trim());
                            cmd.Parameters.AddWithValue("@VoucherDate", dateDateTimePicker.Value);
                            cmd.Parameters.AddWithValue("@Narration", narrationTextBox.Text.Trim());
                            cmd.Parameters.AddWithValue("@VoucherCategoryID", cashCodeTextBox.Text.Trim());
                            cmd.Parameters.AddWithValue("@VoucherCategory", categoryComboBox.SelectedItem);
                            cmd.Parameters.AddWithValue("@Description",cashLabel.Text.Trim());
                            cmd.Parameters.AddWithValue("@Credit", Total);
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
            updateButton.Enabled = false;
            saveButton.Enabled = true;
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

            if (cpvDataGridView.CurrentRow.Cells["Code"].Value==null || Convert.ToInt32( cpvDataGridView.CurrentRow.Cells["Code"].Value.ToString().Trim())==0)
            {
                cpvDataGridView.CurrentRow.Cells["Code"].Value = FetchDataForm.SetCode;
                cpvDataGridView.CurrentRow.Cells["Description"].Value = FetchDataForm.SetName;
                creditTotalTextBox.Text = getCreditTotal().ToString(); 
            }
            

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
            saveButton.Enabled = true;
            dateDateTimePicker.Value = DateTime.Now;
        }
        private void MaxNumberVoucherCode()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter("SELECT DISTINCT MAX(VoucherCode) FROM TransactionTable WHERE VoucherType='" + cpvLabel.Text.Trim() + "'", connection))
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
            SqlTransaction sqlTransaction = null;
            
            connection.Open();
            try
            {
                
                if (cpvDataGridView.Columns[e.ColumnIndex].Name == "RemoveColumnButton")
                {
                    if (dateDateTimePicker.Value.Date==DateTime.Now.Date)
                    {
                        sqlTransaction = connection.BeginTransaction();
                        int id = Convert.ToInt32(cpvDataGridView.Rows[e.RowIndex].Cells["ID"].Value);
                        string query = "DELETE from TransactionTable WHERE VoucherID=@VoucherID ";
                        SqlCommand cmd = new SqlCommand(query, connection, sqlTransaction);
                        cmd.Parameters.AddWithValue("@VoucherID", id);
                        cmd.ExecuteNonQuery();

                        cpvDataGridView.Rows.RemoveAt(e.RowIndex);

                        decimal Total = getCreditTotal();
                        string query1 = "UPDATE TransactionTable SET Narration=@Narration,VoucherCategory=@VoucherCategory,Description=@Description,Credit=@Credit where VoucherType=@VoucherType and VoucherCode=@VoucherCode and VoucherCategoryID=@VoucherCategoryID";
                        SqlCommand cmd1 = new SqlCommand(query1, connection, sqlTransaction);
                        cmd1.Parameters.AddWithValue("@Narration", narrationTextBox.Text.Trim());
                        cmd1.Parameters.AddWithValue("@VoucherCategory", categoryComboBox.SelectedItem);
                        cmd1.Parameters.AddWithValue("@Description", cashLabel.Text.Trim());
                        cmd1.Parameters.AddWithValue("@Credit", Total);
                        cmd1.Parameters.AddWithValue("@VoucherType", cpvLabel.Text.Trim());
                        cmd1.Parameters.AddWithValue("@VoucherCode", cpvTextBox.Text.Trim());
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
                        cpvDataGridView.Rows[i].Cells["ID"].Value = dt.Rows[i]["VoucherID"];

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
            finally
            {
                connection.Close();
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

        private void previousButton_Click(object sender, EventArgs e)
        {
            int voucherNumber = Convert.ToInt32(cpvTextBox.Text.Trim());
            int newNumber = voucherNumber - 1;
            cpvTextBox.Text = newNumber.ToString();
            cpvTextBox_Leave(sender, e);
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
            int voucherNumber = Convert.ToInt32(cpvTextBox.Text.Trim());
            int newNumber = voucherNumber + 1;
            cpvTextBox.Text = newNumber.ToString();
            cpvTextBox_Leave(sender, e);
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

        private void updateButton_Click(object sender, EventArgs e)
        {
            int cashCode = Convert.ToInt32(cashCodeTextBox.Text.Trim());
            connection.Open();

            SqlTransaction transaction = null;
            decimal Total = getCreditTotal();
            try
            {
                transaction = connection.BeginTransaction();
                foreach (DataGridViewRow row in cpvDataGridView.Rows)
                {

                    if (!row.IsNewRow)
                    {
                        int ID = Convert.ToInt32(row.Cells["ID"].Value);
                        

                        if (ID!=0)
                        {
                            string narration = narrationTextBox.Text.Trim();
                            int VCode = Convert.ToInt32(row.Cells["Code"].Value);
                            string VCat = categoryComboBox.SelectedItem.ToString();
                            string VCatCode =row.Cells["PartyCode"].Value.ToString();
                            string Description = row.Cells["Description"].Value.ToString();
                            decimal amount = Convert.ToDecimal(row.Cells["Amount"].Value);
                            string Remarks = row.Cells["Remarks"].Value.ToString();

                            string query = "UPDATE TransactionTable SET  Narration=@Narration,VoucherDate=@VoucherDate,VoucherCategoryID=@VoucherCategoryID,VoucherCategory=@VoucherCategory,VoucherCategoryCode=@VoucherCategoryCode,Description=@Description,Debit=@Debit,Remarks=@Remarks where VoucherID=@VoucherID and VoucherType=@VoucherType and VoucherCode=@VoucherCode and VoucherCategoryID!='" + cashCode + "'";
                            SqlCommand cmd = new SqlCommand(query, connection, transaction);
                            cmd.Parameters.AddWithValue("@VoucherID", ID);
                            cmd.Parameters.AddWithValue("@Narration", narration);
                            cmd.Parameters.AddWithValue("@VoucherDate", dateDateTimePicker.Value);
                            cmd.Parameters.AddWithValue("@VoucherCategoryID", VCode);
                            cmd.Parameters.AddWithValue("@VoucherCategory", VCat);
                            cmd.Parameters.AddWithValue("@VoucherCategoryCode", VCatCode);
                            cmd.Parameters.AddWithValue("@Description", Description);
                            cmd.Parameters.AddWithValue("@Debit", amount);
                            cmd.Parameters.AddWithValue("@Remarks", Remarks);
                            cmd.Parameters.AddWithValue("@VoucherType", cpvLabel.Text.Trim());
                            cmd.Parameters.AddWithValue("@VoucherCode", cpvTextBox.Text.Trim());
                            cmd.ExecuteNonQuery(); 
                        }


                        if (ID == 0)
                        {
                            

                            string query2 = "insert into TransactionTable (Narration,VoucherDate,VoucherCategoryID,VoucherCategory,VoucherCategoryCode,Description,Debit,Remarks,VoucherType,VoucherCode)values(@Narration,@VoucherDate,@VoucherCategoryID,@VoucherCategory,@VoucherCategoryCode,@Description,@Debit,@Remarks,@VoucherType,@VoucherCode)";
                            SqlCommand cmd2 = new SqlCommand(query2, connection, transaction);
                            cmd2.Parameters.AddWithValue("@Narration", narrationTextBox.Text.Trim());
                            cmd2.Parameters.AddWithValue("@VoucherDate", dateDateTimePicker.Value);
                            cmd2.Parameters.AddWithValue("@VoucherCategoryID", Convert.ToInt32(row.Cells["Code"].Value));
                            cmd2.Parameters.AddWithValue("@VoucherCategory", categoryComboBox.SelectedItem.ToString());
                            cmd2.Parameters.AddWithValue("@VoucherCategoryCode", row.Cells["PartyCode"].Value??DBNull.Value);
                            cmd2.Parameters.AddWithValue("@Description", row.Cells["Description"].Value ?? DBNull.Value);
                            cmd2.Parameters.AddWithValue("@Debit", Convert.ToDecimal(row.Cells["Amount"].Value));
                            cmd2.Parameters.AddWithValue("@Remarks", row.Cells["Remarks"].Value ?? DBNull.Value);
                            cmd2.Parameters.AddWithValue("@VoucherType", cpvLabel.Text.Trim());
                            cmd2.Parameters.AddWithValue("@VoucherCode", cpvTextBox.Text.Trim());
                            cmd2.ExecuteNonQuery();


                        }

                    }
                }
                string query1 = "UPDATE TransactionTable SET Narration=@Narration,VoucherCategory=@VoucherCategory,Description=@Description,Credit=@Credit where VoucherType=@VoucherType and VoucherCode=@VoucherCode and VoucherCategoryID=@VoucherCategoryID";
                SqlCommand cmd1 = new SqlCommand(query1, connection, transaction);
                cmd1.Parameters.AddWithValue("@Narration", narrationTextBox.Text.Trim());
                cmd1.Parameters.AddWithValue("@VoucherCategory", categoryComboBox.SelectedItem);
                cmd1.Parameters.AddWithValue("@Description", cashLabel.Text.Trim());
                cmd1.Parameters.AddWithValue("@Credit", Total);
                cmd1.Parameters.AddWithValue("@VoucherType", cpvLabel.Text.Trim());
                cmd1.Parameters.AddWithValue("@VoucherCode", cpvTextBox.Text.Trim());
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
    }
}
