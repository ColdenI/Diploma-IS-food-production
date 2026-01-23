using Program.scr.core;
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

namespace Program.scr.forms
{
    public partial class ProductInfoForm : Form
    {
        private DBT_Products Product;

        public ProductInfoForm(int productId)
        {
            InitializeComponent();

            Product = DBT_Products.GetById(productId);

            this.Text = $"Состав \"{Product.Name}\"";

            dataGridView.Rows.Clear();
            dataGridView.Columns.Clear();
            dataGridView.BringToFront();
            dataGridView.ReadOnly = true;
            dataGridView.Dock = DockStyle.Fill;
            dataGridView.RowsDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dataGridView.Columns.Add("ID", "Сырьё");
            dataGridView.Columns.Add("CompanyName", "Количество");

            foreach (var i in Core.Get_RawMaterialsIdAndQuantity_ByProductId(productId))
            {
                var index = dataGridView.Rows.Add();
                var n = DBT_RawMaterials.GetById(i.Key);
                dataGridView.Rows[index].Cells[0].Value = n.Name;
                dataGridView.Rows[index].Cells[1].Value = $"{i.Value} {n.UnitOfMeasure}";
            }
        }
    }
}
