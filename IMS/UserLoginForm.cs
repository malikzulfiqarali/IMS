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

namespace IMS
{
    public partial class UserLoginForm : Form
    {
        public UserLoginForm()
        {
            InitializeComponent();
        }

        private void UserLoginForm_Load(object sender, EventArgs e)
        {
       

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString);
            SqlCommand cmd = new SqlCommand("SP_GetUserDetail", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserName", UserNameTextBox.Text);
            cmd.Parameters.AddWithValue("@Password", PasswordTextBox.Text);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                //MessageBox.Show("Login Successfull","Success",MessageBoxButtons.OK,MessageBoxIcon.Information);
                this.Hide();
                DashBoard dashBoard = new DashBoard();
                dashBoard.ShowDialog();
            }
            else
            {
                MessageBox.Show("Login Unsuccessfull Please try again with correct credientials", "Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                UserNameTextBox.Focus();
            }
                
            
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
