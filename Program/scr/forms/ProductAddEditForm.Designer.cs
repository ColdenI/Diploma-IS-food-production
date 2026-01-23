namespace Program.scr.forms
{
    partial class ProductAddEditForm
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
            label1 = new Label();
            btnSave = new Button();
            txtName = new TextBox();
            label2 = new Label();
            txtCategory = new TextBox();
            txtUnitOfMeasure = new TextBox();
            numShelfLifeDays = new NumericUpDown();
            numPricePerUnit = new NumericUpDown();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            dgvIngredients = new DataGridView();
            label6 = new Label();
            btnAddIngredient = new Button();
            btnRemoveSelected = new Button();
            ((System.ComponentModel.ISupportInitialize)numShelfLifeDays).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numPricePerUnit).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvIngredients).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 15);
            label1.Name = "label1";
            label1.Size = new Size(69, 19);
            label1.TabIndex = 0;
            label1.Text = "Название";
            // 
            // btnSave
            // 
            btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnSave.Font = new Font("Segoe UI Semibold", 11.7818184F, FontStyle.Bold, GraphicsUnit.Point, 204);
            btnSave.Location = new Point(322, 398);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(116, 43);
            btnSave.TabIndex = 1;
            btnSave.Text = "Сохранить";
            btnSave.UseVisualStyleBackColor = true;
            // 
            // txtName
            // 
            txtName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtName.Location = new Point(154, 12);
            txtName.MaxLength = 200;
            txtName.Name = "txtName";
            txtName.Size = new Size(284, 26);
            txtName.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 47);
            label2.Name = "label2";
            label2.Size = new Size(73, 19);
            label2.TabIndex = 3;
            label2.Text = "Категория";
            // 
            // txtCategory
            // 
            txtCategory.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtCategory.Location = new Point(154, 44);
            txtCategory.MaxLength = 100;
            txtCategory.Name = "txtCategory";
            txtCategory.Size = new Size(284, 26);
            txtCategory.TabIndex = 4;
            // 
            // txtUnitOfMeasure
            // 
            txtUnitOfMeasure.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtUnitOfMeasure.Location = new Point(154, 76);
            txtUnitOfMeasure.MaxLength = 50;
            txtUnitOfMeasure.Name = "txtUnitOfMeasure";
            txtUnitOfMeasure.Size = new Size(284, 26);
            txtUnitOfMeasure.TabIndex = 5;
            // 
            // numShelfLifeDays
            // 
            numShelfLifeDays.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            numShelfLifeDays.Location = new Point(154, 108);
            numShelfLifeDays.Name = "numShelfLifeDays";
            numShelfLifeDays.Size = new Size(284, 26);
            numShelfLifeDays.TabIndex = 6;
            // 
            // numPricePerUnit
            // 
            numPricePerUnit.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            numPricePerUnit.Location = new Point(154, 140);
            numPricePerUnit.Name = "numPricePerUnit";
            numPricePerUnit.Size = new Size(284, 26);
            numPricePerUnit.TabIndex = 7;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 142);
            label3.Name = "label3";
            label3.Size = new Size(80, 19);
            label3.TabIndex = 8;
            label3.Text = "Цена за ед.";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 110);
            label4.Name = "label4";
            label4.Size = new Size(138, 19);
            label4.TabIndex = 9;
            label4.Text = "Срок годности (дни)";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(12, 79);
            label5.Name = "label5";
            label5.Size = new Size(100, 19);
            label5.TabIndex = 10;
            label5.Text = "Ед. измерения";
            // 
            // dgvIngredients
            // 
            dgvIngredients.AllowUserToAddRows = false;
            dgvIngredients.AllowUserToDeleteRows = false;
            dgvIngredients.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvIngredients.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvIngredients.Location = new Point(12, 197);
            dgvIngredients.Name = "dgvIngredients";
            dgvIngredients.ReadOnly = true;
            dgvIngredients.RowHeadersWidth = 47;
            dgvIngredients.Size = new Size(426, 195);
            dgvIngredients.TabIndex = 11;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(12, 175);
            label6.Name = "label6";
            label6.Size = new Size(55, 19);
            label6.TabIndex = 12;
            label6.Text = "Состав:";
            // 
            // btnAddIngredient
            // 
            btnAddIngredient.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnAddIngredient.Location = new Point(12, 398);
            btnAddIngredient.Name = "btnAddIngredient";
            btnAddIngredient.Size = new Size(86, 26);
            btnAddIngredient.TabIndex = 13;
            btnAddIngredient.Text = "Добавить";
            btnAddIngredient.UseVisualStyleBackColor = true;
            // 
            // btnRemoveSelected
            // 
            btnRemoveSelected.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnRemoveSelected.Location = new Point(104, 398);
            btnRemoveSelected.Name = "btnRemoveSelected";
            btnRemoveSelected.Size = new Size(86, 26);
            btnRemoveSelected.TabIndex = 14;
            btnRemoveSelected.Text = "Удалить";
            btnRemoveSelected.UseVisualStyleBackColor = true;
            // 
            // ProductAddEditForm
            // 
            AutoScaleDimensions = new SizeF(8F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(450, 453);
            Controls.Add(btnRemoveSelected);
            Controls.Add(btnAddIngredient);
            Controls.Add(label6);
            Controls.Add(dgvIngredients);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(numPricePerUnit);
            Controls.Add(numShelfLifeDays);
            Controls.Add(txtUnitOfMeasure);
            Controls.Add(txtCategory);
            Controls.Add(label2);
            Controls.Add(txtName);
            Controls.Add(btnSave);
            Controls.Add(label1);
            Name = "ProductAddEditForm";
            Text = "Продукт";
            ((System.ComponentModel.ISupportInitialize)numShelfLifeDays).EndInit();
            ((System.ComponentModel.ISupportInitialize)numPricePerUnit).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvIngredients).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Button btnSave;
        private TextBox txtName;
        private Label label2;
        private TextBox txtCategory;
        private TextBox txtUnitOfMeasure;
        private NumericUpDown numShelfLifeDays;
        private NumericUpDown numPricePerUnit;
        private Label label3;
        private Label label4;
        private Label label5;
        private DataGridView dgvIngredients;
        private Label label6;
        private Button btnAddIngredient;
        private Button btnRemoveSelected;
    }
}