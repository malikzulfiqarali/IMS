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
    public partial class VendorDetailForm : Form
    {
        public VendorDetailForm()
        {
            InitializeComponent();
        }

        private void VendorDetailForm_Load(object sender, EventArgs e)
        {
            clearButton.PerformClick();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void ClearAllTextBoxes(Form form)
        {
            foreach (TextBox textBox in this.Controls.OfType<TextBox>())
            {
                textBox.Clear(); 
            }
            nameTextBox.Focus();

        }
        private void GetMaxNumber()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT MAX(VID) FROM VendorDetail", connection);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    int MaxNumber = Convert.ToInt32(dt.Rows[0][0]);
                    int incrementedNumber = MaxNumber + 1;
                    vidTextBox.Text = incrementedNumber.ToString();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection=new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
                {
                    if (nameTextBox.Text.Trim()==string.Empty)
                    {
                        MessageBox.Show("Name is required","Message",MessageBoxButtons.OK,MessageBoxIcon.Information);
                        nameTextBox.Focus();
                        return;
                    }
                    if (companyTextBox.Text.Trim()==string.Empty)
                    {
                        MessageBox.Show("Company Field is required", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        nameTextBox.Focus();
                        return;
                    }
                    if (mobileTextBox.Text.Trim()==string.Empty)
                    {
                        MessageBox.Show("Mobile Number is required", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        nameTextBox.Focus();
                        return;
                    }
                    if (addressTextBox.Text.Trim()==string.Empty)
                    {
                        MessageBox.Show("Address Field is required", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        nameTextBox.Focus();
                        return;

                    }
                    SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM VendorDetail WHERE VID='"+vidTextBox.Text.Trim()+"'",connection);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if(dt.Rows.Count>0)
                    {
                        if (((DateTime)(dt.Rows[0]["Date"])).Date == System.DateTime.Now.Date)
                        {
                            connection.Open();
                            SqlCommand cmd = new SqlCommand("[SP_UPDATE_VENDOR]", connection);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@VID",vidTextBox.Text.Trim());
                            cmd.Parameters.AddWithValue("@Name", nameTextBox.Text.Trim());
                            cmd.Parameters.AddWithValue("@Company", companyTextBox.Text.Trim());
                            cmd.Parameters.AddWithValue("@CNIC", cnicTextBox.Text.Trim());
                            cmd.Parameters.AddWithValue("@Mobile", mobileTextBox.Text.Trim());
                            cmd.Parameters.AddWithValue("@Designation", designationTextBox.Text.Trim());
                            cmd.Parameters.AddWithValue("@Address", addressTextBox.Text.Trim());
                            int result = cmd.ExecuteNonQuery();
                            if (result>0)
                            {
                                MessageBox.Show("Record is updated Successfully","Success",MessageBoxButtons.OK,MessageBoxIcon.Information);
                                clearButton.PerformClick();
                                connection.Close();
                            }
                            else
                            {
                                MessageBox.Show("Record is not updated Successfully", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            }
                        }
                        else
                        {
                            MessageBox.Show("You cannot Update Previouse Date Record","Message",MessageBoxButtons.OK,MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        connection.Open();
                        SqlCommand cmd = new SqlCommand("[SP_INSERT_VENDOR]", connection);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Date",dateDateTimePicker.Value);
                        cmd.Parameters.AddWithValue("@Name",nameTextBox.Text.Trim());
                        cmd.Parameters.AddWithValue("@Company", companyTextBox.Text.Trim());
                        cmd.Parameters.AddWithValue("@CNIC", cnicTextBox.Text.Trim());
                        cmd.Parameters.AddWithValue("@Mobile", mobileTextBox.Text.Trim());
                        cmd.Parameters.AddWithValue("@Designation", designationTextBox.Text.Trim());
                        cmd.Parameters.AddWithValue("@Address", addressTextBox.Text.Trim());
                        int success = cmd.ExecuteNonQuery();
                        if (success > 0)
                        {
                            MessageBox.Show("Record Inserted Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            clearButton.PerformClick();
                            connection.Close();
                        }
                        else
                        {
                            MessageBox.Show("Record is not inserted Successfully", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        }
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
            ClearAllTextBoxes(this);
            GetMaxNumber();
            LoadData();
            saveButton.Text = "Save";
        }

        private void LoadData()
        {
            using (SqlConnection connection=new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM VendorDetail",connection);
                DataTable dt = new DataTable();
                da.Fill(dt);
                vendorDataGridView.DataSource = dt;
            }
        }

        private void vendorDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex>=0)
            {
                DataGridViewRow row = vendorDataGridView.Rows[e.RowIndex];
                dateDateTimePicker.Value = (DateTime)(row.Cells["Date"].Value);
                vidTextBox.Text = row.Cells["VID"].Value.ToString();
                nameTextBox.Text = row.Cells["Name"].Value.ToString();
                companyTextBox.Text = row.Cells["Company"].Value.ToString();
                cnicTextBox.Text = row.Cells["CNIC"].Value.ToString();
                mobileTextBox.Text = row.Cells["Mobile"].Value.ToString();
                designationTextBox.Text = row.Cells["Designation"].Value.ToString();
                addressTextBox.Text = row.Cells["Address"].Value.ToString();
                saveButton.Text = "Update";
            }
        }
        private void DigitOnlyEntry(KeyPressEventArgs e, TextBox textBox)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

            if (char.IsDigit(e.KeyChar) && textBox.Text.Length >= 13)
            {
                e.Handled = true;
            }
        }

        private void cnicTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            DigitOnlyEntry(e,cnicTextBox);
        }

        private void mobileTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            DigitOnlyEntry(e,mobileTextBox);
        }

        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            string searchTerm = searchTextBox.Text;

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                string filterExpression = string.Format("Name LIKE '%{0}%' OR CNIC LIKE '%{0}%' OR Company LIKE '%{0}%' OR Mobile LIKE '%{0}%'", searchTerm);
                (vendorDataGridView.DataSource as DataTable).DefaultView.RowFilter = filterExpression;
            }
            else
            {
                (vendorDataGridView.DataSource as DataTable).DefaultView.RowFilter = string.Empty;
            }
        }
    }
}
