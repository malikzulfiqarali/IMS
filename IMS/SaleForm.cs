﻿using System;
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
    public partial class SaleForm : Form
    {
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString);
        public SaleForm()
        {
            InitializeComponent();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void productIdTextBox_KeyUp(object sender, KeyEventArgs e)
        {
          
        }

        private void customerCodeTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                FetchCustomers fetchCustomers = new FetchCustomers();
                fetchCustomers.ShowDialog();
            }
        }

        private void customerCodeTextBox_Leave(object sender, EventArgs e)
        {
            customerCodeTextBox.Text = FetchCustomers.SetCode.ToString();
            customerNameTextBox.Text = FetchCustomers.SetName;
            fatherTextBox.Text = FetchCustomers.SetFatherName;
            cnicTextBox.Text = FetchCustomers.SetCNIC;
            mobileTextBox.Text = FetchCustomers.SetMobile;
            addressTextBox.Text = FetchCustomers.SetAddress;
        }
        private void ClearAllData()
        {
            foreach (TextBox textBox in this.Controls.OfType<TextBox>())
            {
                textBox.Clear();
            }
            foreach (ComboBox comboBox in this.Controls.OfType<ComboBox>())
            {
                comboBox.SelectedIndex = -1;
            }
            productDataGridView.Rows.Clear();
        }

        private void addNewButton_Click(object sender, EventArgs e)
        {
            ClearAllData();
            GetMaxNumber();
            AddButtonEnabled();
        }
        private void GetMaxNumber()
        {
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter("select distinct max(SaleCode) from Sale",connection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            int maxNumber = Convert.ToInt32(dt.Rows[0][0]);
            int newNumber = maxNumber + 1;
            saleNumberTextBox.Text = newNumber.ToString();
            connection.Close();
        }

        private void SaleForm_Load(object sender, EventArgs e)
        {
            addButton.Enabled = false; 
            ClearAllData();
            GetMaxNumber();
        }

        private void productIdTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                FetchProduct fetchProduct = new FetchProduct();
                fetchProduct.ShowDialog();

            }
        }

        private void productIdTextBox_DoubleClick(object sender, EventArgs e)
        {
            FetchProduct fetchProduct = new FetchProduct();
            fetchProduct.ShowDialog();
        }

        private void productIdTextBox_Leave(object sender, EventArgs e)
        {
            productIdTextBox.Text = FetchProduct.SetCode.ToString();
            productTextBox.Text = FetchProduct.SetDescription;
            currentStockTextBox.Text = FetchProduct.SetQuantity.ToString();
            priceTextBox.Text = FetchProduct.SetPrice.ToString();
        }

        private void saleQtyTextBox_Enter(object sender, EventArgs e)
        {
        }

        private void saleQtyTextBox_Leave(object sender, EventArgs e)
        {
            if (saleQtyTextBox.Text.Trim()!=string.Empty && priceTextBox.Text.Trim()!=string.Empty)
            {
                totalAmountTextBox.Text = (Convert.ToDecimal(saleQtyTextBox.Text.Trim()) * Convert.ToDecimal(priceTextBox.Text.Trim())).ToString(); 
            }

        }

        private void priceTextBox_Leave(object sender, EventArgs e)
        {
            if (saleQtyTextBox.Text.Trim() != string.Empty && priceTextBox.Text.Trim() != string.Empty )
            {
                totalAmountTextBox.Text = (Convert.ToDecimal (saleQtyTextBox.Text.Trim()) * Convert.ToDecimal(priceTextBox.Text.Trim())).ToString(); 
            }

        }

        private void addButton_Click(object sender, EventArgs e)
        {

            if (Convert.ToInt32( currentStockTextBox.Text.Trim()) >=Convert.ToInt32( saleQtyTextBox.Text.Trim()))
            {
                productDataGridView.Rows.Add(productIdTextBox.Text.Trim(), productTextBox.Text.Trim(), currentStockTextBox.Text.Trim(), saleQtyTextBox.Text.Trim(), priceTextBox.Text.Trim(), totalAmountTextBox.Text.Trim());
                productIdTextBox.Text = string.Empty;
                productTextBox.Text = string.Empty;
                currentStockTextBox.Text = string.Empty;
                saleQtyTextBox.Text = string.Empty;
                saleQtyTextBox.BackColor = Color.White;
                priceTextBox.Text = string.Empty;
                totalAmountTextBox.Text = string.Empty;
                totalTextBox.Text = GetTotal().ToString();
                AdvanceAmountAndBalaneAmountCalculation();
                InstallmentCalculation(); 
            }
            else
            {
                MessageBox.Show("Sale Quantity cannot be greater than Current Stock","Information",MessageBoxButtons.OK,MessageBoxIcon.Information);
                saleQtyTextBox.BackColor = Color.Aqua;
                saleQtyTextBox.Focus();
            }
           
            
            

        }
        private decimal GetTotal()
        {
            decimal Total = 0;
            foreach (DataGridViewRow row in productDataGridView.Rows)
            {
              Total += Convert.ToDecimal( row.Cells["Amount"].Value);
            }
            return Math.Round( Total,0);
        }

        private void productDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (productDataGridView.Columns[e.ColumnIndex].HeaderText == "Remove")
                {
                    productDataGridView.Rows.RemoveAt(e.RowIndex);
                    if (e.RowIndex > 0)
                    {
                        totalTextBox.Text = GetTotal().ToString();
                        AdvanceAmountAndBalaneAmountCalculation();
                        InstallmentCalculation();

                    }
                    else
                    {
                        totalTextBox.Text = string.Empty;
                        advanceTextBox.Text = string.Empty;
                        monthTextBox.Text = string.Empty;
                        balanceTextBox.Text = string.Empty;
                        installmentTextBox.Text = string.Empty;
                    }
                }
                
            }
            catch (Exception ex)
            {

                MessageBox.Show("Please remove this field from remove button" + ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
           
        }

        private void advanceTextBox_Leave(object sender, EventArgs e)
        {
            AdvanceAmountAndBalaneAmountCalculation();
        }

        private void AdvanceAmountAndBalaneAmountCalculation()
        {
            if (totalTextBox.Text.Trim() != string.Empty && advanceTextBox.Text.Trim() != string.Empty)
            {
                balanceTextBox.Text = (Convert.ToDecimal(totalTextBox.Text.Trim()) - Convert.ToDecimal(advanceTextBox.Text.Trim())).ToString();
            }
        }

        private void monthTextBox_Leave(object sender, EventArgs e)
        {
            InstallmentCalculation();
        }

        private void InstallmentCalculation()
        {
            if (balanceTextBox.Text != string.Empty)
            {
                installmentTextBox.Text = (Math.Round((Convert.ToDecimal(balanceTextBox.Text.Trim())) / Convert.ToDecimal(monthTextBox.Text.Trim()),0)).ToString();
            }
        }

        private void productIdTextBox_TextChanged(object sender, EventArgs e)
        {
            CheckTextBoxes();

        }

        private void AddButtonEnabled()
        {
            //if (productIdTextBox.Text.Trim() != String.Empty && saleQtyTextBox.Text.Trim() != string.Empty && priceTextBox.Text.Trim() != string.Empty && totalAmountTextBox.Text.Trim() != string.Empty)
            //{
            //    addButton.Enabled = true;
            //}
        }

        private void totalAmountTextBox_TextChanged(object sender, EventArgs e)
        {
            CheckTextBoxes();
        }
        private void CheckTextBoxes()
        {
            if (!string.IsNullOrWhiteSpace(productIdTextBox.Text) && !string.IsNullOrWhiteSpace(saleQtyTextBox.Text) && !string.IsNullOrWhiteSpace(priceTextBox.Text) && !string.IsNullOrWhiteSpace(productIdTextBox.Text) && !string.IsNullOrWhiteSpace(totalAmountTextBox.Text) && !string.IsNullOrEmpty(customerCodeTextBox.Text) && !string.IsNullOrWhiteSpace(customerNameTextBox.Text))
            {
                addButton.Enabled = true;
            }
            else
            {
                addButton.Enabled = false;
            }
        }

        private void saleQtyTextBox_TextChanged(object sender, EventArgs e)
        {
            CheckTextBoxes();
        }

        private void priceTextBox_TextChanged(object sender, EventArgs e)
        {
            CheckTextBoxes();
        }

        private void customerCodeTextBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            FetchCustomers fetchCustomers = new FetchCustomers();
            fetchCustomers.ShowDialog();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            SqlTransaction transaction = null;
            if (string.IsNullOrWhiteSpace(saleNumberTextBox.Text))
            {
                MessageBox.Show("Sale Invoice Number must not be Empty","Information",MessageBoxButtons.OK,MessageBoxIcon.Information);
                saleNumberTextBox.Focus();
                return;
            }
            if (saleCategoryComboBox.SelectedIndex==-1)
            {
                MessageBox.Show("Sale Category must be selected", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                saleCategoryComboBox.Focus();
                return;

            }
            if (string.IsNullOrEmpty(customerCodeTextBox.Text))
            {
                MessageBox.Show("Customer Code must be entered", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                customerCodeTextBox.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(customerNameTextBox.Text))
            {
                MessageBox.Show("Please Enter Customer Name", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                customerNameTextBox.Focus();
                return; 
            }
            if (string.IsNullOrWhiteSpace(mobileTextBox.Text))
            {
                MessageBox.Show("Please Enter Customer Mobile Number", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                mobileTextBox.Focus();
                return;
            }
            if (productDataGridView.Rows.Count==0)
            {
                MessageBox.Show("Please enter any Product for further Processing", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                productIdTextBox.Focus();
                return;
            }
            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();
                
                foreach (DataGridViewRow row in productDataGridView.Rows)
                {
                    SqlCommand cmd = new SqlCommand("SP_INSERT_SALE", connection,transaction);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SaleCode", saleNumberTextBox.Text.Trim());
                    cmd.Parameters.AddWithValue("@Date", invoiceDateTimePicker.Value);
                    cmd.Parameters.AddWithValue("@CustomerID", Convert.ToInt32( customerCodeTextBox.Text.Trim()));
                    cmd.Parameters.AddWithValue("@ProductID", Convert.ToInt32( row.Cells["PID"].Value)); 
                    cmd.Parameters.AddWithValue("@SaleCategory", saleCategoryComboBox.SelectedItem); 
                    cmd.Parameters.AddWithValue("@Qty", Convert.ToInt32( row.Cells["SaleQty"].Value)); 
                    cmd.Parameters.AddWithValue("@Price", Convert.ToDecimal( row.Cells["Rate"].Value)); 
                    cmd.Parameters.AddWithValue("@SaleAmount", Convert.ToDecimal( row.Cells["Amount"].Value)); 
                    cmd.Parameters.AddWithValue("@TotalInvoiceAmount", Convert.ToDecimal( totalTextBox.Text.Trim())); 
                    cmd.Parameters.AddWithValue("@Advance",   advanceTextBox.Text.Trim()==string.Empty?(object)DBNull.Value : Convert.ToDecimal (advanceTextBox.Text.Trim())); 
                    cmd.Parameters.AddWithValue("@BalanceAmount",  balanceTextBox.Text.Trim()==string.Empty?(object)DBNull.Value: Convert.ToDecimal ( balanceTextBox.Text.Trim())); 
                    cmd.Parameters.AddWithValue("@Months",    monthTextBox.Text.Trim()==string.Empty?(object)DBNull.Value: Convert.ToInt32( monthTextBox.Text.Trim())); 
                    cmd.Parameters.AddWithValue("@InstallmentAmount", installmentTextBox.Text.Trim()==string.Empty?(object)DBNull.Value:Convert.ToDecimal( installmentTextBox.Text.Trim()));
                    cmd.ExecuteNonQuery();
                }
                string voucherCatCode = saleNumber.Text.Trim() + " " + saleNumberTextBox.Text.Trim();
                SqlCommand cmd1 = new SqlCommand("[SP_INSERT_SALE_VOUCHER]", connection,transaction);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@VoucherCode",Convert.ToInt32( saleNumberTextBox.Text.Trim()));
                cmd1.Parameters.AddWithValue("@VoucherType", saleNumber.Text.Trim());
                cmd1.Parameters.AddWithValue("@VoucherDate", invoiceDateTimePicker.Value);
                cmd1.Parameters.AddWithValue("@VoucherCategoryID", Convert.ToInt32( customerCodeTextBox.Text.Trim()));
                cmd1.Parameters.AddWithValue("@VoucherCategory", saleCategoryComboBox.SelectedItem);
                cmd1.Parameters.AddWithValue("@VoucherCategoryCode",  voucherCatCode);
                cmd1.Parameters.AddWithValue("@Description", customerNameTextBox.Text.Trim());
                cmd1.Parameters.AddWithValue("@Debit", totalTextBox.Text.Trim()==string.Empty?(object)DBNull.Value: Convert.ToDecimal( totalTextBox.Text.Trim()));
                cmd1.ExecuteNonQuery();
                transaction.Commit();
                MessageBox.Show("The Transaction is successfull","Sucess",MessageBoxButtons.OK,MessageBoxIcon.Information);
                connection.Close();
                addNewButton.PerformClick();
            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.Message,"Failure",MessageBoxButtons.OK,MessageBoxIcon.Error);
                transaction.Rollback();
            }
            finally
            {
                connection.Close();
            }


        }

        private void customerCodeTextBox_TextChanged(object sender, EventArgs e)
        {
            CheckTextBoxes();
        }

        private void customerNameTextBox_TextChanged(object sender, EventArgs e)
        {
            CheckTextBoxes();
        }

        private void saleCategoryComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (saleCategoryComboBox.SelectedIndex==0)
            {
                advanceTextBox.ReadOnly=true;
                monthTextBox.ReadOnly=true;

            }
            if(saleCategoryComboBox.SelectedIndex==1)
            {
                advanceTextBox.ReadOnly = true;
                monthTextBox.ReadOnly = true;
            }
            if (saleCategoryComboBox.SelectedIndex==2)
            {
                advanceTextBox.ReadOnly = false;
                monthTextBox.ReadOnly = false;
            }
        }
    }
}
