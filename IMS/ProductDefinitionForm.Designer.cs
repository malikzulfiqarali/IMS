namespace IMS
{
    partial class ProductDefinitionForm
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
            this.productInfoDataGridView = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.searchTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.productDescriptionTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.quantityTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.salePriceTextBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.fixedPriceTextBox = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.retailPriceTextBox = new System.Windows.Forms.TextBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.clearButton = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.productCodeTextBox = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.productDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.label12 = new System.Windows.Forms.Label();
            this.categoryComboBox = new System.Windows.Forms.ComboBox();
            this.productIDTextBox = new System.Windows.Forms.TextBox();
            this.companyComboBox = new System.Windows.Forms.ComboBox();
            this.costPriceTextBox = new System.Windows.Forms.TextBox();
            this.addNewCompanyButton = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.modelComboBox = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.productInfoDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // productInfoDataGridView
            // 
            this.productInfoDataGridView.AllowUserToAddRows = false;
            this.productInfoDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.productInfoDataGridView.Location = new System.Drawing.Point(515, 90);
            this.productInfoDataGridView.Name = "productInfoDataGridView";
            this.productInfoDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.productInfoDataGridView.Size = new System.Drawing.Size(625, 619);
            this.productInfoDataGridView.TabIndex = 17;
            this.productInfoDataGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.productInfoDataGridView_CellDoubleClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(512, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 24);
            this.label1.TabIndex = 1;
            this.label1.Text = "Search";
            // 
            // searchTextBox
            // 
            this.searchTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.searchTextBox.Location = new System.Drawing.Point(600, 29);
            this.searchTextBox.Name = "searchTextBox";
            this.searchTextBox.Size = new System.Drawing.Size(435, 29);
            this.searchTextBox.TabIndex = 16;
            this.searchTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.searchTextBox.TextChanged += new System.EventHandler(this.searchTextBox_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(9, 138);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(138, 24);
            this.label2.TabIndex = 3;
            this.label2.Text = "Product Code";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 188);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(193, 24);
            this.label3.TabIndex = 3;
            this.label3.Text = "Product Description";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // productDescriptionTextBox
            // 
            this.productDescriptionTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.productDescriptionTextBox.Location = new System.Drawing.Point(211, 185);
            this.productDescriptionTextBox.Name = "productDescriptionTextBox";
            this.productDescriptionTextBox.Size = new System.Drawing.Size(250, 29);
            this.productDescriptionTextBox.TabIndex = 3;
            this.productDescriptionTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.productDescriptionTextBox.TextChanged += new System.EventHandler(this.productDescriptionTextBox_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(241, 239);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(98, 24);
            this.label4.TabIndex = 3;
            this.label4.Text = "Company";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(31, 244);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(86, 24);
            this.label5.TabIndex = 3;
            this.label5.Text = "Quantity";
            // 
            // quantityTextBox
            // 
            this.quantityTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.quantityTextBox.Location = new System.Drawing.Point(153, 239);
            this.quantityTextBox.Name = "quantityTextBox";
            this.quantityTextBox.Size = new System.Drawing.Size(80, 29);
            this.quantityTextBox.TabIndex = 4;
            this.quantityTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.quantityTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.quantityTextBox_KeyPress);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(31, 361);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(105, 24);
            this.label6.TabIndex = 3;
            this.label6.Text = "Cost Price";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(31, 419);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(105, 24);
            this.label7.TabIndex = 3;
            this.label7.Text = "Sale Price";
            // 
            // salePriceTextBox
            // 
            this.salePriceTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.salePriceTextBox.Location = new System.Drawing.Point(153, 416);
            this.salePriceTextBox.Name = "salePriceTextBox";
            this.salePriceTextBox.Size = new System.Drawing.Size(308, 29);
            this.salePriceTextBox.TabIndex = 9;
            this.salePriceTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.salePriceTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.salePriceTextBox_KeyPress);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(31, 473);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(117, 24);
            this.label8.TabIndex = 3;
            this.label8.Text = "Fixed Price";
            // 
            // fixedPriceTextBox
            // 
            this.fixedPriceTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fixedPriceTextBox.Location = new System.Drawing.Point(153, 470);
            this.fixedPriceTextBox.Name = "fixedPriceTextBox";
            this.fixedPriceTextBox.Size = new System.Drawing.Size(308, 29);
            this.fixedPriceTextBox.TabIndex = 10;
            this.fixedPriceTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.fixedPriceTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.fixedPriceTextBox_KeyPress);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(31, 525);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(116, 24);
            this.label9.TabIndex = 3;
            this.label9.Text = "Retail Price";
            // 
            // retailPriceTextBox
            // 
            this.retailPriceTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.retailPriceTextBox.Location = new System.Drawing.Point(153, 522);
            this.retailPriceTextBox.Name = "retailPriceTextBox";
            this.retailPriceTextBox.Size = new System.Drawing.Size(308, 29);
            this.retailPriceTextBox.TabIndex = 11;
            this.retailPriceTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.retailPriceTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.retailPriceTextBox_KeyPress);
            // 
            // saveButton
            // 
            this.saveButton.BackColor = System.Drawing.Color.LimeGreen;
            this.saveButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.saveButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.saveButton.Location = new System.Drawing.Point(32, 645);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(141, 48);
            this.saveButton.TabIndex = 13;
            this.saveButton.Text = "&Save";
            this.saveButton.UseVisualStyleBackColor = false;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // clearButton
            // 
            this.clearButton.BackColor = System.Drawing.Color.MediumSpringGreen;
            this.clearButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.clearButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clearButton.Location = new System.Drawing.Point(195, 645);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(141, 48);
            this.clearButton.TabIndex = 14;
            this.clearButton.Text = "&Clear";
            this.clearButton.UseVisualStyleBackColor = false;
            this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
            // 
            // closeButton
            // 
            this.closeButton.BackColor = System.Drawing.Color.RoyalBlue;
            this.closeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.closeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.closeButton.Location = new System.Drawing.Point(355, 645);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(141, 48);
            this.closeButton.TabIndex = 15;
            this.closeButton.Text = "&Close";
            this.closeButton.UseVisualStyleBackColor = false;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(29, 90);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(107, 24);
            this.label10.TabIndex = 3;
            this.label10.Text = "Product ID";
            this.label10.Click += new System.EventHandler(this.label10_Click);
            // 
            // productCodeTextBox
            // 
            this.productCodeTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.productCodeTextBox.Location = new System.Drawing.Point(153, 138);
            this.productCodeTextBox.Name = "productCodeTextBox";
            this.productCodeTextBox.Size = new System.Drawing.Size(308, 29);
            this.productCodeTextBox.TabIndex = 2;
            this.productCodeTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.productCodeTextBox.TextChanged += new System.EventHandler(this.textBox9_TextChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(31, 29);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(52, 24);
            this.label11.TabIndex = 3;
            this.label11.Text = "Date";
            // 
            // productDateTimePicker
            // 
            this.productDateTimePicker.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.productDateTimePicker.Location = new System.Drawing.Point(153, 29);
            this.productDateTimePicker.Name = "productDateTimePicker";
            this.productDateTimePicker.Size = new System.Drawing.Size(317, 29);
            this.productDateTimePicker.TabIndex = 0;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(49, 316);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(68, 24);
            this.label12.TabIndex = 3;
            this.label12.Text = "Model";
            // 
            // categoryComboBox
            // 
            this.categoryComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.categoryComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.categoryComboBox.BackColor = System.Drawing.Color.Aqua;
            this.categoryComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.categoryComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.categoryComboBox.FormattingEnabled = true;
            this.categoryComboBox.Location = new System.Drawing.Point(153, 316);
            this.categoryComboBox.Name = "categoryComboBox";
            this.categoryComboBox.Size = new System.Drawing.Size(308, 32);
            this.categoryComboBox.TabIndex = 7;
            this.categoryComboBox.Text = "   -----------------Select-----------------";
            this.categoryComboBox.SelectedIndexChanged += new System.EventHandler(this.categoryComboBox_SelectedIndexChanged);
            // 
            // productIDTextBox
            // 
            this.productIDTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.productIDTextBox.Location = new System.Drawing.Point(153, 90);
            this.productIDTextBox.Name = "productIDTextBox";
            this.productIDTextBox.Size = new System.Drawing.Size(89, 29);
            this.productIDTextBox.TabIndex = 1;
            this.productIDTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.productIDTextBox.TextChanged += new System.EventHandler(this.productIDTextBox_TextChanged);
            // 
            // companyComboBox
            // 
            this.companyComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.companyComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.companyComboBox.BackColor = System.Drawing.Color.Aqua;
            this.companyComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.companyComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.companyComboBox.FormattingEnabled = true;
            this.companyComboBox.Location = new System.Drawing.Point(345, 236);
            this.companyComboBox.Name = "companyComboBox";
            this.companyComboBox.Size = new System.Drawing.Size(136, 32);
            this.companyComboBox.TabIndex = 5;
            this.companyComboBox.Text = "  ---Select---";
            this.companyComboBox.SelectedIndexChanged += new System.EventHandler(this.companyComboBox_SelectedIndexChanged);
            // 
            // costPriceTextBox
            // 
            this.costPriceTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.costPriceTextBox.Location = new System.Drawing.Point(153, 361);
            this.costPriceTextBox.Name = "costPriceTextBox";
            this.costPriceTextBox.Size = new System.Drawing.Size(308, 29);
            this.costPriceTextBox.TabIndex = 8;
            this.costPriceTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.costPriceTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.costPriceTextBox_KeyPress);
            // 
            // addNewCompanyButton
            // 
            this.addNewCompanyButton.BackColor = System.Drawing.Color.DarkOrange;
            this.addNewCompanyButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.addNewCompanyButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addNewCompanyButton.Location = new System.Drawing.Point(114, 575);
            this.addNewCompanyButton.Name = "addNewCompanyButton";
            this.addNewCompanyButton.Size = new System.Drawing.Size(306, 48);
            this.addNewCompanyButton.TabIndex = 12;
            this.addNewCompanyButton.Text = "&Add New Company";
            this.addNewCompanyButton.UseVisualStyleBackColor = false;
            this.addNewCompanyButton.Click += new System.EventHandler(this.addNewCompanyButton_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(43, 274);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(93, 24);
            this.label13.TabIndex = 3;
            this.label13.Text = "Category";
            // 
            // modelComboBox
            // 
            this.modelComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.modelComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.modelComboBox.BackColor = System.Drawing.Color.Aqua;
            this.modelComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.modelComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.modelComboBox.FormattingEnabled = true;
            this.modelComboBox.Location = new System.Drawing.Point(153, 274);
            this.modelComboBox.Name = "modelComboBox";
            this.modelComboBox.Size = new System.Drawing.Size(308, 32);
            this.modelComboBox.TabIndex = 6;
            this.modelComboBox.Text = "   -----------------Select-----------------";
            this.modelComboBox.SelectedIndexChanged += new System.EventHandler(this.modelComboBox_SelectedIndexChanged);
            // 
            // ProductDefinitionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1161, 721);
            this.Controls.Add(this.companyComboBox);
            this.Controls.Add(this.modelComboBox);
            this.Controls.Add(this.categoryComboBox);
            this.Controls.Add(this.productDateTimePicker);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.addNewCompanyButton);
            this.Controls.Add(this.clearButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.retailPriceTextBox);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.fixedPriceTextBox);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.salePriceTextBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.costPriceTextBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.quantityTextBox);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.productDescriptionTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.productCodeTextBox);
            this.Controls.Add(this.productIDTextBox);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.searchTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.productInfoDataGridView);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProductDefinitionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Products";
            this.Load += new System.EventHandler(this.ProductDefinitionForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.productInfoDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView productInfoDataGridView;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox searchTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox productDescriptionTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox quantityTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox salePriceTextBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox fixedPriceTextBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox retailPriceTextBox;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button clearButton;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox productCodeTextBox;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.DateTimePicker productDateTimePicker;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox categoryComboBox;
        private System.Windows.Forms.TextBox productIDTextBox;
        private System.Windows.Forms.ComboBox companyComboBox;
        private System.Windows.Forms.TextBox costPriceTextBox;
        private System.Windows.Forms.Button addNewCompanyButton;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox modelComboBox;
    }
}