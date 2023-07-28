namespace IMS
{
    partial class CashPaymentVoucher
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.nextButton = new System.Windows.Forms.Button();
            this.previousButton = new System.Windows.Forms.Button();
            this.lastButton = new System.Windows.Forms.Button();
            this.categoryComboBox = new System.Windows.Forms.ComboBox();
            this.creditTotalTextBox = new System.Windows.Forms.TextBox();
            this.closeButton = new System.Windows.Forms.Button();
            this.clearButton = new System.Windows.Forms.Button();
            this.updateButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.cashCodeTextBox = new System.Windows.Forms.TextBox();
            this.cpvTextBox = new System.Windows.Forms.TextBox();
            this.cashLabel = new System.Windows.Forms.Label();
            this.firstButton = new System.Windows.Forms.Button();
            this.cpvDataGridView = new System.Windows.Forms.DataGridView();
            this.dateDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.narrationTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cpvLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.crvLabel = new System.Windows.Forms.Label();
            this.crvTextBox = new System.Windows.Forms.TextBox();
            this.Code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PartyCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Remarks = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RemoveColumnButton = new System.Windows.Forms.DataGridViewButtonColumn();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.cpvDataGridView)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // nextButton
            // 
            this.nextButton.Location = new System.Drawing.Point(445, 127);
            this.nextButton.Name = "nextButton";
            this.nextButton.Size = new System.Drawing.Size(54, 29);
            this.nextButton.TabIndex = 31;
            this.nextButton.Text = ">";
            this.nextButton.UseVisualStyleBackColor = true;
            this.nextButton.Click += new System.EventHandler(this.nextButton_Click);
            // 
            // previousButton
            // 
            this.previousButton.Location = new System.Drawing.Point(394, 127);
            this.previousButton.Name = "previousButton";
            this.previousButton.Size = new System.Drawing.Size(54, 29);
            this.previousButton.TabIndex = 30;
            this.previousButton.Text = "<";
            this.previousButton.UseVisualStyleBackColor = true;
            this.previousButton.Click += new System.EventHandler(this.previousButton_Click);
            // 
            // lastButton
            // 
            this.lastButton.Location = new System.Drawing.Point(345, 127);
            this.lastButton.Name = "lastButton";
            this.lastButton.Size = new System.Drawing.Size(54, 29);
            this.lastButton.TabIndex = 28;
            this.lastButton.Text = "<<";
            this.lastButton.UseVisualStyleBackColor = true;
            // 
            // categoryComboBox
            // 
            this.categoryComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.categoryComboBox.FormattingEnabled = true;
            this.categoryComboBox.Items.AddRange(new object[] {
            "Installment Sales",
            "Cash Sales",
            "Credit Sales",
            "Miscsllaneous Payments"});
            this.categoryComboBox.Location = new System.Drawing.Point(836, 182);
            this.categoryComboBox.Name = "categoryComboBox";
            this.categoryComboBox.Size = new System.Drawing.Size(320, 32);
            this.categoryComboBox.TabIndex = 2;
            // 
            // creditTotalTextBox
            // 
            this.creditTotalTextBox.Enabled = false;
            this.creditTotalTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.creditTotalTextBox.Location = new System.Drawing.Point(630, 568);
            this.creditTotalTextBox.Name = "creditTotalTextBox";
            this.creditTotalTextBox.Size = new System.Drawing.Size(100, 29);
            this.creditTotalTextBox.TabIndex = 27;
            this.creditTotalTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // closeButton
            // 
            this.closeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.closeButton.Location = new System.Drawing.Point(475, 584);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(120, 38);
            this.closeButton.TabIndex = 8;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // clearButton
            // 
            this.clearButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clearButton.Location = new System.Drawing.Point(328, 584);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(120, 38);
            this.clearButton.TabIndex = 7;
            this.clearButton.Text = "Add New";
            this.clearButton.UseVisualStyleBackColor = true;
            this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
            // 
            // updateButton
            // 
            this.updateButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.updateButton.Location = new System.Drawing.Point(188, 584);
            this.updateButton.Name = "updateButton";
            this.updateButton.Size = new System.Drawing.Size(120, 38);
            this.updateButton.TabIndex = 6;
            this.updateButton.Text = "Update";
            this.updateButton.UseVisualStyleBackColor = true;
            this.updateButton.Click += new System.EventHandler(this.updateButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.saveButton.Location = new System.Drawing.Point(45, 584);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(120, 38);
            this.saveButton.TabIndex = 5;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // cashCodeTextBox
            // 
            this.cashCodeTextBox.Enabled = false;
            this.cashCodeTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cashCodeTextBox.Location = new System.Drawing.Point(216, 170);
            this.cashCodeTextBox.Name = "cashCodeTextBox";
            this.cashCodeTextBox.ReadOnly = true;
            this.cashCodeTextBox.Size = new System.Drawing.Size(171, 29);
            this.cashCodeTextBox.TabIndex = 12;
            this.cashCodeTextBox.Text = "10002";
            // 
            // cpvTextBox
            // 
            this.cpvTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cpvTextBox.Location = new System.Drawing.Point(216, 127);
            this.cpvTextBox.Name = "cpvTextBox";
            this.cpvTextBox.Size = new System.Drawing.Size(100, 29);
            this.cpvTextBox.TabIndex = 0;
            this.cpvTextBox.TextChanged += new System.EventHandler(this.cpvTextBox_TextChanged);
            this.cpvTextBox.Leave += new System.EventHandler(this.cpvTextBox_Leave);
            // 
            // cashLabel
            // 
            this.cashLabel.AutoSize = true;
            this.cashLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cashLabel.Location = new System.Drawing.Point(49, 173);
            this.cashLabel.Name = "cashLabel";
            this.cashLabel.Size = new System.Drawing.Size(53, 24);
            this.cashLabel.TabIndex = 14;
            this.cashLabel.Text = "Cash";
            // 
            // firstButton
            // 
            this.firstButton.Location = new System.Drawing.Point(496, 127);
            this.firstButton.Name = "firstButton";
            this.firstButton.Size = new System.Drawing.Size(54, 29);
            this.firstButton.TabIndex = 29;
            this.firstButton.Text = ">>";
            this.firstButton.UseVisualStyleBackColor = true;
            // 
            // cpvDataGridView
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.cpvDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.cpvDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.cpvDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Code,
            this.Description,
            this.PartyCode,
            this.Amount,
            this.Remarks,
            this.RemoveColumnButton,
            this.ID});
            this.cpvDataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            this.cpvDataGridView.Location = new System.Drawing.Point(31, 288);
            this.cpvDataGridView.MultiSelect = false;
            this.cpvDataGridView.Name = "cpvDataGridView";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cpvDataGridView.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.cpvDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.cpvDataGridView.Size = new System.Drawing.Size(1114, 274);
            this.cpvDataGridView.TabIndex = 4;
            this.cpvDataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.cpvDataGridView_CellClick);
            this.cpvDataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.cpvDataGridView_CellContentClick);
            this.cpvDataGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.cpvDataGridView_CellDoubleClick);
            this.cpvDataGridView.CellLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.cpvDataGridView_CellLeave);
            this.cpvDataGridView.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.cpvDataGridView_EditingControlShowing);
            this.cpvDataGridView.RowLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.cpvDataGridView_RowLeave);
            this.cpvDataGridView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cpvDataGridView_KeyDown);
            // 
            // dateDateTimePicker
            // 
            this.dateDateTimePicker.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateDateTimePicker.Location = new System.Drawing.Point(836, 142);
            this.dateDateTimePicker.Name = "dateDateTimePicker";
            this.dateDateTimePicker.Size = new System.Drawing.Size(320, 29);
            this.dateDateTimePicker.TabIndex = 18;
            // 
            // narrationTextBox
            // 
            this.narrationTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.narrationTextBox.Location = new System.Drawing.Point(216, 212);
            this.narrationTextBox.Multiline = true;
            this.narrationTextBox.Name = "narrationTextBox";
            this.narrationTextBox.Size = new System.Drawing.Size(514, 54);
            this.narrationTextBox.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(745, 182);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(85, 24);
            this.label5.TabIndex = 17;
            this.label5.Text = "Category";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(773, 142);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 24);
            this.label4.TabIndex = 16;
            this.label4.Text = "Date";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(49, 225);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(86, 24);
            this.label6.TabIndex = 15;
            this.label6.Text = "Narration";
            // 
            // cpvLabel
            // 
            this.cpvLabel.AutoSize = true;
            this.cpvLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cpvLabel.Location = new System.Drawing.Point(49, 132);
            this.cpvLabel.Name = "cpvLabel";
            this.cpvLabel.Size = new System.Drawing.Size(48, 24);
            this.cpvLabel.TabIndex = 19;
            this.cpvLabel.Text = "CPV";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(455, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(355, 37);
            this.label1.TabIndex = 0;
            this.label1.Text = "Cash Payment Voucher";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.RoyalBlue;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1171, 91);
            this.panel1.TabIndex = 13;
            // 
            // crvLabel
            // 
            this.crvLabel.AutoSize = true;
            this.crvLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.crvLabel.Location = new System.Drawing.Point(580, 130);
            this.crvLabel.Name = "crvLabel";
            this.crvLabel.Size = new System.Drawing.Size(49, 24);
            this.crvLabel.TabIndex = 32;
            this.crvLabel.Text = "CRV";
            // 
            // crvTextBox
            // 
            this.crvTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.crvTextBox.Location = new System.Drawing.Point(652, 127);
            this.crvTextBox.Name = "crvTextBox";
            this.crvTextBox.Size = new System.Drawing.Size(100, 29);
            this.crvTextBox.TabIndex = 1;
            // 
            // Code
            // 
            this.Code.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Code.DataPropertyName = "VoucherCategoryID";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Code.DefaultCellStyle = dataGridViewCellStyle2;
            this.Code.HeaderText = "Code";
            this.Code.Name = "Code";
            this.Code.Width = 76;
            // 
            // Description
            // 
            this.Description.DataPropertyName = "Description";
            this.Description.HeaderText = "Description";
            this.Description.Name = "Description";
            this.Description.Width = 345;
            // 
            // PartyCode
            // 
            this.PartyCode.DataPropertyName = "VoucherCategoryCode";
            this.PartyCode.HeaderText = "Party Code";
            this.PartyCode.Name = "PartyCode";
            // 
            // Amount
            // 
            this.Amount.DataPropertyName = "Credit";
            this.Amount.HeaderText = "Amount";
            this.Amount.Name = "Amount";
            // 
            // Remarks
            // 
            this.Remarks.DataPropertyName = "Remarks";
            this.Remarks.HeaderText = "Remarks";
            this.Remarks.Name = "Remarks";
            this.Remarks.Width = 350;
            // 
            // RemoveColumnButton
            // 
            this.RemoveColumnButton.HeaderText = "Romove";
            this.RemoveColumnButton.Name = "RemoveColumnButton";
            this.RemoveColumnButton.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.RemoveColumnButton.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.RemoveColumnButton.Text = "Remove";
            this.RemoveColumnButton.UseColumnTextForButtonValue = true;
            // 
            // ID
            // 
            this.ID.DataPropertyName = "VoucherID";
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            // 
            // CashPaymentVoucher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1171, 628);
            this.Controls.Add(this.crvTextBox);
            this.Controls.Add(this.crvLabel);
            this.Controls.Add(this.nextButton);
            this.Controls.Add(this.previousButton);
            this.Controls.Add(this.lastButton);
            this.Controls.Add(this.categoryComboBox);
            this.Controls.Add(this.creditTotalTextBox);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.clearButton);
            this.Controls.Add(this.updateButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.cashCodeTextBox);
            this.Controls.Add(this.cpvTextBox);
            this.Controls.Add(this.cashLabel);
            this.Controls.Add(this.firstButton);
            this.Controls.Add(this.cpvDataGridView);
            this.Controls.Add(this.dateDateTimePicker);
            this.Controls.Add(this.narrationTextBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cpvLabel);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CashPaymentVoucher";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cash Payment Voucher";
            this.Activated += new System.EventHandler(this.CashPaymentVoucher_Activated);
            this.Load += new System.EventHandler(this.CashPaymentVoucher_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cpvDataGridView)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button nextButton;
        private System.Windows.Forms.Button previousButton;
        private System.Windows.Forms.Button lastButton;
        private System.Windows.Forms.ComboBox categoryComboBox;
        private System.Windows.Forms.TextBox creditTotalTextBox;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Button clearButton;
        private System.Windows.Forms.Button updateButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.TextBox cashCodeTextBox;
        private System.Windows.Forms.TextBox cpvTextBox;
        private System.Windows.Forms.Label cashLabel;
        private System.Windows.Forms.Button firstButton;
        public System.Windows.Forms.DataGridView cpvDataGridView;
        private System.Windows.Forms.DateTimePicker dateDateTimePicker;
        private System.Windows.Forms.TextBox narrationTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label cpvLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label crvLabel;
        private System.Windows.Forms.TextBox crvTextBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn Code;
        private System.Windows.Forms.DataGridViewTextBoxColumn Description;
        private System.Windows.Forms.DataGridViewTextBoxColumn PartyCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn Amount;
        private System.Windows.Forms.DataGridViewTextBoxColumn Remarks;
        private System.Windows.Forms.DataGridViewButtonColumn RemoveColumnButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
    }
}