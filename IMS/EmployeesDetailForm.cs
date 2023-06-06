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
using IMS.Properties;
using System.Configuration;

namespace IMS
{
    public partial class EmployeesDetailForm : Form
    {
        public EmployeesDetailForm()
        {
            InitializeComponent();

        }

        private void ClearAllTextBoxes(Form form)
        {
            foreach (TextBox textBox in this.Controls.OfType<TextBox>())
            {
                textBox.Clear();
            }
            nameTextBox.Focus();
        }
        private void loadData()
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
            {
                using (SqlDataAdapter da=new SqlDataAdapter("SELECT * FROM EmployeesDetail",connection))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    employeesDataGridView.DataSource = dt;
                }
            }
           
        }
        private void GetMaxNumber()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter("SELECT MAX(EID) FROM EmployeesDetail", connection))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            int Number = Convert.ToInt32(dt.Rows[0][0]);
                            int incrementedNumber = Number + 1;
                            eidTextBox.Text = incrementedNumber.ToString();
                        }
                        

                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }
        private void EmployeesDetailForm_Load(object sender, EventArgs e)
        {
            ClearAllTextBoxes(this);
            loadData();
            GetMaxNumber();
            saveButton.Text = "Save";

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString);
                        
            try
            {

                if (nameTextBox.Text==string.Empty)
                {
                    MessageBox.Show("Name is Required","Message",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    nameTextBox.Focus();
                    return;

                }
                if (fatherTextBox.Text==string.Empty)
                {
                    MessageBox.Show("Father Name is required", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    fatherTextBox.Focus();
                    return;

                }
                if (addressTextBox.Text == string.Empty)
                {
                    MessageBox.Show("Address is required", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    addressTextBox.Focus();
                    return;

                }
                if (mobileTextBox.Text==string.Empty)
                {
                    MessageBox.Show("Mobile Number is required", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    mobileTextBox.Focus();
                    return;
                }
                if (designationTextBox.Text==string.Empty)
                {
                    MessageBox.Show("Designation Field is required", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    designationTextBox.Focus();
                    return;
                }
                connection.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM EmployeesDetail WHERE EID='"+eidTextBox.Text.Trim()+"'",connection);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    if (((DateTime)(dt.Rows[0]["Date"])).Date==System.DateTime.Now.Date)
                    {
                        SqlCommand cmd = new SqlCommand("SP_UPDATE_EMPLOYEE", connection);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@EID", eidTextBox.Text.Trim());
                        cmd.Parameters.AddWithValue("@Name",nameTextBox.Text.Trim());
                        cmd.Parameters.AddWithValue("@Father", fatherTextBox.Text.Trim());
                        cmd.Parameters.AddWithValue("@CNIC", cnicTextBox.Text.Trim());
                        cmd.Parameters.AddWithValue("@DOB", dobDateTimePicker.Value);
                        cmd.Parameters.AddWithValue("@Mobile", mobileTextBox.Text.Trim());
                        cmd.Parameters.AddWithValue("@Designation", designationTextBox.Text.Trim());
                        cmd.Parameters.AddWithValue("@Address", addressTextBox.Text.Trim());
                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Record Updated Successfully","Success",MessageBoxButtons.OK,MessageBoxIcon.Information);
                            clearButton.PerformClick();
                            loadData();
                            saveButton.Text = "Save";
                            connection.Close();
                        }
                        else
                        {
                            MessageBox.Show("Record is not Updated Successfully", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            connection.Close();
                        }
                        
                    }
                    else
                    {
                        MessageBox.Show("You cannot update previouse Date Record","Error",MessageBoxButtons.OK,MessageBoxIcon.Information);
                        connection.Close();
                    }
                    
                }
                else
                {
                    SqlCommand cmd = new SqlCommand("[SP_INSERT_EMPLOYEE]", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Date",System.DateTime.Now);
                    cmd.Parameters.AddWithValue("@Name",nameTextBox.Text.Trim());
                    cmd.Parameters.AddWithValue("@Father", fatherTextBox.Text.Trim());
                    cmd.Parameters.AddWithValue("@CNIC", cnicTextBox.Text.Trim());
                    cmd.Parameters.AddWithValue("@DOB", dobDateTimePicker.Value);
                    cmd.Parameters.AddWithValue("@Mobile", mobileTextBox.Text.Trim());
                    cmd.Parameters.AddWithValue("@Designation", designationTextBox.Text.Trim());
                    cmd.Parameters.AddWithValue("@Address", addressTextBox.Text.Trim());
                    int success = cmd.ExecuteNonQuery();
                    if (success>0)
                    {
                        MessageBox.Show("Record Inserted Successfully","Message",MessageBoxButtons.OK,MessageBoxIcon.Information);
                        clearButton.PerformClick();
                        loadData();
                        connection.Close();
                        
                    }
                    else
                    {
                        MessageBox.Show("Record is not Inserted Successfully", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        connection.Close();
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

        private void employeesDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >=0)
            {
                DataGridViewRow row = employeesDataGridView.Rows[e.RowIndex];
                dateDateTimePicker.Value = (DateTime)(row.Cells["Date"].Value);
                eidTextBox.Text = row.Cells["EID"].Value.ToString();
                nameTextBox.Text = row.Cells["Name"].Value.ToString();
                fatherTextBox.Text = row.Cells["Father"].Value.ToString();
                cnicTextBox.Text = row.Cells["CNIC"].Value.ToString();
                dobDateTimePicker.Value = (DateTime)(row.Cells["DOB"].Value);
                mobileTextBox.Text = row.Cells["Mobile"].Value.ToString();
                designationTextBox.Text = row.Cells["Designation"].Value.ToString();
                addressTextBox.Text = row.Cells["Address"].Value.ToString();
                saveButton.Text = "Update";

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ClearAllTextBoxes(this);
            GetMaxNumber();
            saveButton.Text = "Save";
            
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

        private void mobileTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            DigitOnlyEntry(e, mobileTextBox);
        }

        private void cnicTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            DigitOnlyEntry(e, cnicTextBox);
        }

        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void searchTextBox_TextChanged_1(object sender, EventArgs e)
        {
            string searchTerm = searchTextBox.Text;

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                string filterExpression = string.Format("Name LIKE '%{0}%' OR CNIC LIKE '%{0}%' OR Father LIKE '%{0}%' OR Mobile LIKE '%{0}%'", searchTerm);
                (employeesDataGridView.DataSource as DataTable).DefaultView.RowFilter = filterExpression;
            }
            else
            {
                (employeesDataGridView.DataSource as DataTable).DefaultView.RowFilter = string.Empty;
            }
        }
    }
}
