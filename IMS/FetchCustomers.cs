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
    }
}
