namespace IMS
{
    partial class FetchCustomers
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.CustomerSearchTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.customerInfoDataGridView = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.customerInfoDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // CustomerSearchTextBox
            // 
            this.CustomerSearchTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CustomerSearchTextBox.Location = new System.Drawing.Point(237, 19);
            this.CustomerSearchTextBox.Name = "CustomerSearchTextBox";
            this.CustomerSearchTextBox.Size = new System.Drawing.Size(348, 29);
            this.CustomerSearchTextBox.TabIndex = 2;
            this.CustomerSearchTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.CustomerSearchTextBox.TextChanged += new System.EventHandler(this.CustomerSearchTextBox_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(134, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 24);
            this.label1.TabIndex = 3;
            this.label1.Text = "Search";
            // 
            // customerInfoDataGridView
            // 
            this.customerInfoDataGridView.AllowUserToAddRows = false;
            this.customerInfoDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.customerInfoDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.customerInfoDataGridView.Location = new System.Drawing.Point(68, 58);
            this.customerInfoDataGridView.Name = "customerInfoDataGridView";
            this.customerInfoDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.customerInfoDataGridView.Size = new System.Drawing.Size(664, 589);
            this.customerInfoDataGridView.TabIndex = 4;
            this.customerInfoDataGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.customerInfoDataGridView_CellDoubleClick);
            this.customerInfoDataGridView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.customerInfoDataGridView_KeyDown);
            // 
            // FetchCustomers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 666);
            this.Controls.Add(this.CustomerSearchTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.customerInfoDataGridView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FetchCustomers";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Fetch Customers";
            this.Load += new System.EventHandler(this.FetchCustomers_Load);
            ((System.ComponentModel.ISupportInitialize)(this.customerInfoDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox CustomerSearchTextBox;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.DataGridView customerInfoDataGridView;
    }
}