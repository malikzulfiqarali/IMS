namespace IMS
{
    partial class FetchCompany
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
            this.searchTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.companyInfoDataGridView = new System.Windows.Forms.DataGridView();
            this.companySearchTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.companyInfoDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // searchTextBox
            // 
            this.searchTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.searchTextBox.Location = new System.Drawing.Point(237, -89);
            this.searchTextBox.Name = "searchTextBox";
            this.searchTextBox.Size = new System.Drawing.Size(348, 29);
            this.searchTextBox.TabIndex = 2;
            this.searchTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(134, -89);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 24);
            this.label1.TabIndex = 3;
            this.label1.Text = "Search";
            // 
            // companyInfoDataGridView
            // 
            this.companyInfoDataGridView.AllowUserToAddRows = false;
            this.companyInfoDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.companyInfoDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.companyInfoDataGridView.Location = new System.Drawing.Point(17, 58);
            this.companyInfoDataGridView.Name = "companyInfoDataGridView";
            this.companyInfoDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.companyInfoDataGridView.Size = new System.Drawing.Size(664, 589);
            this.companyInfoDataGridView.TabIndex = 4;
            this.companyInfoDataGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.companyInfoDataGridView_CellDoubleClick);
            this.companyInfoDataGridView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.companyInfoDataGridView_KeyDown);
            // 
            // companySearchTextBox
            // 
            this.companySearchTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.companySearchTextBox.Location = new System.Drawing.Point(202, 12);
            this.companySearchTextBox.Name = "companySearchTextBox";
            this.companySearchTextBox.Size = new System.Drawing.Size(348, 29);
            this.companySearchTextBox.TabIndex = 0;
            this.companySearchTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.companySearchTextBox.TextChanged += new System.EventHandler(this.companySearchTextBox_TextChanged);
            this.companySearchTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.companySearchTextBox_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(99, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 24);
            this.label2.TabIndex = 6;
            this.label2.Text = "Search";
            // 
            // FetchCompany
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(699, 659);
            this.Controls.Add(this.companySearchTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.searchTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.companyInfoDataGridView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FetchCompany";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FetchCompany";
            this.Load += new System.EventHandler(this.FetchCompany_Load);
            ((System.ComponentModel.ISupportInitialize)(this.companyInfoDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox searchTextBox;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.DataGridView companyInfoDataGridView;
        private System.Windows.Forms.TextBox companySearchTextBox;
        private System.Windows.Forms.Label label2;
    }
}