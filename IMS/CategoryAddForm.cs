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
    public partial class CategoryAddForm : Form
    {
        public CategoryAddForm()
        {
            InitializeComponent();
        }
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString);

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CategoryAddForm_Load(object sender, EventArgs e)
        {
            fillDataCategoryTypCB();
            loadDataInDataGridView();
            clearAllTextBoxes(this);
            getMaxNumber("select max(CategoryID) from CategoryTable");

        }

        private void categoryTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (categoryTypeComboBox.SelectedIndex == -1)
            {
                categoryTypeComboBox.Text = "------------Select----------";
            }

        }
        private void fillDataCategoryTypCB()
        {
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM CategoryType", connection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            categoryTypeComboBox.DataSource = dt;
            categoryTypeComboBox.ValueMember = "CategoryTypeID";
            categoryTypeComboBox.DisplayMember = "CategoryTypeName";
            connection.Close();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (modelTextBox.Text==string.Empty)
            {
                MessageBox.Show("Model Field can not be empty You must feed something", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (categoryTypeComboBox.SelectedIndex==-1)
            {
                MessageBox.Show("You must select some category type from the List", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                connection.Open();
                SqlDataAdapter da = new SqlDataAdapter("select * from CategoryTable WHERE CategoryID='"+categoryIDTextBox.Text.Trim()+"'",connection);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count>0)
                {
                    if (((DateTime)dt.Rows[0]["Date"]).Date==System.DateTime.Now.Date)
                    {
                        SqlCommand cmd = new SqlCommand("[SP_UPDATE_CATEGORY]", connection);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CategoryID", categoryIDTextBox.Text.Trim());
                        cmd.Parameters.AddWithValue("@CategoryTypeID", categoryTypeComboBox.SelectedValue);
                        cmd.Parameters.AddWithValue("@Category", modelTextBox.Text);
                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Records are updated successfully in Database","Success",MessageBoxButtons.OK,MessageBoxIcon.Information);
                            connection.Close();
                            clearAllTextBoxes(this);
                            saveButton.Text = "Save";
                            loadDataInDataGridView();
                           // getMaxNumber("select max(CategoryID) from CategoryTable");

                        }
                        else
                        {
                            MessageBox.Show("Records are not updated successfully into Database", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        }
                    }
                }
                else
                {
                    SqlCommand cmd = new SqlCommand("[SP_INSERT_CATEGORY]", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Date",System.DateTime.Now);
                    cmd.Parameters.AddWithValue("@CategoryTypeID", Convert.ToInt32(categoryTypeComboBox.SelectedValue));
                    cmd.Parameters.AddWithValue("@Category", modelTextBox.Text.Trim());
                    int result = cmd.ExecuteNonQuery();
                    if (result>=0)
                    {
                        MessageBox.Show("Records are inserted successfully into Database", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        connection.Close();
                        clearAllTextBoxes(this);
                        loadDataInDataGridView();
                        getMaxNumber("select max(CategoryID) from CategoryTable");
                        
                    }
                    else
                    {
                        MessageBox.Show("Records are not inserted successfully in Database", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        public void getMaxNumber(string query)
        {
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter(query, connection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                int maxNumber = Convert.ToInt32(dt.Rows[0][0]);
                int incrementNumber = maxNumber + 1;
                categoryIDTextBox.Text = incrementNumber.ToString();
            }
            else
            {
                MessageBox.Show("Data not found something went wrong","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);

            }
            connection.Close();

        }
        private void clearAllTextBoxes(Form form)
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

        private void categoryInfoDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = categoryInfoDataGridView.Rows[e.RowIndex];

                // Get the value of the cells in the selected row
                catgoryDateTimePicker.Value = (DateTime)(row.Cells["Date"].Value);
                categoryIDTextBox.Text = row.Cells["CategoryID"].Value.ToString();
                categoryTypeComboBox.SelectedValue = row.Cells["CategoryTypeID"].Value.ToString();
                modelTextBox.Text = row.Cells["Category"].Value.ToString();
                saveButton.Text = "Update";
            }
        }
        private void loadDataInDataGridView()
        {
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter("Select * from CategoryTable",connection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            categoryInfoDataGridView.DataSource = dt;
            connection.Close();
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            clearAllTextBoxes(this);
            getMaxNumber("select max(CategoryID) from CategoryTable");
        }

        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            string searchKeyword = searchTextBox.Text;

            if (!string.IsNullOrWhiteSpace(searchKeyword))
            {
                string filterExpression = string.Format("Category LIKE '%{0}%' ", searchKeyword);
                (categoryInfoDataGridView.DataSource as DataTable).DefaultView.RowFilter = filterExpression;
            }
            else
            {
                (categoryInfoDataGridView.DataSource as DataTable).DefaultView.RowFilter = string.Empty;
            }
        }

        private void addNewCatTypeButton_Click(object sender, EventArgs e)
        {
            AddNewCategoryTypeForm addNewCategoryTypeForm = new AddNewCategoryTypeForm();
            addNewCategoryTypeForm.ShowDialog();
            RefreshData();
        }

        private void RefreshData()
        {
            fillDataCategoryTypCB();
            loadDataInDataGridView();
            clearAllTextBoxes(this);
            getMaxNumber("select max(CategoryID) from CategoryTable");
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            RefreshData();
        }
    }
}
