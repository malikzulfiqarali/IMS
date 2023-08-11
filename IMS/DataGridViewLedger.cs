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
                SqlCommand cmd = new SqlCommand("[USP_LEDGER]", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@VoucherCategoryID",codeTextBox.Text.Trim());
                cmd.Parameters.AddWithValue("@StartDate",startDateTimePicker.Value.Date);
                cmd.Parameters.AddWithValue("@EndDate",endDateTimePicker.Value.Date);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                ledgerDataGridView.Rows.Clear();
                ledgerDataGridView.DataSource = dt;
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
    }

}
