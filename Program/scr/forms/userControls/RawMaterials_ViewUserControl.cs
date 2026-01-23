using Microsoft.Data.SqlClient;
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

namespace Program.scr.forms.userControls
{
    public partial class RawMaterials_ViewUserControl : UserControl
    {
        DataGridView dataGridView;
        TextBox textBox_search;
        Button button_edit;
        Button button_update;
        Button button_create;
        Button button_remove;

        public RawMaterials_ViewUserControl()
        {
            InitializeComponent();

            this.Load += RawMaterials_ViewForm_Load;
            this.Disposed += RawMaterials_ViewForm_Disposed;
            this.Size = new Size(600, 500);
            this.Text = "Сырьё";

            button_create = new Button()
            {
                Text = "Добавить",
                Height = 30,
                Dock = DockStyle.Bottom
            };
            button_edit = new Button()
            {
                Text = "Изменить",
                Height = 30,
                Dock = DockStyle.Bottom
            };
            button_remove = new Button()
            {
                Text = "Удалить",
                Height = 30,
                Dock = DockStyle.Bottom
            };
            button_update = new Button()
            {
                Text = "Обновить",
                Height = 30,
                Dock = DockStyle.Bottom
            };
            textBox_search = new TextBox()
            {
                Dock = DockStyle.Top
            };
            dataGridView = new DataGridView()
            {
                Dock = DockStyle.Fill
            };

            button_update.Click += Button_update_Click;
            button_remove.Click += Button_remove_Click;
            button_create.Click += Button_create_Click;
            button_edit.Click += Button_edit_Click;
            textBox_search.TextChanged += TextBox_search_TextChanged;

            this.Controls.Add(button_create);
            this.Controls.Add(button_edit);
            this.Controls.Add(button_remove);
            this.Controls.Add(button_update);
            this.Controls.Add(textBox_search);
            this.Controls.Add(dataGridView);
        }

        private void RawMaterials_ViewForm_Disposed(object? sender, EventArgs e)
        {
            this.Load -= RawMaterials_ViewForm_Load;
            this.Disposed -= RawMaterials_ViewForm_Disposed;
            button_update.Click -= Button_update_Click;
            button_remove.Click -= Button_remove_Click;
            button_create.Click -= Button_create_Click;
            button_edit.Click -= Button_edit_Click;
            textBox_search.TextChanged -= TextBox_search_TextChanged;
        }

        private void Button_edit_Click(object? sender, EventArgs e)
        {
            new TopPageForm(new RawMaterials_AddEditUserControl(DBT_RawMaterials.GetById((int)dataGridView.CurrentCell.OwningRow.Cells[0].Value)),"Изменить").ShowDialog();
            UpdateTable();
        }

        private void Button_create_Click(object? sender, EventArgs e)
        {
            new TopPageForm(new RawMaterials_AddEditUserControl(), "Добавить").ShowDialog();
            UpdateTable();
        }

        private void Button_remove_Click(object? sender, EventArgs e)
        {
            if (MessageBox.Show("Вы уверены что хотите удалить запись?\r\nОтменить будет невозможно!\r\n", "Удалить", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                return;

            int rawMaterialId = (int)dataGridView.CurrentCell.OwningRow.Cells[0].Value;

            // Проверяем, используется ли сырьё в рецептурах
            if (IsRawMaterialUsedInRecipes(rawMaterialId))
            {
                MessageBox.Show("Невозможно удалить сырьё: оно используется в рецептурах.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Проверяем, есть ли поставки этого сырья
            if (IsRawMaterialInSupplies(rawMaterialId))
            {
                MessageBox.Show("Невозможно удалить сырьё: оно есть в поставках.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Если всё ок — удаляем
            int res = DBT_RawMaterials.Remove(rawMaterialId);
            if (res == -1)
                MessageBox.Show("Ошибка удаления!");
            else if (res == 0)
                MessageBox.Show("Успешно удалено!");

            UpdateTable();
        }

        private bool IsRawMaterialUsedInRecipes(int rawMaterialId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(scr.core.SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var cmd = new SqlCommand("SELECT COUNT(*) FROM Recipes WHERE RawMaterialID = @RawMaterialID", connection))
                    {
                        cmd.Parameters.AddWithValue("@RawMaterialID", rawMaterialId);
                        int count = (int)cmd.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch
            {
                return true; // В случае ошибки считаем, что используется
            }
        }

        private bool IsRawMaterialInSupplies(int rawMaterialId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(scr.core.SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var cmd = new SqlCommand("SELECT COUNT(*) FROM Supplies WHERE RawMaterialID = @RawMaterialID", connection))
                    {
                        cmd.Parameters.AddWithValue("@RawMaterialID", rawMaterialId);
                        int count = (int)cmd.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch
            {
                return true; // В случае ошибки считаем, что используется
            }
        }

        private void TextBox_search_TextChanged(object? sender, EventArgs e) => UpdateTable();
        private void Button_update_Click(object? sender, EventArgs e) => UpdateTable();
        private void RawMaterials_ViewForm_Load(object sender, EventArgs e) => UpdateTable();

        private void UpdateTable()
        {
            dataGridView.Rows.Clear();
            dataGridView.Columns.Clear();
            dataGridView.BringToFront();
            dataGridView.ReadOnly = true;
            dataGridView.Dock = DockStyle.Fill;
            dataGridView.RowsDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dataGridView.Columns.Add("ID", "ID");
            dataGridView.Columns.Add("Name", "Наименование");
            dataGridView.Columns.Add("UnitOfMeasure", "Единица измерения");
            dataGridView.Columns.Add("MinStockLevel", "Минимальный остаток");
            dataGridView.Columns.Add("QuantityInStock", "На складе");

            using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
            {
                connection.Open();
                using (var query = connection.CreateCommand())
                {
                    query.CommandText = "SELECT * FROM RawMaterials";
                    using (var reader = query.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var index = dataGridView.Rows.Add();
                            dataGridView.Rows[index].Cells[0].Value = reader.GetInt32(0);
                            dataGridView.Rows[index].Cells[1].Value = reader.GetString(1);
                            dataGridView.Rows[index].Cells[2].Value = reader.GetString(2);
                            dataGridView.Rows[index].Cells[3].Value = reader.GetDecimal(3);
                            dataGridView.Rows[index].Cells[4].Value = reader.GetDecimal(4);

                            string search = textBox_search.Text.ToLower();
                            if (!string.IsNullOrWhiteSpace(search))
                                if (
                                    !dataGridView.Rows[index].Cells[0].Value.ToString().ToLower().Contains(search) &&
                                    !dataGridView.Rows[index].Cells[1].Value.ToString().ToLower().Contains(search) &&
                                    !dataGridView.Rows[index].Cells[2].Value.ToString().ToLower().Contains(search) &&
                                    !dataGridView.Rows[index].Cells[3].Value.ToString().ToLower().Contains(search) &&
                                    !dataGridView.Rows[index].Cells[4].Value.ToString().ToLower().Contains(search)
                                ) dataGridView.Rows.RemoveAt(index);
                        }
                    }
                }
            }
        }
    }


}
