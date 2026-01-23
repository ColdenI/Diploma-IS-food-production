namespace Program.scr.forms.client
{
    partial class ClientOrderViewForm
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
            dgvOrderItems = new DataGridView();
            lblTotalAmount = new Label();
            lblOrderDate = new Label();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvOrderItems).BeginInit();
            SuspendLayout();
            // 
            // dgvOrderItems
            // 
            dgvOrderItems.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvOrderItems.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvOrderItems.Location = new Point(12, 51);
            dgvOrderItems.Name = "dgvOrderItems";
            dgvOrderItems.RowHeadersWidth = 47;
            dgvOrderItems.Size = new Size(485, 253);
            dgvOrderItems.TabIndex = 0;
            // 
            // lblTotalAmount
            // 
            lblTotalAmount.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblTotalAmount.AutoSize = true;
            lblTotalAmount.Font = new Font("Segoe UI", 9.818182F, FontStyle.Bold, GraphicsUnit.Point, 204);
            lblTotalAmount.ForeColor = Color.Green;
            lblTotalAmount.Location = new Point(12, 317);
            lblTotalAmount.Name = "lblTotalAmount";
            lblTotalAmount.Size = new Size(51, 20);
            lblTotalAmount.TabIndex = 1;
            lblTotalAmount.Text = "label1";
            // 
            // lblOrderDate
            // 
            lblOrderDate.AutoSize = true;
            lblOrderDate.Font = new Font("Segoe UI Semibold", 9.163636F, FontStyle.Bold);
            lblOrderDate.Location = new Point(12, 9);
            lblOrderDate.Name = "lblOrderDate";
            lblOrderDate.Size = new Size(47, 19);
            lblOrderDate.TabIndex = 2;
            lblOrderDate.Text = "label2";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 9.163636F, FontStyle.Bold);
            label1.Location = new Point(12, 29);
            label1.Name = "label1";
            label1.Size = new Size(47, 19);
            label1.TabIndex = 3;
            label1.Text = "label2";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // ClientOrderViewForm
            // 
            AutoScaleDimensions = new SizeF(8F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(509, 350);
            Controls.Add(label1);
            Controls.Add(lblOrderDate);
            Controls.Add(lblTotalAmount);
            Controls.Add(dgvOrderItems);
            Name = "ClientOrderViewForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Статус заказа";
            ((System.ComponentModel.ISupportInitialize)dgvOrderItems).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvOrderItems;
        private Label lblTotalAmount;
        private Label lblOrderDate;
        private Label label1;
    }
}