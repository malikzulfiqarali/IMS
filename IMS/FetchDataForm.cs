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
    public partial class FetchDataForm : Form
    {
        public static int SetCode { get; set; }
        public static string SetName { get; set; }
  
        public FetchDataForm()
        {
            InitializeComponent();
        }

        private void FetchDataForm_Load(object sender, EventArgs e)
        {
            using (SqlConnection connection=new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
            {
                using (SqlDataAdapter da=new SqlDataAdapter("SP_FETCH_INFORMATION", connection))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    infoDataGridView.DataSource = dt;
                }
            }
            searchTextBox.Focus();
        }

        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            string searchKeyWord = searchTextBox.Text.Trim();
            if (!string.IsNullOrWhiteSpace(searchKeyWord))
            {
                string filterExpression = string.Format("Party LIKE '%{0}%'", searchKeyWord);
                (infoDataGridView.DataSource as DataTable).DefaultView.RowFilter = filterExpression;
            }
            else
            {
                (infoDataGridView.DataSource as DataTable).DefaultView.RowFilter = string.Empty;

            }
            
        }

        private void infoDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            SetCode = Convert.ToInt32(infoDataGridView.CurrentRow.Cells["Code"].Value);
            SetName = infoDataGridView.CurrentRow.Cells["Party"].Value.ToString();
            this.Hide();


        }

        private void infoDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            
                if (e.KeyCode == Keys.Enter)
                {
                    // Get the current cursor position row index
                    int rowIndex = infoDataGridView.CurrentCell.RowIndex;

                    // Get the data from the selected row
                    SetCode =Convert.ToInt32 (infoDataGridView.Rows[rowIndex].Cells[0].Value);
                    SetName = infoDataGridView.Rows[rowIndex].Cells[1].Value.ToString();
                    // ...

                    // Do something with the row data, for example, display it in a message box
                    //MessageBox.Show($"Selected row data: {value1}, {value2}, ...");

                    // Prevent the Enter key from being processed further
                    e.Handled = true;
                    this.Hide();
                }
            

        }

        private void searchTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetterOrDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
