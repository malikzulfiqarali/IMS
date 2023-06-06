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
using System.IO;
using System.Drawing.Imaging;
using System.Resources;
using IMS.Properties;

namespace IMS
{
    public partial class CustomerDefinitionForm : Form
    {
        public CustomerDefinitionForm()
        {
            InitializeComponent();

        }
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString);




        private void CustomerDefinitionForm_Load(object sender, EventArgs e)
        {
            LoadData();
            ClearTextBoxes(this);
            string query = "SP_GetCustomerMaxNumber";
            getMaxNumber(query);
            
        }
        private void LoadData()

        {
            try
            {
                connection.Open();
                SqlDataAdapter da = new SqlDataAdapter("select * from CustomerDetail ", connection);
                DataTable dt = new DataTable();
                da.Fill(dt);
                CustomerInfoDataGridView.DataSource = dt;
                connection.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void textBox13_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Please Choose an Image";
            openFileDialog.Filter = "Choose an image(*.jpg;*.png;*.gif)|*.jpg;*.png;*.gif";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                CustomerPictureBox.Image = Image.FromFile(openFileDialog.FileName);
            }
        }

        private void AddNewButton_Click(object sender, EventArgs e)
        {
            ClearButton.PerformClick();
            string query = "SP_GetCustomerMaxNumber";
            getMaxNumber(query);
            SaveButton.Text = "Save";

        }
        
        public void getMaxNumber(string query)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString);
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                int maxNumber = Convert.ToInt32(dt.Rows[0][0]);
                int incrementNumber = maxNumber + 1;
                CustomerIDTextBox.Text = incrementNumber.ToString();

            }
            else
            {
                MessageBox.Show("Data not found something went wrong");
            }

        }
        private byte[] SavePhoto()
        {
            MemoryStream ms = new MemoryStream();
            CustomerPictureBox.Image.Save(ms, CustomerPictureBox.Image.RawFormat);
            return ms.GetBuffer();

        }
        private Image GetPhoto(byte[] photo)
        {
            MemoryStream ms = new MemoryStream(photo);
            return Image.FromStream(ms);
        }


        private void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("SP_GetDataBasedOnCustomerID", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("CustomerID", CustomerIDTextBox.Text);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    SqlCommand command = new SqlCommand("[SP_UPDATE_CUSTOMER]", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("CustomerID", CustomerIDTextBox.Text);
                    command.Parameters.AddWithValue("CustomerName", CustomerNameTextBox.Text);
                    command.Parameters.AddWithValue("CustomerFatherName", CustomerFatherNameTextBox.Text);
                    command.Parameters.AddWithValue("CustomerAddress", CustomerAddressTextBox.Text);
                    command.Parameters.AddWithValue("CustomerProfession", CustomerProfessionTextBox.Text);
                    command.Parameters.AddWithValue("CustomerOfficeAddress", CustomerOfficeAddressTextBox.Text);
                    command.Parameters.AddWithValue("CustomerMobile", CustomerMobileTextBox.Text);
                    command.Parameters.AddWithValue("CustomerCNIC", CustomerCnicTextBox.Text);
                    command.Parameters.AddWithValue("CustomerImage", SavePhoto());
                    command.Parameters.AddWithValue("WitnessOneName", WitnessNameTextBox.Text);
                    command.Parameters.AddWithValue("WitnessOneFatherName", WitnessFatherNameTextBox.Text);
                    command.Parameters.AddWithValue("WitnessOneCNIC", WitnessCnicTextBox.Text);
                    command.Parameters.AddWithValue("WitnessOneMobile", WitnessMobileTextBox.Text);
                    command.Parameters.AddWithValue("WitnessOneProfession", WitnessProfessionTextBox.Text);
                    command.Parameters.AddWithValue("WitnessOneAddress", WitnessAddressTextBox.Text);
                    command.Parameters.AddWithValue("WitnessTwoName", SecondWitnessNameTextBox.Text);
                    command.Parameters.AddWithValue("WitnessTwoFatherName", SecondWitnessFatherNameTextBox.Text);
                    command.Parameters.AddWithValue("WitnessTwoCNIC", SecondWitnessCNICTextBox.Text);
                    command.Parameters.AddWithValue("WitnessTwoMobile", SeconWitnessMobileTextBox.Text);
                    command.Parameters.AddWithValue("WitnessTwoProfession", SecondWitnessProfessionTextBox.Text);
                    command.Parameters.AddWithValue("WitnessTwoAddress", SecondWitnessAddressTextBox.Text);
                    int result = command.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Records Updated SuccessFully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        connection.Close();
                        LoadData();
                        ClearButton.PerformClick();
                    }
                    else
                    {
                        MessageBox.Show("Record not Updated successfully", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                }
                else

                {
                    SqlCommand sqlCommand = new SqlCommand("SP_Insert_Customer", connection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("CustomerName", CustomerNameTextBox.Text);
                    sqlCommand.Parameters.AddWithValue("CustomerFatherName", CustomerFatherNameTextBox.Text);
                    sqlCommand.Parameters.AddWithValue("CustomerAddress", CustomerAddressTextBox.Text);
                    sqlCommand.Parameters.AddWithValue("CustomerProfession", CustomerProfessionTextBox.Text);
                    sqlCommand.Parameters.AddWithValue("CustomerOfficeAddress", CustomerOfficeAddressTextBox.Text);
                    sqlCommand.Parameters.AddWithValue("CustomerMobile", CustomerMobileTextBox.Text);
                    sqlCommand.Parameters.AddWithValue("CustomerCNIC", CustomerCnicTextBox.Text);
                    sqlCommand.Parameters.AddWithValue("Date", System.DateTime.Now);
                    sqlCommand.Parameters.AddWithValue("CustomerImage", SavePhoto());
                    sqlCommand.Parameters.AddWithValue("WitnessOneName", WitnessNameTextBox.Text);
                    sqlCommand.Parameters.AddWithValue("WitnessOneFatherName", WitnessFatherNameTextBox.Text);
                    sqlCommand.Parameters.AddWithValue("WitnessOneCNIC", WitnessCnicTextBox.Text);
                    sqlCommand.Parameters.AddWithValue("WitnessOneMobile", WitnessMobileTextBox.Text);
                    sqlCommand.Parameters.AddWithValue("WitnessOneProfession", WitnessProfessionTextBox.Text);
                    sqlCommand.Parameters.AddWithValue("WitnessOneAddress", WitnessAddressTextBox.Text);
                    sqlCommand.Parameters.AddWithValue("WitnessTwoName", SecondWitnessNameTextBox.Text);
                    sqlCommand.Parameters.AddWithValue("WitnessTwoFatherName", SecondWitnessFatherNameTextBox.Text);
                    sqlCommand.Parameters.AddWithValue("WitnessTwoCNIC", SecondWitnessCNICTextBox.Text);
                    sqlCommand.Parameters.AddWithValue("WitnessTwoMobile", SeconWitnessMobileTextBox.Text);
                    sqlCommand.Parameters.AddWithValue("WitnessTwoProfession", SecondWitnessProfessionTextBox.Text);
                    sqlCommand.Parameters.AddWithValue("WitnessTwoAddress", SecondWitnessAddressTextBox.Text);
                    int result = sqlCommand.ExecuteNonQuery();
                    if (result > 0)

                    {
                        MessageBox.Show("Records Inserted SuccessFully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        connection.Close();
                        LoadData();
                        AddNewButton.PerformClick();
                    }
                    else
                    {
                        MessageBox.Show("Record not inserted successfully", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

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

        private void ClearButton_Click(object sender, EventArgs e)
        {
            ClearTextBoxes(this);
            CustomerPictureBox.Image = Resources.No_Image_Available;
            string query = "SP_GetCustomerMaxNumber";
            getMaxNumber(query);
            SaveButton.Text = "Save";
        }
        private void ClearTextBoxes(Form form)
        {
            foreach (GroupBox groupBox in this.Controls.OfType<GroupBox>())
            {
                foreach (TextBox textBox in groupBox.Controls.OfType<TextBox>())
                {
                    textBox.Clear();
                }
            }
            CustomerNameTextBox.Focus();

        }




        private void CustomerPictureBox_Click(object sender, EventArgs e)
        {

        }

        private void SearchTextBox_TextChanged(object sender, EventArgs e)
        {
            //(CustomerInfoDataGridView.DataSource as DataTable).DefaultView.RowFilter = "CustomerName LIKE '%"+SearchTextBox+"%'";
           
                string searchTerm = SearchTextBox.Text;

                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    string filterExpression = string.Format("CustomerName LIKE '%{0}%' OR CustomerCNIC LIKE '%{0}%' OR CustomerMobile LIKE '%{0}%'", searchTerm);
                    (CustomerInfoDataGridView.DataSource as DataTable).DefaultView.RowFilter = filterExpression;
                }
                else
                {
                    (CustomerInfoDataGridView.DataSource as DataTable).DefaultView.RowFilter = string.Empty;
                }
            


        }

        private void CustomerIDTextBox_TextChanged(object sender, EventArgs e)
        {
        }

        private void CustomerIDTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void CustomerInfoDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void CustomerInfoDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow row = CustomerInfoDataGridView.Rows[e.RowIndex];

                // Get the value of the cells in the selected row
                CustomerDateTimePicker.Value = (DateTime)(row.Cells["Date"].Value);
                CustomerNameTextBox.Text = row.Cells["CustomerName"].Value.ToString();
                CustomerMobileTextBox.Text = row.Cells["CustomerMobile"].Value.ToString();
                CustomerCnicTextBox.Text = row.Cells["CustomerCNIC"].Value.ToString();
                CustomerFatherNameTextBox.Text = row.Cells["CustomerFatherName"].Value.ToString();
                CustomerAddressTextBox.Text = row.Cells["CustomerAddress"].Value.ToString();
                CustomerProfessionTextBox.Text = row.Cells["CustomerProfession"].Value.ToString();
                CustomerOfficeAddressTextBox.Text = row.Cells["CustomerOfficeAddress"].Value.ToString();
                WitnessNameTextBox.Text = row.Cells["WitnessOneName"].Value.ToString();
                WitnessFatherNameTextBox.Text = row.Cells["WitnessOneFatherName"].Value.ToString();
                WitnessCnicTextBox.Text = row.Cells["WitnessOneCNIC"].Value.ToString();
                WitnessMobileTextBox.Text = row.Cells["WitnessOneMobile"].Value.ToString();
                WitnessProfessionTextBox.Text = row.Cells["WitnessOneProfession"].Value.ToString();
                WitnessAddressTextBox.Text = row.Cells["WitnessOneAddress"].Value.ToString();
                SecondWitnessNameTextBox.Text = row.Cells["WitnessTwoName"].Value.ToString();
                SecondWitnessFatherNameTextBox.Text = row.Cells["WitnessTwoFatherName"].Value.ToString();
                SecondWitnessCNICTextBox.Text = row.Cells["WitnessTwoCNIC"].Value.ToString();
                SeconWitnessMobileTextBox.Text = row.Cells["WitnessTwoMobile"].Value.ToString();
                SecondWitnessProfessionTextBox.Text = row.Cells["WitnessTwoProfession"].Value.ToString();
                SecondWitnessAddressTextBox.Text = row.Cells["WitnessTwoAddress"].Value.ToString();
                CustomerIDTextBox.Text = row.Cells["CustomerID"].Value.ToString();
                CustomerPictureBox.Image = ((row.Cells["CustomerImage"].Value) is DBNull) ? Resources.No_Image_Available : GetPhoto((byte[])(row.Cells["CustomerImage"].Value));
                SaveButton.Text = "Update";

            }
            

        }

        private void CustomerCnicTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void CustomerCnicTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            DigitOnlyEntry(e,CustomerCnicTextBox);
        }
        private void DigitOnlyEntry(KeyPressEventArgs e,TextBox textBox)
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
        private void LettersOnlyEntry(KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }
        }


        private void CustomerMobileTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            DigitOnlyEntry(e,CustomerMobileTextBox);
        }

        private void WitnessCnicTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            DigitOnlyEntry(e,WitnessCnicTextBox);
        }

        private void WitnessMobileTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            DigitOnlyEntry(e,WitnessMobileTextBox);
        }

        private void SecondWitnessCNICTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            DigitOnlyEntry(e,SecondWitnessCNICTextBox);
        }

        private void SeconWitnessMobileTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            DigitOnlyEntry(e,SeconWitnessMobileTextBox);
        }

        private void CustomerNameTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            LettersOnlyEntry(e);
        }

        private void CustomerFatherNameTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            LettersOnlyEntry(e);
        }

        private void WitnessNameTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            LettersOnlyEntry(e);
        }

        private void WitnessFatherNameTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            LettersOnlyEntry(e);
        }

        private void SecondWitnessNameTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            LettersOnlyEntry(e);
        }

        private void SecondWitnessFatherNameTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            LettersOnlyEntry(e);
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SeconWitnessMobileTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void label22_Click(object sender, EventArgs e)
        {

        }

        private void WitnessMobileTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void CustomerOfficeAddressTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void CustomerNameTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void WitnessFatherNameTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void SecondWitnessNameTextBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
