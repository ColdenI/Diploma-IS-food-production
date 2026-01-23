namespace Program
{
    partial class AuthForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panel_empl = new Panel();
            textBox_e_password = new TextBox();
            label3 = new Label();
            textBox_e_login = new TextBox();
            button_close = new Button();
            button_e_login = new Button();
            label2 = new Label();
            panel_client = new Panel();
            button_c_catalog = new Button();
            textBox_c_order = new TextBox();
            label4 = new Label();
            button2 = new Button();
            textBox_c_email = new TextBox();
            label5 = new Label();
            button_c_login = new Button();
            label1 = new Label();
            button_empl = new Button();
            button_client = new Button();
            panel_empl.SuspendLayout();
            panel_client.SuspendLayout();
            SuspendLayout();
            // 
            // panel_empl
            // 
            panel_empl.Controls.Add(textBox_e_password);
            panel_empl.Controls.Add(label3);
            panel_empl.Controls.Add(textBox_e_login);
            panel_empl.Controls.Add(button_close);
            panel_empl.Controls.Add(button_e_login);
            panel_empl.Controls.Add(label2);
            panel_empl.Location = new Point(15, 12);
            panel_empl.Name = "panel_empl";
            panel_empl.Size = new Size(288, 190);
            panel_empl.TabIndex = 0;
            panel_empl.Visible = false;
            // 
            // textBox_e_password
            // 
            textBox_e_password.Location = new Point(15, 105);
            textBox_e_password.Name = "textBox_e_password";
            textBox_e_password.PasswordChar = '*';
            textBox_e_password.Size = new Size(255, 26);
            textBox_e_password.TabIndex = 9;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(15, 83);
            label3.Name = "label3";
            label3.Size = new Size(56, 19);
            label3.TabIndex = 8;
            label3.Text = "Пароль";
            // 
            // textBox_e_login
            // 
            textBox_e_login.Location = new Point(15, 45);
            textBox_e_login.Name = "textBox_e_login";
            textBox_e_login.Size = new Size(255, 26);
            textBox_e_login.TabIndex = 7;
            // 
            // button_close
            // 
            button_close.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button_close.Location = new Point(252, 3);
            button_close.Name = "button_close";
            button_close.Size = new Size(33, 26);
            button_close.TabIndex = 6;
            button_close.Text = "X";
            button_close.UseVisualStyleBackColor = true;
            button_close.Click += button_close_Click;
            // 
            // button_e_login
            // 
            button_e_login.Font = new Font("Segoe UI", 11.7818184F, FontStyle.Regular, GraphicsUnit.Point, 204);
            button_e_login.Location = new Point(70, 145);
            button_e_login.Name = "button_e_login";
            button_e_login.Size = new Size(143, 36);
            button_e_login.TabIndex = 5;
            button_e_login.Text = "Войти";
            button_e_login.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(15, 23);
            label2.Name = "label2";
            label2.Size = new Size(47, 19);
            label2.TabIndex = 0;
            label2.Text = "Логин";
            // 
            // panel_client
            // 
            panel_client.Controls.Add(button_c_catalog);
            panel_client.Controls.Add(textBox_c_order);
            panel_client.Controls.Add(label4);
            panel_client.Controls.Add(button2);
            panel_client.Controls.Add(textBox_c_email);
            panel_client.Controls.Add(label5);
            panel_client.Controls.Add(button_c_login);
            panel_client.Location = new Point(15, 12);
            panel_client.Name = "panel_client";
            panel_client.Size = new Size(288, 190);
            panel_client.TabIndex = 1;
            // 
            // button_c_catalog
            // 
            button_c_catalog.Font = new Font("Segoe UI", 11.7818184F, FontStyle.Regular, GraphicsUnit.Point, 204);
            button_c_catalog.Location = new Point(15, 145);
            button_c_catalog.Name = "button_c_catalog";
            button_c_catalog.Size = new Size(125, 36);
            button_c_catalog.TabIndex = 15;
            button_c_catalog.Text = "Каталог";
            button_c_catalog.UseVisualStyleBackColor = true;
            // 
            // textBox_c_order
            // 
            textBox_c_order.Location = new Point(15, 105);
            textBox_c_order.Name = "textBox_c_order";
            textBox_c_order.Size = new Size(255, 26);
            textBox_c_order.TabIndex = 14;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(15, 83);
            label4.Name = "label4";
            label4.Size = new Size(71, 19);
            label4.TabIndex = 13;
            label4.Text = "№ Заказа";
            // 
            // button2
            // 
            button2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button2.Location = new Point(252, 3);
            button2.Name = "button2";
            button2.Size = new Size(33, 26);
            button2.TabIndex = 7;
            button2.Text = "X";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button_close_Click;
            // 
            // textBox_c_email
            // 
            textBox_c_email.Location = new Point(15, 45);
            textBox_c_email.Name = "textBox_c_email";
            textBox_c_email.Size = new Size(255, 26);
            textBox_c_email.TabIndex = 12;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(15, 23);
            label5.Name = "label5";
            label5.Size = new Size(41, 19);
            label5.TabIndex = 10;
            label5.Text = "Email";
            // 
            // button_c_login
            // 
            button_c_login.Font = new Font("Segoe UI", 11.7818184F, FontStyle.Regular, GraphicsUnit.Point, 204);
            button_c_login.Location = new Point(146, 145);
            button_c_login.Name = "button_c_login";
            button_c_login.Size = new Size(124, 36);
            button_c_login.TabIndex = 11;
            button_c_login.Text = "Проверить";
            button_c_login.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Black", 13.7454548F, FontStyle.Bold, GraphicsUnit.Point, 204);
            label1.Location = new Point(24, 18);
            label1.Name = "label1";
            label1.Size = new Size(270, 30);
            label1.TabIndex = 2;
            label1.Text = "Пищевое производство";
            // 
            // button_empl
            // 
            button_empl.Font = new Font("Segoe UI", 11.7818184F, FontStyle.Regular, GraphicsUnit.Point, 204);
            button_empl.Location = new Point(85, 82);
            button_empl.Name = "button_empl";
            button_empl.Size = new Size(143, 36);
            button_empl.TabIndex = 3;
            button_empl.Text = "Я сотрудник";
            button_empl.UseVisualStyleBackColor = true;
            // 
            // button_client
            // 
            button_client.Font = new Font("Segoe UI", 11.7818184F, FontStyle.Regular, GraphicsUnit.Point, 204);
            button_client.Location = new Point(85, 124);
            button_client.Name = "button_client";
            button_client.Size = new Size(143, 36);
            button_client.TabIndex = 4;
            button_client.Text = "Я клиент";
            button_client.UseVisualStyleBackColor = true;
            // 
            // AuthForm
            // 
            AutoScaleDimensions = new SizeF(8F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(321, 217);
            Controls.Add(panel_empl);
            Controls.Add(panel_client);
            Controls.Add(button_client);
            Controls.Add(button_empl);
            Controls.Add(label1);
            Name = "AuthForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Авторизация";
            panel_empl.ResumeLayout(false);
            panel_empl.PerformLayout();
            panel_client.ResumeLayout(false);
            panel_client.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel_empl;
        private Panel panel_client;
        private Label label1;
        private Button button_empl;
        private Label label2;
        private Button button_client;
        private Label label3;
        private TextBox textBox_e_login;
        private Button button_close;
        private Button button_e_login;
        private Button button2;
        private TextBox textBox_e_password;
        private TextBox textBox_c_order;
        private Label label4;
        private TextBox textBox_c_email;
        private Label label5;
        private Button button_c_login;
        private Button button_c_catalog;
    }
}
