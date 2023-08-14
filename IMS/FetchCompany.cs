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
    public partial class FetchCompany : Form
    {
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString);
        public static int SetCode { get; set; }
        public static string SetName { get; set; }
        public FetchCompany()
        {
            InitializeComponent();
        }

        private void FetchCompany_Load(object sender, EventArgs e)
        {
            
            try
            {
                connection.Open();
                SqlDataAdapter da = new SqlDataAdapter("[SP_FETCH_VENDOR_INFORMATION]", connection);
                DataTable dt = new DataTable();
                da.Fill(dt);
                companyInfoDataGridView.DataSource = dt;
                
                connection.Close();
                


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                connection.Close();
            }
            companySearchTextBox.Focus();

        }

        private void companySearchTextBox_TextChanged(object sender, EventArgs e)
        {
            string searchKeyWord = companySearchTextBox.Text.Trim();
            if (!string.IsNullOrWhiteSpace(searchKeyWord))
            {
                string filterExpression = string.Format("Party LIKE '%{0}%'", searchKeyWord);
                (companyInfoDataGridView.DataSource as DataTable).DefaultView.RowFilter = filterExpression;
            }
            else
            {
                (companyInfoDataGridView.DataSource as DataTable).DefaultView.RowFilter = string.Empty;

            }
        }

        private void companySearchTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetterOrDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void companyInfoDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int rowIndex = companyInfoDataGridView.CurrentCell.RowIndex;
                SetCode = Convert.ToInt32(companyInfoDataGridView.Rows[rowIndex].Cells[0].Value);
                SetName = companyInfoDataGridView.Rows[rowIndex].Cells[1].Value.ToString();
                e.Handled = true;
                this.Hide();
            }
        }

        private void companyInfoDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            SetCode = Convert.ToInt32(companyInfoDataGridView.CurrentRow.Cells["Code"].Value);
            SetName = companyInfoDataGridView.CurrentRow.Cells["Party"].Value.ToString();
            this.Hide();
        }
    }
}
