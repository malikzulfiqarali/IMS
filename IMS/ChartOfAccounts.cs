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
    public partial class ChartOfAccounts : Form
    {
        public ChartOfAccounts()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (accountCodeTextBox.Text.Trim()==string.Empty)
            {
                MessageBox.Show("Account Code is required","Message",MessageBoxButtons.OK,MessageBoxIcon.Information);
                accountCodeTextBox.Focus();
                return;
            }
            if (accountCategoryComboBox.SelectedIndex==-1)
            {
                MessageBox.Show("Please select Category", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                accountCodeTextBox.Focus();
                return;

            }
            if (accountHeadTextBox.Text.Trim()==string.Empty)
            {
                MessageBox.Show("Account Head is required", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                accountCodeTextBox.Focus();
                return;

            }
            try
            {
                using (SqlConnection connection=new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM [dbo].[ChartOfAccounts] WHERE ID='"+IDTextBox.Text.Trim()+"'", connection);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        if (dateDateTimePicker.Value.Date==DateTime.Now.Date)
                        {
                            connection.Open();
                            SqlCommand cmd = new SqlCommand("[SP_UPDATE_CHART_OF_ACCOUNTS]", connection);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@ID",IDTextBox.Text.Trim());
                            cmd.Parameters.AddWithValue("@AccountCode", accountCodeTextBox.Text.Trim());
                            cmd.Parameters.AddWithValue("@AccountCategory", accountCategoryComboBox.SelectedItem.ToString().Trim());
                            cmd.Parameters.AddWithValue("@AccountHead", accountHeadTextBox.Text.Trim());
                            int result = cmd.ExecuteNonQuery();
                            if (result>0)
                            {
                                MessageBox.Show("Record updated successfully","Success",MessageBoxButtons.OK,MessageBoxIcon.Information);
                                LoadData();
                                clearButton.PerformClick();
                                connection.Close();
                            }
                            else
                            {
                                MessageBox.Show("Record  is not updated successfully", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                connection.Close();

                            }
                        }
                        else
                        {
                            MessageBox.Show("You cannot update Previouse date Record", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            clearButton.PerformClick();
                            connection.Close();

                        }
                    }
                    else
                    {
                        connection.Open();
                        SqlCommand cmd = new SqlCommand("[SP_INSERT_CHART_OF_ACCOUNTS]", connection);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Date",System.DateTime.Now);
                        cmd.Parameters.AddWithValue("@AccountCode",accountCodeTextBox.Text.Trim());
                        cmd.Parameters.AddWithValue("@AccountCategory",accountCategoryComboBox.SelectedItem.ToString().Trim());
                        cmd.Parameters.AddWithValue("@AccountHead",accountHeadTextBox.Text.Trim());
                        int success = cmd.ExecuteNonQuery();
                        if (success > 0)
                        {
                            MessageBox.Show("Record is inserted successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadData();
                            clearButton.PerformClick();
                            //accountCategoryComboBox.SelectedIndex=-1;
                            connection.Close();

                        }
                        else
                        {
                            MessageBox.Show("Record is not inserted successfully", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            connection.Close();

                        }
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void LoadData()
        {
            try
            {
                using (SqlConnection connection=new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM [dbo].[ChartOfAccounts]", connection);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    chartDataGridView.DataSource = dt;

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        private void ChartOfAccounts_Load(object sender, EventArgs e)
        {
            LoadData();
            clearButton.PerformClick();
            dateDateTimePicker.Value = DateTime.Now;
            
        }

        private void GetMaxNumber()
        {
            using (SqlConnection connection=new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT MAX(ID) FROM ChartOfAccounts", connection);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count>0)
                {
                    int maxNumber = Convert.ToInt32(dt.Rows[0][0]);
                    int incrementedNumber = maxNumber + 1;
                    IDTextBox.Text = incrementedNumber.ToString();
                }
            }
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            ClearAllTexBoxes();
            //accountCategoryComboBox.SelectedIndex = -1;
            saveButton.Text = "Save";
            GetMaxNumber();
            dateDateTimePicker.Value = DateTime.Now;
        }

        private void ClearAllTexBoxes()
        {
            foreach (TextBox textBox in this.Controls.OfType<TextBox>())
            {
                textBox.Clear();
            }
            accountCodeTextBox.Focus();
        }

        private void chartDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex>=0)
            {
                DataGridViewRow row = chartDataGridView.Rows[e.RowIndex];
                dateDateTimePicker.Value = (DateTime)(row.Cells["Date"].Value);
                IDTextBox.Text = row.Cells["ID"].Value.ToString();
                accountCodeTextBox.Text = row.Cells["AccountCode"].Value.ToString();
                accountCategoryComboBox.SelectedItem = row.Cells["AccountCategory"].Value.ToString();
                accountHeadTextBox.Text = row.Cells["AccountHead"].Value.ToString();
                saveButton.Text = "Update";
            }
        }

        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            string searchKeyWord = searchTextBox.Text.Trim();
            if (!string.IsNullOrWhiteSpace(searchKeyWord))
            {
                string filterExpression = string.Format("AccountCode LIKE '%{0}%' or AccountHead LIKE '%{0}%' or AccountCategory LIKE '%{0}%' ", searchKeyWord );
                (chartDataGridView.DataSource as DataTable).DefaultView.RowFilter = filterExpression;
            }
            else
            {
                (chartDataGridView.DataSource as DataTable).DefaultView.RowFilter = string.Empty;

            }

        }

        private void accountCodeTextBox_TextChanged(object sender, EventArgs e)
        {

        }
        private void DigitOnlyEntry(KeyPressEventArgs e, TextBox textBox)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

            if (char.IsDigit(e.KeyChar) && textBox.Text.Length >=10 )
            {
                e.Handled = true;
            }
        }

        private void accountCodeTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            DigitOnlyEntry(e,accountCodeTextBox);
        }

        private void accountCategoryComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
            {
                using (SqlDataAdapter da = new SqlDataAdapter("SELECT MAX(AccountCode) FROM ChartOfAccounts WHERE AccountCategory='" + accountCategoryComboBox.SelectedItem.ToString().Trim() + "' ", connection))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        long maxNumber = 0;
                        maxNumber = Convert.ToInt64(dt.Rows[0][0]);
                        long incrementNumber = maxNumber + 1;
                        accountCodeTextBox.Text = incrementNumber.ToString();
                    }
                }
            }
        }
    }
}
