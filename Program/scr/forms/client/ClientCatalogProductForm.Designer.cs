namespace Program.scr.forms.client
{
    partial class ClientCatalogProductForm
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
            button_order = new Button();
            tableLayoutPanel = new TableLayoutPanel();
            SuspendLayout();
            // 
            // button_order
            // 
            button_order.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button_order.Font = new Font("Segoe UI", 11.7818184F, FontStyle.Bold, GraphicsUnit.Point, 204);
            button_order.Location = new Point(546, 395);
            button_order.Name = "button_order";
            button_order.Size = new Size(124, 43);
            button_order.TabIndex = 1;
            button_order.Text = "Заказать";
            button_order.UseVisualStyleBackColor = true;
            button_order.Click += button_order_Click;
            // 
            // tableLayoutPanel
            // 
            tableLayoutPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel.AutoScroll = true;
            tableLayoutPanel.BackColor = SystemColors.Control;
            tableLayoutPanel.ColumnCount = 3;
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
            tableLayoutPanel.Location = new Point(0, 0);
            tableLayoutPanel.Name = "tableLayoutPanel";
            tableLayoutPanel.RowCount = 2;
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel.Size = new Size(680, 389);
            tableLayoutPanel.TabIndex = 3;
            // 
            // ClientCatalogProductForm
            // 
            AutoScaleDimensions = new SizeF(8F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(682, 450);
            Controls.Add(tableLayoutPanel);
            Controls.Add(button_order);
            Name = "ClientCatalogProductForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Каталог товаров";
            Load += ClientCatalogProductForm_Load;
            ResumeLayout(false);
        }

        #endregion
        private Button button_order;
        private TableLayoutPanel tableLayoutPanel;
    }
}