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
    public partial class DataGridViewLedger : Form
    {
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString);
        
        public DataGridViewLedger()
        {
            InitializeComponent();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void codeTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                FetchDataForm fetchDataForm = new FetchDataForm();
                fetchDataForm.ShowDialog();
                e.Handled = true;

            }
           
        }

        private void codeTextBox_DoubleClick(object sender, EventArgs e)
        {
            FetchDataForm fetchDataForm = new FetchDataForm();
            fetchDataForm.ShowDialog();
            
        }

        private void codeTextBox_Leave(object sender, EventArgs e)
        {
            string codeValue = codeTextBox.Text;
            if (string.IsNullOrEmpty(codeTextBox.Text))
            {
                codeValue = FetchDataForm.SetCode.ToString().Trim();
                descriptionTextBox.Text = FetchDataForm.SetName;
                codeTextBox.Text = codeValue;
                
            }
        }

        private void DataGridViewLedger_Load(object sender, EventArgs e)
        {
            codeTextBox.Focus();
            DataGridViewCellStyle headerStyle = new DataGridViewCellStyle();
            headerStyle.Font = new Font("Arial", 14, FontStyle.Bold);
            ledgerDataGridView.ColumnHeadersDefaultCellStyle = headerStyle;
        }

        private void previewButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(codeTextBox.Text))
            {
                MessageBox.Show("You must select a code to retrieve required information", "Information", MessageBoxButtons.OK,MessageBoxIcon.Information);
                codeTextBox.Focus();
                return;
            }
            if (string.IsNullOrEmpty(descriptionTextBox.Text))
            {
                MessageBox.Show("Description field is required", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                codeTextBox.Focus();
                return;
            }
            try
            {
                connection.Open();
                SqlCommand cmd1 = new SqlCommand("[SP_OPENING_BALANCE]", connection);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@VoucherCategoryID", codeTextBox.Text.Trim());
                cmd1.Parameters.AddWithValue("@StartDate", startDateTimePicker.Value.Date);
                SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                DataTable dt1 = new DataTable();
                da1.Fill(dt1);
                if (dt1.Rows.Count > 0)
                {
                    string openingBalance = Convert.ToString(dt1.Rows[0][0]);
                    if (string.IsNullOrEmpty(openingBalance))
                    {
                        openingBalanceTextBox.Text = "0";
                    }
                    else
                    {
                        openingBalanceTextBox.Text = openingBalance;
                    }

                    
                }
                connection.Close();


                connection.Open();
                SqlCommand cmd = new SqlCommand("[USP_LEDGER]", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@VoucherCategoryID",codeTextBox.Text.Trim());
                cmd.Parameters.AddWithValue("@StartDate",startDateTimePicker.Value.Date);
                cmd.Parameters.AddWithValue("@EndDate",endDateTimePicker.Value.Date);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                ledgerDataGridView.DataSource = null;
                ledgerDataGridView.Rows.Clear();
                ledgerDataGridView.AutoGenerateColumns = false;
                ledgerDataGridView.DataSource = dt;
                connection.Close();

                debitTotalTextBox.Text = Convert.ToString (GetDebitTotal());
                creditTotalTextBox.Text = Convert.ToString (GetCreditTotal());
                decimal opeing =Convert.ToDecimal( openingBalanceTextBox.Text);
                decimal debit = Convert.ToDecimal(debitTotalTextBox.Text);
                decimal credit = Convert.ToDecimal(creditTotalTextBox.Text);
                decimal closing = opeing + debit - credit;
                closingBalanceTextBox.Text = closing.ToString();



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
        decimal debitTotal = 0;
        private decimal GetDebitTotal()
        {
            foreach (DataGridViewRow row in ledgerDataGridView.Rows)
            {
                debitTotal +=Convert.ToDecimal( row.Cells["Debit"].Value);
            }
            return debitTotal;
        }
        decimal creditTotal = 0;
        private decimal GetCreditTotal()
        {
            foreach (DataGridViewRow row in ledgerDataGridView.Rows)
            {
                creditTotal += Convert.ToDecimal(row.Cells["Credit"].Value);
            }
            return creditTotal;
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            foreach (TextBox textBox in this.Controls.OfType<TextBox>())
            {
                textBox.Clear();
            }

            ledgerDataGridView.DataSource = null;
        }
    }

}
