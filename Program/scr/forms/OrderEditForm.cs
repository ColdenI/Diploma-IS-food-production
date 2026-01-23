using Microsoft.Data.SqlClient;
using System.Data;
using System.Text;

namespace Program.scr.forms
{
    public partial class OrderEditForm : Form
    {
        private int orderId;
        private int userAccessLevel;
        private scr.core.dbt.DBT_SalesOrders order;

        // Списки для хранения сотрудников
        private List<scr.core.dbt.DBT_Employees> managers = new List<scr.core.dbt.DBT_Employees>();
        private List<scr.core.dbt.DBT_Employees> cooks = new List<scr.core.dbt.DBT_Employees>();

        public OrderEditForm(int orderId)
        {
            this.orderId = orderId;
            this.userAccessLevel = scr.core.Core.ThisUser_AC;
            InitializeComponent();
            InitializeComponent_();

            LoadOrderData();

            // Автоматическое назначение менеджера, если он открыл заказ без назначенного менеджера
            if (userAccessLevel == 1 && order.ManagerID == null)
            {
                AssignCurrentManagerToOrder();
            }

            LoadComboBoxes();
            UpdateUIForRole();
        }

        private void AssignCurrentManagerToOrder()
        {
            // Получаем ID текущего пользователя (менеджера)
            int currentUserId = scr.core.Core.ThisUser_ID; // предполагается, что у вас есть такой глобальный ID

            using (SqlConnection conn = new SqlConnection(scr.core.SQL._sqlConnectStr))
            {
                conn.Open();
                using (var cmd = new SqlCommand("UPDATE SalesOrders SET ManagerID = @ManagerID WHERE ID = @OrderID", conn))
                {
                    cmd.Parameters.AddWithValue("@ManagerID", currentUserId);
                    cmd.Parameters.AddWithValue("@OrderID", orderId);
                    cmd.ExecuteNonQuery();
                }
            }

            // Обновляем локальный объект заказа
            order.ManagerID = currentUserId;


        }

        private void InitializeComponent_()
        {
            this.Text = "Редактирование заказа";
            this.StartPosition = FormStartPosition.CenterScreen;


            dgvOrderItems.ReadOnly = true;
            dgvOrderItems.AllowUserToAddRows = false;
            dgvOrderItems.AllowUserToDeleteRows = false;

            dgvOrderItems.Columns.Add("Name", "Название");
            dgvOrderItems.Columns.Add("Quantity", "Количество");
            dgvOrderItems.Columns.Add("Price", "Цена");
            dgvOrderItems.Columns.Add("Subtotal", "Итого");

            // Buttons
            btnSave.Click += BtnSave_Click;
        }

        private void LoadOrderData()
        {
            using (SqlConnection conn = new SqlConnection(scr.core.SQL._sqlConnectStr))
            {
                conn.Open();
                using (var cmd = new SqlCommand(@"
            SELECT s.ID, c.FullName AS ClientName, e1.FullName AS ManagerName, e2.FullName AS CookName,
                   s.OrderDate, s.CompletionDate, s.TotalAmount, s.Status, s.ManagerID
            FROM SalesOrders s
            LEFT JOIN Clients c ON s.ClientID = c.ID
            LEFT JOIN Employees e1 ON s.ManagerID = e1.ID
            LEFT JOIN Employees e2 ON s.CookID = e2.ID
            WHERE s.ID = @OrderID", conn))
                {
                    cmd.Parameters.AddWithValue("@OrderID", orderId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            order = new scr.core.dbt.DBT_SalesOrders
                            {
                                ID = reader.GetInt32("ID"),
                                Status = reader.GetString("Status"),
                                OrderDate = reader.GetDateTime("OrderDate"),
                                TotalAmount = reader.GetDecimal("TotalAmount")
                            };

                            if (!reader.IsDBNull("CompletionDate"))
                                order.CompletionDate = reader.GetDateTime("CompletionDate");
                            else
                                order.CompletionDate = null;

                            if (!reader.IsDBNull("ManagerID"))
                                order.ManagerID = reader.GetInt32("ManagerID");
                            else
                                order.ManagerID = null;

                            lblOrderNumber.Text = $"Заказ №{order.ID}";
                            lblClient.Text = $"Клиент: {reader["ClientName"]}";
                            lblManager.Text = $"Менеджер: {reader["ManagerName"] ?? "Не назначен"}";
                            lblCook.Text = $"Повар: {reader["CookName"] ?? "Не назначен"}";
                            lblOrderDate.Text = $"Дата создания: {order.OrderDate:dd.MM.yyyy HH:mm}";
                            lblCompletionDate.Text = $"Дата завершения: {(order.CompletionDate?.ToString("dd.MM.yyyy HH:mm") ?? "Не установлена")}";
                            lblTotalAmount.Text = $"Общая сумма: {order.TotalAmount:C}";
                            lblStatus.Text = $"Статус: {order.Status}";

                            LoadOrderItems(conn, orderId);
                        }
                    }
                }
            }
        }

        private void LoadOrderItems(SqlConnection conn_, int orderId)
        {
            dgvOrderItems.Rows.Clear();
            using (SqlConnection conn = new SqlConnection(scr.core.SQL._sqlConnectStr))
            {
                conn.Open();
                using (var cmd = new SqlCommand(@"
                SELECT p.Name, oi.Quantity, p.PricePerUnit, oi.Subtotal
                FROM OrderItems oi
                INNER JOIN Products p ON oi.ProductID = p.ID
                WHERE oi.OrderID = @OrderID", conn))
                {
                    cmd.Parameters.AddWithValue("@OrderID", orderId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var row = new DataGridViewRow();
                            row.CreateCells(dgvOrderItems);
                            row.Cells[0].Value = reader["Name"];
                            row.Cells[1].Value = reader["Quantity"];
                            row.Cells[2].Value = ((decimal)reader["PricePerUnit"]).ToString("C");
                            row.Cells[3].Value = ((decimal)reader["Subtotal"]).ToString("C");
                            dgvOrderItems.Rows.Add(row);
                        }
                    }
                }
            }
        }

        #region LoadComboBoxes
        private void LoadComboBoxes()
        {
            LoadComboBox_Managers();
            LoadComboBox_Cooks();
        }

        private void LoadComboBox_Managers()
        {
            managers.Clear();
            var allEmps = scr.core.dbt.DBT_Employees.GetAll();
            var allAuths = scr.core.dbt.DBT_Auth.GetAll();

            foreach (var emp in allEmps)
            {
                // Проверяем, не уволен ли сотрудник
                if (emp.HireDate != null && emp.HireDate.Value.Year == 1900)
                    continue;

                var auth = allAuths.FirstOrDefault(a => a.EmployeeID == emp.ID);
                if (auth != null && (auth.AccessLevel == 0 || auth.AccessLevel == 1)) // Админ или менеджер
                {
                    managers.Add(emp);
                }
            }

            cmbManager.Items.Clear();
            foreach (var emp in managers)
            {
                cmbManager.Items.Add($"{emp.FullName}");
            }
        }

        private void LoadComboBox_Cooks()
        {
            cooks.Clear();
            var allEmps = scr.core.dbt.DBT_Employees.GetAll();
            var allAuths = scr.core.dbt.DBT_Auth.GetAll();

            foreach (var emp in allEmps)
            {
                // Проверяем, не уволен ли сотрудник
                if (emp.HireDate != null && emp.HireDate.Value.Year == 1900)
                    continue;

                var auth = allAuths.FirstOrDefault(a => a.EmployeeID == emp.ID);
                if (auth != null && auth.AccessLevel == 2) // Повар
                {
                    cooks.Add(emp);
                }
            }

            cmbCook.Items.Clear();
            foreach (var emp in cooks)
            {
                cmbCook.Items.Add($"{emp.FullName}");
            }
        }
        #endregion

        private void UpdateUIForRole()
        {
            // Устанавливаем текущие значения
            using (SqlConnection conn = new SqlConnection(scr.core.SQL._sqlConnectStr))
            {
                conn.Open();
                using (var cmd = new SqlCommand("SELECT ManagerID, CookID FROM SalesOrders WHERE ID = @OrderID", conn))
                {
                    cmd.Parameters.AddWithValue("@OrderID", orderId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (!reader.IsDBNull("ManagerID"))
                            {
                                int mgrId = reader.GetInt32("ManagerID");
                                var mgrIndex = managers.FindIndex(m => m.ID == mgrId);
                                if (mgrIndex >= 0)
                                    cmbManager.SelectedIndex = mgrIndex;
                            }

                            if (!reader.IsDBNull("CookID"))
                            {
                                int cookId = reader.GetInt32("CookID");
                                var cookIndex = cooks.FindIndex(c => c.ID == cookId);
                                if (cookIndex >= 0)
                                    cmbCook.SelectedIndex = cookIndex;
                            }
                        }
                    }
                }
            }

            // Определяем доступные статусы
            var allowedStatuses = GetAllowedStatuses(order.Status);

            // Всегда добавляем текущий статус в список, если его там нет
            if (!allowedStatuses.Contains(order.Status))
                allowedStatuses.Insert(0, order.Status);

            cmbStatus.DataSource = allowedStatuses;

            // Устанавливаем текущий статус
            cmbStatus.SelectedItem = order.Status;

            // В зависимости от роли — показываем/скрываем элементы
            switch (userAccessLevel)
            {
                case 0: // Админ
                    cmbManager.Enabled = true;
                    cmbManager.Visible = true;
                    cmbCook.Enabled = true;
                    cmbCook.Visible = true;
                    cmbStatus.Enabled = true;
                    btnSave.Enabled = true;
                    break;

                case 1: // Менеджер
                    cmbManager.Enabled = false;
                    cmbCook.Visible = true;
                    cmbCook.Enabled = true;
                    cmbStatus.Enabled = true;
                    btnSave.Enabled = true;
                    break;

                case 2: // Повар
                    if (order.Status == "В процессе")
                    {
                        cmbStatus.Enabled = true;
                        cmbStatus.SelectedItem = "Готово";
                        btnSave.Enabled = true;
                    }
                    else
                    {
                        cmbStatus.Enabled = false;
                    }
                    break;

                default:
                    cmbManager.Visible = false;
                    cmbCook.Visible = false;
                    cmbStatus.Enabled = false;
                    btnSave.Enabled = false;
                    break;
            }

            // Повару не показываем лишние поля
            if (userAccessLevel == 2)
            {
                lblManager.Visible = false;
                lblCook.Visible = false;
                lblOrderDate.Visible = false;
                lblTotalAmount.Visible = false;
                lblClient.Visible = false;
                cmbManager.Visible = false;
                cmbCook.Visible = false;
            }

            if(order.Status == "Отменено" || order.Status == "Завершено")
            {
                cmbManager.Enabled = false;
                cmbCook.Enabled = false;
                cmbStatus.Enabled = false;
            }
        }

        private List<string> GetAllowedStatuses(string currentStatus)
        {
            var statuses = new List<string>();
            switch (currentStatus)
            {
                case "Новый":
                    statuses.Add("В процессе");
                    statuses.Add("Отменено");
                    break;
                case "В процессе":
                    statuses.Add("Готово");
                    statuses.Add("Отменено");
                    break;
                case "Готово":
                    statuses.Add("Завершено");
                    break;
                case "Завершено":
                case "Отменено":
                    // Нельзя менять
                    break;
            }
            return statuses;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            string newStatus = cmbStatus.SelectedItem?.ToString();
            scr.core.dbt.DBT_Employees selectedManager = null;
            scr.core.dbt.DBT_Employees selectedCook = null;

            if (cmbManager.SelectedIndex >= 0)
                selectedManager = managers[cmbManager.SelectedIndex];

            if (cmbCook.SelectedIndex >= 0)
                selectedCook = cooks[cmbCook.SelectedIndex];

            //int? newManagerId = userAccessLevel == 0 ? (selectedManager?.ID) : null;
            int? newManagerId = (selectedManager?.ID);
            int? newCookId = (userAccessLevel >= 0 && selectedCook != null) ? selectedCook.ID : null;

            // Проверки
            if (newStatus == "В процессе" && newCookId == null)
            {
                MessageBox.Show("Назначьте повара перед переходом в статус \"В процессе\".", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (newStatus == "В процессе" && order.Status != "В процессе")
            {
                if (!TryConsumeRawMaterials())
                {
                    return;
                }
            }

            if (newStatus == "Отменено" && order.Status == "В процессе")
            {
                RevertRawMaterials();
            }

            // Обновляем заказ
            using (SqlConnection conn = new SqlConnection(scr.core.SQL._sqlConnectStr))
            {
                conn.Open();
                using (var cmd = new SqlCommand(@"
                    UPDATE SalesOrders SET Status = @Status, ManagerID = @ManagerID, CookID = @CookID", conn))
                {
                    cmd.Parameters.AddWithValue("@Status", newStatus);
                    cmd.Parameters.AddWithValue("@ManagerID", newManagerId ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@CookID", newCookId ?? (object)DBNull.Value);

                    if (newStatus == "Завершено" || newStatus == "Отменено")
                    {
                        cmd.CommandText += ", CompletionDate = @CompletionDate";
                        cmd.Parameters.AddWithValue("@CompletionDate", DateTime.Now);
                    }

                    cmd.CommandText += " WHERE ID = @OrderID";
                    cmd.Parameters.AddWithValue("@OrderID", orderId);

                    cmd.ExecuteNonQuery();
                }
            }


            LoadOrderData(); // Обновляем отображение
            MessageBox.Show("Изменения сохранены.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);


            Close();
        }

        private bool TryConsumeRawMaterials()
        {
            using (SqlConnection conn = new SqlConnection(scr.core.SQL._sqlConnectStr))
            {
                conn.Open();

                // 1. Проверка наличия сырья
                var shortages = new List<string>();
                var rawMaterialQuantities = new List<(int rmId, string rmName, decimal stock, decimal needed)>();

                using (var cmd = new SqlCommand(@"
            SELECT rm.ID, rm.Name, rm.QuantityInStock, r.Quantity * oi.Quantity AS Needed
            FROM OrderItems oi
            INNER JOIN Products p ON oi.ProductID = p.ID
            INNER JOIN Recipes r ON p.ID = r.ProductID
            INNER JOIN RawMaterials rm ON r.RawMaterialID = rm.ID
            WHERE oi.OrderID = @OrderID", conn))
                {
                    cmd.Parameters.AddWithValue("@OrderID", orderId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int rmId = reader.GetInt32("ID");
                            string rmName = reader.GetString("Name");
                            decimal stock = reader.GetDecimal("QuantityInStock");
                            decimal needed = reader.GetDecimal("Needed");

                            if (stock < needed)
                            {
                                shortages.Add($"{rmName} (требуется {needed:F3}, доступно {stock:F3})");
                            }
                            else
                            {
                                rawMaterialQuantities.Add((rmId, rmName, stock, needed));
                            }
                        }
                    }
                }

                if (shortages.Any())
                {
                    MessageBox.Show("Недостаточно сырья:\n" + string.Join("\n", shortages), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                // 2. Списание сырья
                foreach (var (rmId, _, _, qty) in rawMaterialQuantities)
                {
                    using (var updateCmd = new SqlCommand("UPDATE RawMaterials SET QuantityInStock = QuantityInStock - @Qty WHERE ID = @RMID", conn))
                    {
                        updateCmd.Parameters.AddWithValue("@Qty", qty);
                        updateCmd.Parameters.AddWithValue("@RMID", rmId);
                        updateCmd.ExecuteNonQuery();
                    }
                }

                // 3. Проверка на низкий уровень
                CheckLowStockAlert(conn);
            }

            return true;
        }

        private void RevertRawMaterials()
        {
            using (SqlConnection conn = new SqlConnection(scr.core.SQL._sqlConnectStr))
            {
                conn.Open();

                var rawMaterialUpdates = new List<(int rmId, decimal qty)>();

                using (var cmd = new SqlCommand(@"
            SELECT rm.ID, r.Quantity * oi.Quantity AS Qty
            FROM OrderItems oi
            INNER JOIN Products p ON oi.ProductID = p.ID
            INNER JOIN Recipes r ON p.ID = r.ProductID
            INNER JOIN RawMaterials rm ON r.RawMaterialID = rm.ID
            WHERE oi.OrderID = @OrderID", conn))
                {
                    cmd.Parameters.AddWithValue("@OrderID", orderId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int rmId = reader.GetInt32("ID");
                            decimal qty = reader.GetDecimal("Qty");
                            rawMaterialUpdates.Add((rmId, qty));
                        }
                    }
                }

                foreach (var (rmId, qty) in rawMaterialUpdates)
                {
                    using (var updateCmd = new SqlCommand("UPDATE RawMaterials SET QuantityInStock = QuantityInStock + @Qty WHERE ID = @RMID", conn))
                    {
                        updateCmd.Parameters.AddWithValue("@Qty", qty);
                        updateCmd.Parameters.AddWithValue("@RMID", rmId);
                        updateCmd.ExecuteNonQuery();
                    }
                }
            }
        }

        private void CheckLowStockAlert(SqlConnection conn)
        {
            using (var cmd = new SqlCommand(@"
                SELECT Name FROM RawMaterials
                WHERE QuantityInStock < MinStockLevel", conn))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    var lowStockItems = new List<string>();
                    while (reader.Read())
                    {
                        lowStockItems.Add(reader.GetString("Name"));
                    }

                    if (lowStockItems.Any())
                    {
                        MessageBox.Show("Низкий уровень запаса:\n" + string.Join(", ", lowStockItems), "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

    }
}
