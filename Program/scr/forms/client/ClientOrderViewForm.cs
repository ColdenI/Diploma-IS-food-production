using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Program.scr.forms.client
{
    public partial class ClientOrderViewForm : Form
    {
        public ClientOrderViewForm(string email, int orderId)
        {
            InitializeComponent();

            dgvOrderItems.AllowUserToAddRows = false;
            dgvOrderItems.AllowUserToDeleteRows = false;
            dgvOrderItems.ReadOnly = true;
            dgvOrderItems.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dgvOrderItems.Columns.Add("Name", "Название");
            dgvOrderItems.Columns.Add("Price", "Цена");
            dgvOrderItems.Columns.Add("Quantity", "Количество");
            dgvOrderItems.Columns.Add("Subtotal", "Итого");



            LoadOrderDetails(email, orderId);
        }

        private void LoadOrderDetails(string email, int orderId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(scr.core.SQL._sqlConnectStr))
                {
                    connection.Open();

                    // 1. Проверяем, существует ли клиент с таким email
                    int? clientId = GetClientIdByEmail(connection, email);
                    if (clientId == null)
                    {
                        MessageBox.Show("Клиент с таким email не найден.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        this.Close();
                        return;
                    }

                    // 2. Проверяем, принадлежит ли заказ этому клиенту
                    OrderInfo? orderInfo = GetOrderInfoByIdAndClientId(connection, orderId, clientId.Value);
                    if (orderInfo == null)
                    {
                        MessageBox.Show("Заказ не найден или не принадлежит данному клиенту.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        this.Close();
                        return;
                    }

                    // 3. Если всё ок — отображаем детали
                    lblTotalAmount.Text = $"Общая сумма: {orderInfo.Value.TotalAmount:C}";
                    lblOrderDate.Text = $"Дата заказа: {orderInfo.Value.OrderDate:dd.MM.yyyy HH:mm}";

                    label1.Text = $"Статус: {orderInfo.Value.Status}";

                    // Загружаем состав заказа
                    LoadOrderItems(connection, orderId);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private int? GetClientIdByEmail(SqlConnection conn, string email)
        {
            using (var cmd = new SqlCommand("SELECT ID FROM Clients WHERE Email = @Email", conn))
            {
                cmd.Parameters.AddWithValue("@Email", email);
                var result = cmd.ExecuteScalar();
                return result != null ? (int)result : (int?)null;
            }
        }

        private OrderInfo? GetOrderInfoByIdAndClientId(SqlConnection conn, int orderId, int clientId)
        {
            using (var cmd = new SqlCommand(@"
                SELECT TotalAmount, OrderDate, Status
                FROM SalesOrders
                WHERE ID = @OrderId AND ClientID = @ClientId", conn))
            {
                cmd.Parameters.AddWithValue("@OrderId", orderId);
                cmd.Parameters.AddWithValue("@ClientId", clientId);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new OrderInfo(
                            reader.GetDecimal("TotalAmount"),
                            reader.GetDateTime("OrderDate"),
                            reader.GetString("Status")
                        );
                    }
                }
            }
            return null;
        }

        public struct OrderInfo
        {
            public decimal TotalAmount;
            public string Status;
            public DateTime OrderDate;

            public OrderInfo(decimal totalAmount, DateTime orderDate, string status)
            {
                TotalAmount = totalAmount;
                OrderDate = orderDate;
                Status = status;
            }
        }

        private void LoadOrderItems(SqlConnection connection, int orderId)
        {
            dgvOrderItems.Rows.Clear();

            using (var cmd = new SqlCommand(@"
                SELECT p.Name, p.PricePerUnit, oi.Quantity, oi.Subtotal
                FROM OrderItems oi
                INNER JOIN Products p ON oi.ProductID = p.ID
                WHERE oi.OrderID = @OrderId", connection))
            {
                cmd.Parameters.AddWithValue("@OrderId", orderId);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var row = new DataGridViewRow();
                        row.CreateCells(dgvOrderItems);

                        row.Cells[0].Value = reader["Name"];
                        row.Cells[1].Value = ((decimal)reader["PricePerUnit"]).ToString("C");
                        row.Cells[2].Value = reader["Quantity"].ToString();
                        row.Cells[3].Value = ((decimal)reader["Subtotal"]).ToString("C");

                        dgvOrderItems.Rows.Add(row);
                    }
                }
            }
        }
    }
}
