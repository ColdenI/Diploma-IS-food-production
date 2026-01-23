namespace Program.scr.forms.client
{
    partial class ClientDoOrderForm
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
            btnOrder = new Button();
            btnCheck = new Button();
            label1 = new Label();
            txtEmail = new TextBox();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            txtFullName = new TextBox();
            txtContactPerson = new TextBox();
            txtPhone = new TextBox();
            dgvOrderItems = new DataGridView();
            label5 = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvOrderItems).BeginInit();
            SuspendLayout();
            // 
            // btnOrder
            // 
            btnOrder.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnOrder.Enabled = false;
            btnOrder.Font = new Font("Segoe UI", 11.7818184F, FontStyle.Bold, GraphicsUnit.Point, 204);
            btnOrder.Location = new Point(418, 365);
            btnOrder.Name = "btnOrder";
            btnOrder.Size = new Size(124, 43);
            btnOrder.TabIndex = 2;
            btnOrder.Text = "Заказать";
            btnOrder.UseVisualStyleBackColor = true;
            // 
            // btnCheck
            // 
            btnCheck.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCheck.Location = new Point(456, 8);
            btnCheck.Name = "btnCheck";
            btnCheck.Size = new Size(86, 26);
            btnCheck.TabIndex = 3;
            btnCheck.Text = "Проверить";
            btnCheck.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 12);
            label1.Name = "label1";
            label1.Size = new Size(41, 19);
            label1.TabIndex = 4;
            label1.Text = "Email";
            // 
            // txtEmail
            // 
            txtEmail.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtEmail.Location = new Point(135, 9);
            txtEmail.MaxLength = 250;
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(315, 26);
            txtEmail.TabIndex = 5;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 44);
            label2.Name = "label2";
            label2.Size = new Size(40, 19);
            label2.TabIndex = 6;
            label2.Text = "ФИО";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 76);
            label3.Name = "label3";
            label3.Size = new Size(117, 19);
            label3.TabIndex = 7;
            label3.Text = "Контактное лицо";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 108);
            label4.Name = "label4";
            label4.Size = new Size(63, 19);
            label4.TabIndex = 8;
            label4.Text = "Телефон";
            // 
            // txtFullName
            // 
            txtFullName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtFullName.Enabled = false;
            txtFullName.Location = new Point(135, 41);
            txtFullName.MaxLength = 250;
            txtFullName.Name = "txtFullName";
            txtFullName.Size = new Size(407, 26);
            txtFullName.TabIndex = 9;
            // 
            // txtContactPerson
            // 
            txtContactPerson.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtContactPerson.Enabled = false;
            txtContactPerson.Location = new Point(135, 73);
            txtContactPerson.MaxLength = 250;
            txtContactPerson.Name = "txtContactPerson";
            txtContactPerson.Size = new Size(407, 26);
            txtContactPerson.TabIndex = 10;
            // 
            // txtPhone
            // 
            txtPhone.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtPhone.Enabled = false;
            txtPhone.Location = new Point(135, 105);
            txtPhone.MaxLength = 50;
            txtPhone.Name = "txtPhone";
            txtPhone.Size = new Size(407, 26);
            txtPhone.TabIndex = 11;
            // 
            // dgvOrderItems
            // 
            dgvOrderItems.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvOrderItems.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvOrderItems.Location = new Point(12, 137);
            dgvOrderItems.Name = "dgvOrderItems";
            dgvOrderItems.RowHeadersWidth = 47;
            dgvOrderItems.Size = new Size(530, 222);
            dgvOrderItems.TabIndex = 12;
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 9.818182F, FontStyle.Bold, GraphicsUnit.Point, 204);
            label5.ForeColor = Color.ForestGreen;
            label5.Location = new Point(24, 378);
            label5.Name = "label5";
            label5.Size = new Size(56, 20);
            label5.TabIndex = 13;
            label5.Text = "Итого:";
            // 
            // ClientDoOrderForm
            // 
            AutoScaleDimensions = new SizeF(8F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(557, 421);
            Controls.Add(label5);
            Controls.Add(dgvOrderItems);
            Controls.Add(txtPhone);
            Controls.Add(txtContactPerson);
            Controls.Add(txtFullName);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(txtEmail);
            Controls.Add(label1);
            Controls.Add(btnCheck);
            Controls.Add(btnOrder);
            Name = "ClientDoOrderForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Заказать";
            ((System.ComponentModel.ISupportInitialize)dgvOrderItems).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnOrder;
        private Button btnCheck;
        private Label label1;
        private TextBox txtEmail;
        private Label label2;
        private Label label3;
        private Label label4;
        private TextBox txtFullName;
        private TextBox txtContactPerson;
        private TextBox txtPhone;
        private DataGridView dgvOrderItems;
        private Label label5;
    }
}