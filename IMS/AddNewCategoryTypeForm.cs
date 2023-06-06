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
    public partial class AddNewCategoryTypeForm : Form
    {
        public AddNewCategoryTypeForm()
        {
            InitializeComponent();
        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void saveAndCloseButton_Click(object sender, EventArgs e)
        {
            if (categoryTypeTextBox.Text == string.Empty)
            {
                MessageBox.Show("You must enter category Name in the required field","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                categoryTypeTextBox.Focus();
                return;
            }
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("SP_INSERT_CATEGORYTYPE",connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Date",System.DateTime.Now);
                cmd.Parameters.AddWithValue("@CategoryTypeName", categoryTypeTextBox.Text);
                int result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    MessageBox.Show("Rocords are Inserted successfully","Success",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Rocords are not Inserted successfully", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }

        }
        private void GetMaxNumber()
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter("select max(CategoryTypeID) from [dbo].[CategoryType]", connection);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    int maxNumber = (int)(dt.Rows[0][0]);
                    int incrementNumber = maxNumber + 1;
                    categoryTypeIdTextBox.Text = incrementNumber.ToString();
                }
                else
                {
                    MessageBox.Show("Something Went Wrong", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                   
                }
            }
            
        }

        private void AddNewCategoryTypeForm_Load(object sender, EventArgs e)
        {
            GetMaxNumber();
        }
    }
}
