using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IMS
{
    public partial class FetchProduct : Form
    {
        public static int SetCode { get; set; }
        public static string SetDescription { get; set; }
        public static int SetQuantity { get; set; }
        public static decimal SetPrice { get; set; }
        public FetchProduct()
        {
            InitializeComponent();
        }

        private void FetchProduct_Load(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
            {
                using (SqlDataAdapter da = new SqlDataAdapter("[SP_PRODUCT_INFORMATION]", connection))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    productInfoDataGridView.DataSource = dt;
                }
                productSearchTextBox.Focus();
            }
        }

        private void productSearchTextBox_TextChanged(object sender, EventArgs e)
        {
            string searchKeyWord = productSearchTextBox.Text.Trim();
            if (!string.IsNullOrWhiteSpace(searchKeyWord))
            {
                string filterExpression = string.Format("Code LIKE '%{0}%' OR Description LIKE '%{0}%'", searchKeyWord);
                (productInfoDataGridView.DataSource as DataTable).DefaultView.RowFilter = filterExpression;
            }
            else
            {
                (productInfoDataGridView.DataSource as DataTable).DefaultView.RowFilter = string.Empty;

            }
        }

        private void productInfoDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Get the current cursor position row index
                int rowIndex = productInfoDataGridView.CurrentCell.RowIndex;

                // Get the data from the selected row
                SetCode = Convert.ToInt32(productInfoDataGridView.CurrentRow.Cells["Id"].Value);
                SetDescription = productInfoDataGridView.CurrentRow.Cells["Description"].Value.ToString();
                SetQuantity = Convert.ToInt32(productInfoDataGridView.CurrentRow.Cells["Qty"].Value);
                SetPrice = Convert.ToDecimal(productInfoDataGridView.CurrentRow.Cells["Price"].Value);

                // ...

                // Do something with the row data, for example, display it in a message box
                //MessageBox.Show($"Selected row data: {value1}, {value2}, ...");

                // Prevent the Enter key from being processed further
                e.Handled = true;
                this.Hide();
            }
        }

        private void productInfoDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            SetCode = Convert.ToInt32(productInfoDataGridView.CurrentRow.Cells["Id"].Value);
            SetDescription = productInfoDataGridView.CurrentRow.Cells["Description"].Value.ToString();
            SetQuantity = Convert.ToInt32(productInfoDataGridView.CurrentRow.Cells["Qty"].Value);
            SetPrice = Convert.ToDecimal(productInfoDataGridView.CurrentRow.Cells["Price"].Value);
            this.Hide();
        }
    }
}
