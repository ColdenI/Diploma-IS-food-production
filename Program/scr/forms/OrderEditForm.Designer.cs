namespace Program.scr.forms
{
    partial class OrderEditForm
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
            lblOrderNumber = new Label();
            lblClient = new Label();
            lblManager = new Label();
            lblCook = new Label();
            lblOrderDate = new Label();
            lblCompletionDate = new Label();
            lblTotalAmount = new Label();
            lblStatus = new Label();
            cmbCook = new ComboBox();
            cmbManager = new ComboBox();
            cmbStatus = new ComboBox();
            dgvOrderItems = new DataGridView();
            btnSave = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvOrderItems).BeginInit();
            SuspendLayout();
            // 
            // lblOrderNumber
            // 
            lblOrderNumber.Font = new Font("Segoe UI", 9.163636F, FontStyle.Bold, GraphicsUnit.Point, 204);
            lblOrderNumber.Location = new Point(12, 4);
            lblOrderNumber.Name = "lblOrderNumber";
            lblOrderNumber.Size = new Size(570, 22);
            lblOrderNumber.TabIndex = 0;
            lblOrderNumber.Text = "label1";
            // 
            // lblClient
            // 
            lblClient.Location = new Point(12, 32);
            lblClient.Name = "lblClient";
            lblClient.Size = new Size(570, 22);
            lblClient.TabIndex = 1;
            lblClient.Text = "label1";
            // 
            // lblManager
            // 
            lblManager.Location = new Point(12, 62);
            lblManager.Name = "lblManager";
            lblManager.Size = new Size(254, 22);
            lblManager.TabIndex = 2;
            lblManager.Text = "Менеджер";
            // 
            // lblCook
            // 
            lblCook.Location = new Point(12, 95);
            lblCook.Name = "lblCook";
            lblCook.Size = new Size(254, 22);
            lblCook.TabIndex = 3;
            lblCook.Text = "Повар";
            // 
            // lblOrderDate
            // 
            lblOrderDate.Location = new Point(12, 126);
            lblOrderDate.Name = "lblOrderDate";
            lblOrderDate.Size = new Size(197, 22);
            lblOrderDate.TabIndex = 4;
            lblOrderDate.Text = "дата";
            // 
            // lblCompletionDate
            // 
            lblCompletionDate.Location = new Point(215, 126);
            lblCompletionDate.Name = "lblCompletionDate";
            lblCompletionDate.Size = new Size(372, 22);
            lblCompletionDate.TabIndex = 5;
            lblCompletionDate.Text = "дата";
            // 
            // lblTotalAmount
            // 
            lblTotalAmount.Font = new Font("Segoe UI Semibold", 9.818182F, FontStyle.Bold, GraphicsUnit.Point, 204);
            lblTotalAmount.ForeColor = Color.Green;
            lblTotalAmount.Location = new Point(12, 345);
            lblTotalAmount.Name = "lblTotalAmount";
            lblTotalAmount.Size = new Size(175, 22);
            lblTotalAmount.TabIndex = 6;
            lblTotalAmount.Text = "label1";
            // 
            // lblStatus
            // 
            lblStatus.Location = new Point(12, 165);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(169, 22);
            lblStatus.TabIndex = 7;
            lblStatus.Text = "Статус:";
            // 
            // cmbCook
            // 
            cmbCook.FormattingEnabled = true;
            cmbCook.Location = new Point(272, 92);
            cmbCook.Name = "cmbCook";
            cmbCook.Size = new Size(318, 27);
            cmbCook.TabIndex = 8;
            // 
            // cmbManager
            // 
            cmbManager.FormattingEnabled = true;
            cmbManager.Location = new Point(272, 59);
            cmbManager.Name = "cmbManager";
            cmbManager.Size = new Size(318, 27);
            cmbManager.TabIndex = 9;
            // 
            // cmbStatus
            // 
            cmbStatus.Enabled = false;
            cmbStatus.FormattingEnabled = true;
            cmbStatus.Location = new Point(187, 162);
            cmbStatus.Name = "cmbStatus";
            cmbStatus.Size = new Size(403, 27);
            cmbStatus.TabIndex = 10;
            // 
            // dgvOrderItems
            // 
            dgvOrderItems.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvOrderItems.Location = new Point(12, 195);
            dgvOrderItems.Name = "dgvOrderItems";
            dgvOrderItems.RowHeadersWidth = 47;
            dgvOrderItems.Size = new Size(578, 144);
            dgvOrderItems.TabIndex = 11;
            // 
            // btnSave
            // 
            btnSave.Enabled = false;
            btnSave.Location = new Point(474, 376);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(113, 38);
            btnSave.TabIndex = 12;
            btnSave.Text = "Применить";
            btnSave.UseVisualStyleBackColor = true;
            // 
            // OrderEditForm
            // 
            AutoScaleDimensions = new SizeF(8F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(599, 423);
            Controls.Add(btnSave);
            Controls.Add(dgvOrderItems);
            Controls.Add(cmbStatus);
            Controls.Add(cmbManager);
            Controls.Add(cmbCook);
            Controls.Add(lblStatus);
            Controls.Add(lblTotalAmount);
            Controls.Add(lblCompletionDate);
            Controls.Add(lblOrderDate);
            Controls.Add(lblCook);
            Controls.Add(lblManager);
            Controls.Add(lblClient);
            Controls.Add(lblOrderNumber);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "OrderEditForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "OrderEditForm";
            ((System.ComponentModel.ISupportInitialize)dgvOrderItems).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Label lblOrderNumber;
        private Label lblClient;
        private Label lblManager;
        private Label lblCook;
        private Label lblOrderDate;
        private Label lblCompletionDate;
        private Label lblTotalAmount;
        private Label lblStatus;
        private ComboBox cmbCook;
        private ComboBox cmbManager;
        private ComboBox cmbStatus;
        private DataGridView dgvOrderItems;
        private Button btnSave;
    }
}