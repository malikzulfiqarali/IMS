using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IMS
{
    public partial class DashBoard : Form
    {
        public DashBoard()
        {
            InitializeComponent();
        }

        private void userLoginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //UserLoginForm frm = new UserLoginForm();
            //frm.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void customerDefinitionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CustomerDefinitionForm customerDefinitionForm = new CustomerDefinitionForm();
            customerDefinitionForm.ShowDialog();
        }

        private void DashBoard_Load(object sender, EventArgs e)
        {
            DashBoard dashBoard = new DashBoard();
            dashBoard.transactionsToolStripMenuItem.Enabled = false;
        }

        private void productDefinitionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProductDefinitionForm productDefinitionForm = new ProductDefinitionForm();
            productDefinitionForm.ShowDialog();
        }

        private void categoryDefinitionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CategoryAddForm categoryAddForm = new CategoryAddForm();
            categoryAddForm.ShowDialog();
        }

        private void definitionToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void employeesDefinitionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EmployeesDetailForm employeesDetailForm = new EmployeesDetailForm();
            employeesDetailForm.ShowDialog();
        }

        private void vendorDefinitionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VendorDetailForm vendorDetailForm = new VendorDetailForm();
            vendorDetailForm.ShowDialog();
        }

        private void chartOfAccountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChartOfAccounts chartOfAccounts = new ChartOfAccounts();
            chartOfAccounts.ShowDialog();
        }

        private void cashReceiptsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CashReceiptVoucher cashReceiptVoucher = new CashReceiptVoucher();
            cashReceiptVoucher.ShowDialog();
        }

        private void journalVouchersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            JournalVoucherForm journalVoucherForm = new JournalVoucherForm();
            journalVoucherForm.ShowDialog();
        }

        private void cashPaymentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CashPaymentVoucher cashPaymentVoucher = new CashPaymentVoucher();
            cashPaymentVoucher.ShowDialog();
        }

        private void salesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaleForm saleForm = new SaleForm();
            saleForm.ShowDialog();
        }

        private void ledgerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewLedger dataGridViewLedger = new DataGridViewLedger();
            dataGridViewLedger.ShowDialog();
        }
    }
}
