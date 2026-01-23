using Microsoft.Data.SqlClient;
using Program.scr.core;
using Program.scr.core.dbt;
using Program.scr.forms.client;

namespace Program.scr.forms.userControls
{
    public partial class SalesOrders_ViewUserControl : UserControl
    {
        DataGridView dataGridView;
        TextBox textBox_search;
        Button button_edit;
        Button button_update;
        Button button_create;
        Button button_remove;

        public SalesOrders_ViewUserControl()
        {
            InitializeComponent();

            this.Load += SalesOrders_ViewForm_Load;
            this.Disposed += SalesOrders_ViewForm_Disposed;
            this.Size = new Size(600, 500);
            this.Text = "Заказы";

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
            //this.Controls.Add(button_remove);
            this.Controls.Add(button_update);
            this.Controls.Add(textBox_search);
            this.Controls.Add(dataGridView);
        }

        private void SalesOrders_ViewForm_Disposed(object? sender, EventArgs e)
        {
            this.Load -= SalesOrders_ViewForm_Load;
            this.Disposed -= SalesOrders_ViewForm_Disposed;
            button_update.Click -= Button_update_Click;
            button_remove.Click -= Button_remove_Click;
            button_create.Click -= Button_create_Click;
            button_edit.Click -= Button_edit_Click;
            textBox_search.TextChanged -= TextBox_search_TextChanged;
        }

        private void Button_edit_Click(object? sender, EventArgs e)
        {
            new OrderEditForm((int)dataGridView.CurrentCell.OwningRow.Cells[0].Value).ShowDialog();
            UpdateTable();
        }

        private void Button_create_Click(object? sender, EventArgs e)
        {
            new ClientCatalogProductForm(true).ShowDialog();
            UpdateTable();
        }

        private void Button_remove_Click(object? sender, EventArgs e)
        {
            if (MessageBox.Show("Вы уверены что хотите удалить запись?\r\nОтменить будет невозможно!\r\n", "Удалить", MessageBoxButtons.OKCancel) == DialogResult.Cancel) return;
            int res = DBT_SalesOrders.Remove((int)dataGridView.CurrentCell.OwningRow.Cells[0].Value);
            if (res == -1) MessageBox.Show("Ошибка удаления!");
            else if (res == 0) MessageBox.Show("Успешно удалено!");
            UpdateTable();
        }

        private void TextBox_search_TextChanged(object? sender, EventArgs e) => UpdateTable();
        private void Button_update_Click(object? sender, EventArgs e) => UpdateTable();
        private void SalesOrders_ViewForm_Load(object sender, EventArgs e) => UpdateTable();

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
            dataGridView.Columns.Add("ClientID", "Клиент");
            dataGridView.Columns.Add("ManagerID", "Менеджер");
            dataGridView.Columns.Add("CookID", "Повар");
            dataGridView.Columns.Add("OrderDate", "Дата заказа");
            dataGridView.Columns.Add("CompletionDate", "Дата завершения");
            dataGridView.Columns.Add("TotalAmount", "Сумма заказа");
            dataGridView.Columns.Add("Status", "Статус");

            using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
            {
                connection.Open();
                using (var query = connection.CreateCommand())
                {
                    if (Core.ThisUser_AC == 0) query.CommandText = "SELECT * \r\nFROM SalesOrders\r\nORDER BY \r\n    CASE WHEN ManagerID IS NULL THEN 0 ELSE 1 END,\r\n    ID";
                    else if (Core.ThisUser_AC == 1)
                    {
                        query.CommandText = "SELECT * FROM SalesOrders WHERE Status NOT IN ('Завершено', 'Отменено')  AND (ManagerID IS NULL OR ManagerID = @id) ORDER BY CASE WHEN ManagerID IS NULL THEN 0 ELSE 1 END, ID DESC;";
                        query.Parameters.AddWithValue("@id", Core.ThisUser_ID);
                    }
                    else if (Core.ThisUser_AC == 2)
                    {
                        query.CommandText = "SELECT * FROM SalesOrders WHERE (CookID = @id) AND Status  IN ('В процессе')";
                        query.Parameters.AddWithValue("@id", Core.ThisUser_ID);
                    }
                    else query.CommandText = "SELECT * FROM SalesOrders";
                    using (var reader = query.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var index = dataGridView.Rows.Add();
                            dataGridView.Rows[index].Cells[0].Value = reader.GetInt32(0);
                            if (reader.IsDBNull(1)) dataGridView.Rows[index].Cells[1].Value = "-";
                            else dataGridView.Rows[index].Cells[1].Value = DBT_Clients.GetById(reader.GetInt32(1)).FullName;
                            if (reader.IsDBNull(2)) dataGridView.Rows[index].Cells[2].Value = "-";
                            else dataGridView.Rows[index].Cells[2].Value = DBT_Employees.GetById(reader.GetInt32(2)).FullName;
                            if (reader.IsDBNull(3)) dataGridView.Rows[index].Cells[3].Value = "-";
                            else dataGridView.Rows[index].Cells[3].Value = DBT_Employees.GetById(reader.GetInt32(3)).FullName;
                            dataGridView.Rows[index].Cells[4].Value = DateTime.Parse(reader.GetValue(4).ToString());
                            if (reader.IsDBNull(5)) dataGridView.Rows[index].Cells[5].Value = "-";
                            else dataGridView.Rows[index].Cells[5].Value = DateTime.Parse(reader.GetValue(5).ToString());
                            dataGridView.Rows[index].Cells[6].Value = reader.GetDecimal(6).ToString("C");
                            dataGridView.Rows[index].Cells[7].Value = reader.GetString(7);

                            string search = textBox_search.Text.ToLower();
                            if (!string.IsNullOrWhiteSpace(search))
                                if (
                                    !dataGridView.Rows[index].Cells[0].Value.ToString().ToLower().Contains(search) &&
                                    !dataGridView.Rows[index].Cells[1].Value.ToString().ToLower().Contains(search) &&
                                    !dataGridView.Rows[index].Cells[2].Value.ToString().ToLower().Contains(search) &&
                                    !dataGridView.Rows[index].Cells[3].Value.ToString().ToLower().Contains(search) &&
                                    !dataGridView.Rows[index].Cells[4].Value.ToString().ToLower().Contains(search) &&
                                    !dataGridView.Rows[index].Cells[5].Value.ToString().ToLower().Contains(search) &&
                                    !dataGridView.Rows[index].Cells[6].Value.ToString().ToLower().Contains(search) &&
                                    !dataGridView.Rows[index].Cells[7].Value.ToString().ToLower().Contains(search)
                                ) dataGridView.Rows.RemoveAt(index);
                        }
                    }
                }
            }
        }
    }


}
