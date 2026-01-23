using Microsoft.Data.SqlClient;
using Program.scr.core;
using Program.scr.core.dbt;

namespace Program.scr.forms.userControls
{
    public partial class Products_ViewUserControl : UserControl
    {
        DataGridView dataGridView;
        TextBox textBox_search;
        Button button_edit;
        Button button_update;
        Button button_create;
        Button button_remove;
        Button button_info;

        public Products_ViewUserControl()
        {
            InitializeComponent();

            this.Load += Products_ViewForm_Load;
            this.Disposed += Products_ViewForm_Disposed;
            this.Size = new Size(600, 500);
            this.Text = "Товары";

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
            button_info = new Button()
            {
                Text = "Состав",
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
            button_info.Click += (s, e) => new ProductInfoForm((int)dataGridView.CurrentCell.OwningRow.Cells[0].Value).ShowDialog();
            textBox_search.TextChanged += TextBox_search_TextChanged;

            this.Controls.Add(button_create);
            this.Controls.Add(button_edit);
            this.Controls.Add(button_remove);
            this.Controls.Add(button_info);
            this.Controls.Add(button_update);
            
            this.Controls.Add(textBox_search);
            this.Controls.Add(dataGridView);
        }

        private void Products_ViewForm_Disposed(object? sender, EventArgs e)
        {
            this.Load -= Products_ViewForm_Load;
            this.Disposed -= Products_ViewForm_Disposed;
            button_update.Click -= Button_update_Click;
            button_remove.Click -= Button_remove_Click;
            button_create.Click -= Button_create_Click;
            button_edit.Click -= Button_edit_Click;
            textBox_search.TextChanged -= TextBox_search_TextChanged;
        }

        private void Button_edit_Click(object? sender, EventArgs e)
        {
            new ProductAddEditForm((int)dataGridView.CurrentCell.OwningRow.Cells[0].Value).ShowDialog();
            UpdateTable();
        }

        private void Button_create_Click(object? sender, EventArgs e)
        {
            new ProductAddEditForm().ShowDialog();
            UpdateTable();
        }

        private void Button_remove_Click(object? sender, EventArgs e)
        {
            if (MessageBox.Show("Вы уверены что хотите удалить запись?\r\nОтменить будет невозможно!\r\n", "Удалить", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                return;

            int productId = (int)dataGridView.CurrentCell.OwningRow.Cells[0].Value;

            // Проверяем, используется ли продукт в заказах
            if (IsProductUsedInOrders(productId))
            {
                MessageBox.Show("Невозможно удалить продукт: он используется в заказах.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Если всё ок — удаляем
            int res = DBT_Products.Remove(productId);
            if (res == -1)
                MessageBox.Show("Ошибка удаления!");
            else if (res == 0)
                MessageBox.Show("Успешно удалено!");

            UpdateTable();
        }

        private bool IsProductUsedInOrders(int productId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(scr.core.SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var cmd = new SqlCommand("SELECT COUNT(*) FROM OrderItems WHERE ProductID = @ProductID", connection))
                    {
                        cmd.Parameters.AddWithValue("@ProductID", productId);
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
        private void Products_ViewForm_Load(object sender, EventArgs e) => UpdateTable();

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
            dataGridView.Columns.Add("Category", "Категория");
            dataGridView.Columns.Add("UnitOfMeasure", "Единица измерения");
            dataGridView.Columns.Add("ShelfLifeDays", "Срок годности");
            dataGridView.Columns.Add("PricePerUnit", "Цена за ед. товара");

            using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
            {
                connection.Open();
                using (var query = connection.CreateCommand())
                {
                    query.CommandText = "SELECT * FROM Products";
                    using (var reader = query.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var index = dataGridView.Rows.Add();
                            dataGridView.Rows[index].Cells[0].Value = reader.GetInt32(0);
                            dataGridView.Rows[index].Cells[1].Value = reader.GetString(1);
                            dataGridView.Rows[index].Cells[2].Value = reader.GetString(2);
                            dataGridView.Rows[index].Cells[3].Value = reader.GetString(3);
                            dataGridView.Rows[index].Cells[4].Value = reader.GetInt32(4);
                            dataGridView.Rows[index].Cells[5].Value = reader.GetDecimal(5).ToString("C");

                            string search = textBox_search.Text.ToLower();
                            if (!string.IsNullOrWhiteSpace(search))
                                if (
                                    !dataGridView.Rows[index].Cells[0].Value.ToString().ToLower().Contains(search) &&
                                    !dataGridView.Rows[index].Cells[1].Value.ToString().ToLower().Contains(search) &&
                                    !dataGridView.Rows[index].Cells[2].Value.ToString().ToLower().Contains(search) &&
                                    !dataGridView.Rows[index].Cells[3].Value.ToString().ToLower().Contains(search) &&
                                    !dataGridView.Rows[index].Cells[4].Value.ToString().ToLower().Contains(search) &&
                                    !dataGridView.Rows[index].Cells[5].Value.ToString().ToLower().Contains(search)
                                ) dataGridView.Rows.RemoveAt(index);
                        }
                    }
                }
            }
        }
    }
}
