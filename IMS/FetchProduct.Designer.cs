namespace IMS
{
    partial class FetchProduct
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
            this.productSearchTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.productInfoDataGridView = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.productInfoDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // productSearchTextBox
            // 
            this.productSearchTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.productSearchTextBox.Location = new System.Drawing.Point(237, 19);
            this.productSearchTextBox.Name = "productSearchTextBox";
            this.productSearchTextBox.Size = new System.Drawing.Size(348, 29);
            this.productSearchTextBox.TabIndex = 5;
            this.productSearchTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.productSearchTextBox.TextChanged += new System.EventHandler(this.productSearchTextBox_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(134, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 24);
            this.label1.TabIndex = 6;
            this.label1.Text = "Search";
            // 
            // productInfoDataGridView
            // 
            this.productInfoDataGridView.AllowUserToAddRows = false;
            this.productInfoDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.productInfoDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.productInfoDataGridView.Location = new System.Drawing.Point(68, 58);
            this.productInfoDataGridView.Name = "productInfoDataGridView";
            this.productInfoDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.productInfoDataGridView.Size = new System.Drawing.Size(664, 589);
            this.productInfoDataGridView.TabIndex = 7;
            this.productInfoDataGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.productInfoDataGridView_CellDoubleClick);
            this.productInfoDataGridView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.productInfoDataGridView_KeyDown);
            // 
            // FetchProduct
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 666);
            this.Controls.Add(this.productSearchTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.productInfoDataGridView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FetchProduct";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Fetch Product";
            this.Load += new System.EventHandler(this.FetchProduct_Load);
            ((System.ComponentModel.ISupportInitialize)(this.productInfoDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox productSearchTextBox;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.DataGridView productInfoDataGridView;
    }
}