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
    public partial class addNewCompanyForm : Form
    {
        public addNewCompanyForm()
        {
            InitializeComponent();
        }
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString);

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void AddNewCompanyForm_Load(object sender, EventArgs e)
        {
            GetMaxNumber();
            companyNameTextBox.Focus();
        }
        private void GetMaxNumber()
        {
            {
                connection.Open();
                SqlDataAdapter da = new SqlDataAdapter("select max(CompanyID) from [dbo].[CompanyDetail]", connection);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    int maxNumber = (int)(dt.Rows[0][0]);
                    int incrementNumber = maxNumber + 1;
                    companyIDTextBox.Text = incrementNumber.ToString();
                    connection.Close();
                }
                else
                {
                    MessageBox.Show("Something Went Wrong", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    connection.Close();
                }
            }
        }

        private void saveAndCloseButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (companyNameTextBox.Text.Trim()==string.Empty)
                {
                    MessageBox.Show("Company Name field Can not be empty","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    return;
                }
                if (companyContactTextBox.Text.Trim()==string.Empty)
                {
                    MessageBox.Show("Company Contact field Can not be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (companyAddressTextBox.Text.Trim()==string.Empty)
                {
                    MessageBox.Show("Company Address field Can not be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (contactingPersonTextBox.Text.Trim()==string.Empty)
                {
                    MessageBox.Show("Contact Person Name field Can not be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                connection.Open();
                SqlCommand cmd = new SqlCommand("SP_INSERT_COMPANY", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Date",System.DateTime.Now);
                cmd.Parameters.AddWithValue("@Company",companyNameTextBox.Text);
                cmd.Parameters.AddWithValue("@CompanyContact", companyContactTextBox.Text);
                cmd.Parameters.AddWithValue("@CompanyAddress", companyAddressTextBox.Text);
                cmd.Parameters.AddWithValue("@CompanyContactPerson", contactingPersonTextBox.Text);
                int result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    MessageBox.Show("Records are inserted successfully","Success",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    connection.Close();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Records are not inserted successfully", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void companyNameTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if ((!char.IsControl(e.KeyChar)) && (!char.IsDigit(e.KeyChar)))
            //{
            //    e.Handled = true;
            //}
        }

        private void companyContactTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((!char.IsControl(e.KeyChar)) && (!char.IsDigit(e.KeyChar)))
            {
                e.Handled = true;
            }
        }
    }
}
