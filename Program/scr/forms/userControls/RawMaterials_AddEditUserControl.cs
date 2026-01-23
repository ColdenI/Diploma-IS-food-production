using Program.scr.core.dbt;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Program.scr.forms.userControls
{
    public partial class RawMaterials_AddEditUserControl : UserControl
    {
        DBT_RawMaterials Object;

        Button button_apply;
        TableLayoutPanel tableLayout;

        TextBox textBox_Name;
        TextBox textBox_UnitOfMeasure;
        TextBox textBox_MinStockLevel;
        TextBox textBox_QuantityInStock;

        public RawMaterials_AddEditUserControl()
        {
            InitializeComponent();
            Init();
        }
        public RawMaterials_AddEditUserControl(DBT_RawMaterials obj)
        {
            InitializeComponent();
            Object = obj;
            Init();

            textBox_Name.Text = obj.Name.ToString();
            textBox_UnitOfMeasure.Text = obj.UnitOfMeasure.ToString();
            textBox_MinStockLevel.Text = obj.MinStockLevel.ToString();
            textBox_QuantityInStock.Text = obj.QuantityInStock.ToString();
        }

        private void Init()
        {
            this.Size = new Size(600, 500);
            this.Text = "Сырьё - " + (Object == null ? "Добавить" : "Изменить").ToString();
            this.MinimumSize = new Size(400, 400);
            button_apply = new Button()
            {
                Height = 30,
                Dock = DockStyle.Bottom
            };
            button_apply.Click += Button_apply_Click;
            button_apply.Text = Object == null ? "Добавить" : "Изменить";
            this.Controls.Add(button_apply);

            tableLayout = new TableLayoutPanel()
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 4
            };

            Label label_Name = new Label();
            SetLabel(ref label_Name, "Наименование");
            tableLayout.Controls.Add(label_Name, 0, 0);
            textBox_Name = new TextBox();
            textBox_Name.Dock = DockStyle.Fill;
            textBox_Name.MaxLength = 255;
            tableLayout.Controls.Add(textBox_Name, 1, 0);

            Label label_UnitOfMeasure = new Label();
            SetLabel(ref label_UnitOfMeasure, "Единица измерения");
            tableLayout.Controls.Add(label_UnitOfMeasure, 0, 1);
            textBox_UnitOfMeasure = new TextBox();
            textBox_UnitOfMeasure.Dock = DockStyle.Fill;
            textBox_UnitOfMeasure.MaxLength = 50;
            tableLayout.Controls.Add(textBox_UnitOfMeasure, 1, 1);

            Label label_MinStockLevel = new Label();
            SetLabel(ref label_MinStockLevel, "Минимальный остаток");
            tableLayout.Controls.Add(label_MinStockLevel, 0, 2);
            textBox_MinStockLevel = new TextBox();
            textBox_MinStockLevel.Dock = DockStyle.Fill;
            textBox_MinStockLevel.MaxLength = 1000;
            tableLayout.Controls.Add(textBox_MinStockLevel, 1, 2);

            Label label_QuantityInStock = new Label();
            SetLabel(ref label_QuantityInStock, "На складе");
            tableLayout.Controls.Add(label_QuantityInStock, 0, 3);
            textBox_QuantityInStock = new TextBox();
            textBox_QuantityInStock.Dock = DockStyle.Fill;
            textBox_QuantityInStock.MaxLength = 1000;
            tableLayout.Controls.Add(textBox_QuantityInStock, 1, 3);

            this.Controls.Add(tableLayout);
        }

        private void SetLabel(ref Label label, string text = "")
        {
            label.Font = new Font(Font.FontFamily, 12);
            label.TextAlign = ContentAlignment.TopLeft;
            label.Dock = DockStyle.Fill;
            label.AutoSize = false;
            label.Width = 200;
            label.Text = text;
        }

        private void Button_apply_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox_Name.Text)) { MessageBox.Show("Поле 'Наименование' имеет некорректное значение!"); return; }
            if (string.IsNullOrWhiteSpace(textBox_UnitOfMeasure.Text)) { MessageBox.Show("Поле 'Единица измерения' имеет некорректное значение!"); return; }
            if (!decimal.TryParse(textBox_MinStockLevel.Text, out decimal tp_MinStockLevel)) { MessageBox.Show("Поле 'Минимальный остаток' имеет некорректное значение!"); return; }
            if (!decimal.TryParse(textBox_QuantityInStock.Text, out decimal tp_QuantityInStock)) { MessageBox.Show("Поле 'На складе' имеет некорректное значение!"); return; }

            int res = 0;

            if (Object == null)
            {
                res = DBT_RawMaterials.Create(
                    new DBT_RawMaterials()
                    {
                        Name = textBox_Name.Text,
                        UnitOfMeasure = textBox_UnitOfMeasure.Text,
                        MinStockLevel = decimal.Parse(textBox_MinStockLevel.Text),
                        QuantityInStock = decimal.Parse(textBox_QuantityInStock.Text)
                    }
                );
            }
            else
            {
                res = DBT_RawMaterials.Edit(
                    new DBT_RawMaterials()
                    {
                        ID = Object.ID,
                        Name = textBox_Name.Text,
                        UnitOfMeasure = textBox_UnitOfMeasure.Text,
                        MinStockLevel = decimal.Parse(textBox_MinStockLevel.Text),
                        QuantityInStock = decimal.Parse(textBox_QuantityInStock.Text)
                    }
                );
            }
            if (res == -1) MessageBox.Show("Ошибка! Один из ID не ссылается на запись в БД!");

            (this.Parent as Form).Close();
        }
    }
}
