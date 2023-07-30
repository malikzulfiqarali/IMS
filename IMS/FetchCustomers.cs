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
    public partial class FetchCustomers : Form
    {
        public static int SetCode { get; set; }
        public static string SetName { get; set; }
        public static string SetFatherName { get; set; }
        public static string SetCNIC { get; set; }
        public static string SetMobile { get; set; }
        public static string SetAddress { get; set; }
        public FetchCustomers()
        {
            InitializeComponent();
        }

        private void FetchCustomers_Load(object sender, EventArgs e)
        {
            using (SqlConnection connection=new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
            {
                using (SqlDataAdapter da=new SqlDataAdapter("[SP_CUSTOMER_INFORMATION]", connection))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    customerInfoDataGridView.DataSource = dt;
                }
                CustomerSearchTextBox.Focus();
            }

        }

        private void CustomerSearchTextBox_TextChanged(object sender, EventArgs e)
        {
            string searchKeyWord = CustomerSearchTextBox.Text.Trim();
            if (!string.IsNullOrWhiteSpace(searchKeyWord))
            {
                string filterExpression = string.Format("Party LIKE '%{0}%'OR Father LIKE '%{0}%' OR CNIC LIKE '%{0}%' OR Mobile LIKE '%{0}%' OR Address LIKE '%{0}%'", searchKeyWord);
                (customerInfoDataGridView.DataSource as DataTable).DefaultView.RowFilter = filterExpression;
            }
            else
            {
                (customerInfoDataGridView.DataSource as DataTable).DefaultView.RowFilter = string.Empty;

            }
        }

        private void customerInfoDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            SetCode = Convert.ToInt32(customerInfoDataGridView.CurrentRow.Cells["Code"].Value);
            SetName = customerInfoDataGridView.CurrentRow.Cells["Party"].Value.ToString();
            SetFatherName = customerInfoDataGridView.CurrentRow.Cells["Father"].Value.ToString();
            SetCNIC = customerInfoDataGridView.CurrentRow.Cells["CNIC"].Value.ToString();
            SetMobile = customerInfoDataGridView.CurrentRow.Cells["Mobile"].Value.ToString();
            SetAddress = customerInfoDataGridView.CurrentRow.Cells["Address"].Value.ToString();
            this.Hide();
        }

        private void customerInfoDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Get the current cursor position row index
                int rowIndex = customerInfoDataGridView.CurrentCell.RowIndex;

                // Get the data from the selected row
                SetCode = Convert.ToInt32(customerInfoDataGridView.CurrentRow.Cells["Code"].Value);
                SetName = customerInfoDataGridView.CurrentRow.Cells["Party"].Value.ToString();
                SetFatherName = customerInfoDataGridView.CurrentRow.Cells["Father"].Value.ToString();
                SetCNIC = customerInfoDataGridView.CurrentRow.Cells["CNIC"].Value.ToString();
                SetMobile = customerInfoDataGridView.CurrentRow.Cells["Mobile"].Value.ToString();
                SetAddress = customerInfoDataGridView.CurrentRow.Cells["Address"].Value.ToString();
                // ...

                // Do something with the row data, for example, display it in a message box
                //MessageBox.Show($"Selected row data: {value1}, {value2}, ...");

                // Prevent the Enter key from being processed further
                e.Handled = true;
                this.Hide();
            }
        }

        private void CustomerSearchTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetterOrDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
