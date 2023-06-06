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
using System.IO;
using System.Configuration;

namespace IMS
{
    public partial class ProductDefinitionForm : Form
    {
       SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString);

        public ProductDefinitionForm()
        {
            InitializeComponent();
        }

        private void ProductDefinitionForm_Load(object sender, EventArgs e)
        {
            FillCategoryComboBox();
            fillCompanyComboBox();
            FillModelComboBox();
            loadData();
            clearAllTextBoxesAndComboxes(this);
            getMaxNumber("select MAX(ProductID) from Product");
            saveButton.Text = "Save";
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void productDescriptionTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void productIDTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }
        private void loadData()
        {
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter("select * from Product",connection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            productInfoDataGridView.DataSource = dt;
            connection.Close();
        }
        public void getMaxNumber(string query)
        {
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter(query,connection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                int maxNumber = Convert.ToInt32(dt.Rows[0][0]);
                int incrementNumber = maxNumber + 1;
                productIDTextBox.Text = incrementNumber.ToString();
            }
            else
            {
                MessageBox.Show("Data not found something went wrong");
                
            }
            connection.Close();

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void FillCategoryComboBox()
        {
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter("SELECT CategoryID,Category FROM CATEGORYTABLE",connection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            categoryComboBox.DataSource = dt;
            categoryComboBox.ValueMember = "CategoryID";
            categoryComboBox.DisplayMember = "Category";
            connection.Close();
        }
        private void FillModelComboBox()
        {
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter("SELECT CategoryTypeID,CategoryTypeName FROM [dbo].[CategoryType]", connection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            modelComboBox.DataSource = dt;
            modelComboBox.ValueMember = "CategoryTypeID";
            modelComboBox.DisplayMember = "CategoryTypeName";
            connection.Close();
        }
        private void fillCompanyComboBox()
        {
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter("SELECT CompanyID,Company FROM CompanyDetail", connection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            companyComboBox.DataSource = dt;
            companyComboBox.ValueMember = "CompanyID";
            companyComboBox.DisplayMember = "Company";
            connection.Close();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            ValidationComboBox();
            try
            {
                connection.Open();
                SqlDataAdapter da = new SqlDataAdapter("select * from Product where ProductID='"+productIDTextBox.Text.Trim()+"'",connection);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {

                    if (((DateTime)dt.Rows[0]["Date"]).Date==(DateTime.Now).Date)
                    {
                        SqlCommand command = new SqlCommand("SP_UPDATE_PRODUCT", connection);
                        {
                            command.CommandType = CommandType.StoredProcedure;
                        }
                        command.Parameters.AddWithValue("@ProductID", productIDTextBox.Text.Trim());
                        command.Parameters.AddWithValue("@CategoryID", categoryComboBox.SelectedValue.ToString());
                        command.Parameters.AddWithValue("@CompanyID", companyComboBox.SelectedValue.ToString());
                        command.Parameters.AddWithValue("CategoryTypeID",modelComboBox.SelectedValue.ToString());
                        command.Parameters.AddWithValue("@ProductDescription", productDescriptionTextBox.Text);
                        command.Parameters.AddWithValue("@Quantity", quantityTextBox.Text);
                        command.Parameters.AddWithValue("@CostPrice", Convert.ToDecimal(costPriceTextBox.Text));
                        command.Parameters.AddWithValue("@SalePrice", Convert.ToDecimal(salePriceTextBox.Text));
                        command.Parameters.AddWithValue("@FixedPrice", Convert.ToDecimal(fixedPriceTextBox.Text));
                        command.Parameters.AddWithValue("@RetailPrice", Convert.ToDecimal(retailPriceTextBox.Text));
                        int success = command.ExecuteNonQuery();
                        if (success > 0)
                        {
                            MessageBox.Show("Record updated successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            connection.Close();
                            clearAllTextBoxesAndComboxes(this);
                            getMaxNumber("select MAX(ProductID) from Product");
                            saveButton.Text = "Save";
                            loadData();
                        }
                        else
                        {
                            MessageBox.Show("Record is updated successfully", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);


                        }
                    }
                    else
                    {
                        MessageBox.Show("You can not update previous date record", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }


                }
                else
                {
                    SqlCommand cmd = new SqlCommand("SP_INSERT_PRODUCT", connection);
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                    }
                    cmd.Parameters.AddWithValue("@Date", System.DateTime.Now);
                    cmd.Parameters.AddWithValue("@ProductCode", Guid.NewGuid().ToString());
                    cmd.Parameters.AddWithValue("@ProductDescription", productDescriptionTextBox.Text);
                    cmd.Parameters.AddWithValue("@CategoryID", categoryComboBox.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@CompanyID", companyComboBox.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("CategoryTypeID", modelComboBox.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@Quantity", quantityTextBox.Text);
                    cmd.Parameters.AddWithValue("@CostPrice", Convert.ToDecimal(costPriceTextBox.Text));
                    cmd.Parameters.AddWithValue("@SalePrice", Convert.ToDecimal(salePriceTextBox.Text));
                    cmd.Parameters.AddWithValue("@FixedPrice", Convert.ToDecimal(fixedPriceTextBox.Text));
                    cmd.Parameters.AddWithValue("@RetailPrice", Convert.ToDecimal(retailPriceTextBox.Text));
                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Record inserted successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        connection.Close();
                        clearAllTextBoxesAndComboxes(this);
                        getMaxNumber("select MAX(ProductID) from Product");
                        loadData();
                    }
                    else
                    {
                        MessageBox.Show("Record is not inserted successfully", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

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

        private void productInfoDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = productInfoDataGridView.Rows[e.RowIndex];

                // Get the value of the cells in the selected row
                productDateTimePicker.Value = (DateTime)(row.Cells["Date"].Value);
                productIDTextBox.Text = row.Cells["ProductID"].Value.ToString();
                categoryComboBox.SelectedValue= row.Cells["CategoryID"].Value.ToString();
                companyComboBox.SelectedValue= row.Cells["CompanyID"].Value.ToString();
                modelComboBox.SelectedValue = row.Cells["CategoryTypeID"].Value.ToString();
                productCodeTextBox.Text = row.Cells["ProductCode"].Value.ToString();
                productDescriptionTextBox.Text = row.Cells["ProductDescription"].Value.ToString();
                quantityTextBox.Text = row.Cells["Quantity"].Value.ToString();
                costPriceTextBox.Text = row.Cells["CostPrice"].Value.ToString();
                salePriceTextBox.Text = row.Cells["SalePrice"].Value.ToString();
                fixedPriceTextBox.Text = row.Cells["FixedPrice"].Value.ToString();
                retailPriceTextBox.Text = row.Cells["RetailPrice"].Value.ToString();
                saveButton.Text = "Update";
                

            }
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            clearAllTextBoxesAndComboxes(this);
            getMaxNumber("select MAX(ProductID) from Product");
            saveButton.Text = "Save";
        }

        private void clearAllTextBoxesAndComboxes(Form form)
        {
            foreach (TextBox textBox in this.Controls.OfType<TextBox>())
            {
                textBox.Clear();
            }
            foreach (ComboBox comboBox in this.Controls.OfType<ComboBox>())
            {
                comboBox.SelectedIndex = -1;

             }
        }
        private void ValidationComboBox()
        {
            if(categoryComboBox.SelectedIndex == -1 && companyComboBox.SelectedIndex == -1 && modelComboBox.SelectedIndex==-1)
            {
                MessageBox.Show("Please select any item from Category and Company","Warning",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return;
            }
            if (quantityTextBox.Text==string.Empty && costPriceTextBox.Text == string.Empty && salePriceTextBox.Text==string.Empty && fixedPriceTextBox.Text==string.Empty && retailPriceTextBox.Text==string.Empty)
            {
                MessageBox.Show("Please enter amounts in cost price, sale price, fixed price, retail price in respective Text Boxes", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }
        }

        private void companyComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (companyComboBox.SelectedIndex == -1)
            {
                companyComboBox.Text = "----Select----";
            }
        }

        private void categoryComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (categoryComboBox.SelectedIndex == -1)
            {
                categoryComboBox.Text = "---------------------Select-----------------";
            }
        }
        private void digitOnlyEntry(KeyPressEventArgs e,TextBox textBox)
        {
            if ((!char.IsControl(e.KeyChar)) && (!char.IsDigit(e.KeyChar)))
            {
                e.Handled = true;
            }
        }

        private void quantityTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            digitOnlyEntry(e, quantityTextBox);
        }

        private void costPriceTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            digitOnlyEntry(e,costPriceTextBox);
        }

        private void salePriceTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            digitOnlyEntry(e,salePriceTextBox);
        }

        private void fixedPriceTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            digitOnlyEntry(e,fixedPriceTextBox);
        }

        private void retailPriceTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            digitOnlyEntry(e,retailPriceTextBox);
        }

        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            string searchKeyword = searchTextBox.Text;

            if (!string.IsNullOrWhiteSpace(searchKeyword))
            {
                string filterExpression = string.Format("ProductCode LIKE '%{0}%' OR ProductDescription LIKE '%{0}%'", searchKeyword);
                (productInfoDataGridView.DataSource as DataTable).DefaultView.RowFilter = filterExpression;
            }
            else
            {
                (productInfoDataGridView.DataSource as DataTable).DefaultView.RowFilter = string.Empty;
            }
        }

        private void addNewCompanyButton_Click(object sender, EventArgs e)
        {
            addNewCompanyForm addNewCompanyForm = new addNewCompanyForm();
            addNewCompanyForm.ShowDialog();
            fillCompanyComboBox();

        }

        private void modelComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (modelComboBox.SelectedIndex == -1)
            {
                modelComboBox.Text = "---------------------Select-----------------";
            }
        }
    }
}
