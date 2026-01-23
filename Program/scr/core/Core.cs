using Microsoft.Data.SqlClient;
using System.Data;

namespace Program.scr.core
{
    public static class Core
    {
        public static int ThisUser_ID = -1;
        public static int ThisUser_AC = -1;

        public static string[] ACs = { "Нет доступа", "Админ", "Менеджер", "Повар" };

        public static Dictionary<int, decimal> Get_RawMaterialsIdAndQuantity_ByProductId(int productId)
        {
            var objs = new Dictionary<int, decimal>();

            try
            {
                using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var query = connection.CreateCommand())
                    {
                        query.CommandText = "SELECT RawMaterialID, Quantity FROM Recipes WHERE ProductID = @id";
                        query.Parameters.AddWithValue("@id", productId);
                        using (var reader = query.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                objs.Add(reader.GetInt32(0), reader.GetDecimal(1));
                            }
                        }
                    }
                }
            }
            catch { objs = null; }
            return objs;
        }

        public static int SaveOrder(int clientId, Dictionary<int, int> orderDict, bool isManager = false)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            int orderId = -1;
                            int? managerId = null;

                            if (isManager) managerId = Core.ThisUser_ID;
                            else managerId = GetLeastBusyManager(connection, transaction);

                            if (managerId == null)
                            {
                                throw new Exception("Нет доступных менеджеров для назначения.");
                            }

                            // 1. Создаём заказ в SalesOrders
                            using (var cmd = new SqlCommand(
                                @"INSERT INTO SalesOrders (ClientID, ManagerID, OrderDate, TotalAmount, Status) 
                                  OUTPUT INSERTED.ID
                                  VALUES (@ClientID, @ManagerID, @OrderDate, @TotalAmount, @Status)", connection, transaction))
                            {
                                decimal total = 0;

                                // Предварительно вычисляем сумму
                                foreach (var item in orderDict)
                                {
                                    int productId = item.Key;
                                    int quantity = item.Value;

                                    using (var priceCmd = new SqlCommand(
                                        "SELECT PricePerUnit FROM Products WHERE ID = @ProductID", connection, transaction))
                                    {
                                        priceCmd.Parameters.AddWithValue("@ProductID", productId);
                                        var result = priceCmd.ExecuteScalar();
                                        if (result != null)
                                        {
                                            decimal price = Convert.ToDecimal(result);
                                            total += price * quantity;
                                        }
                                        else
                                        {
                                            throw new Exception($"Продукт с ID {productId} не найден.");
                                        }
                                    }
                                }

                                cmd.Parameters.AddWithValue("@ClientID", clientId);
                                cmd.Parameters.AddWithValue("@ManagerID", managerId);
                                cmd.Parameters.AddWithValue("@OrderDate", DateTime.Now);
                                cmd.Parameters.AddWithValue("@TotalAmount", total);
                                cmd.Parameters.AddWithValue("@Status", "Новый");

                                orderId = (int)cmd.ExecuteScalar();
                            }

                            if (orderId == -1) throw new Exception("Не удалось создать заказ.");

                            // 2. Создаём позиции заказа в OrderItems
                            foreach (var item in orderDict)
                            {
                                int productId = item.Key;
                                int quantity = item.Value;

                                using (var cmdItem = new SqlCommand(
                                    @"INSERT INTO OrderItems (OrderID, ProductID, Quantity, Subtotal)
                                      VALUES (@OrderID, @ProductID, @Quantity, @Subtotal)", connection, transaction))
                                {
                                    using (var priceCmd = new SqlCommand(
                                        "SELECT PricePerUnit FROM Products WHERE ID = @ProductID", connection, transaction))
                                    {
                                        priceCmd.Parameters.AddWithValue("@ProductID", productId);
                                        decimal price = Convert.ToDecimal(priceCmd.ExecuteScalar());
                                        decimal subtotal = price * quantity;

                                        cmdItem.Parameters.AddWithValue("@OrderID", orderId);
                                        cmdItem.Parameters.AddWithValue("@ProductID", productId);
                                        cmdItem.Parameters.AddWithValue("@Quantity", quantity);
                                        cmdItem.Parameters.AddWithValue("@Subtotal", subtotal);

                                        cmdItem.ExecuteNonQuery();
                                    }
                                }
                            }

                            transaction.Commit();
                            return orderId;
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch
            {
                return -1;
            }
        }

        private static int? GetLeastBusyManager(SqlConnection conn, SqlTransaction trans)
        {
            using (var cmd = new SqlCommand(@"
                SELECT e.ID
                FROM Employees e
                LEFT JOIN (
                    SELECT ManagerID, COUNT(*) AS ActiveOrders
                    FROM SalesOrders
                    WHERE Status IN ('Новый', 'В процессе')
                    GROUP BY ManagerID
                ) o ON e.ID = o.ManagerID
                WHERE e.Position = 'Менеджер'
                ORDER BY ISNULL(o.ActiveOrders, 0) ASC
                ", conn, trans))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return reader.GetInt32(0);
                    }
                }
            }
            return null; // Нет доступных менеджеров
        }

        public static class Auth
        {
            public static AuthResult? ValidateCredentials(string login, string password)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(scr.core.SQL._sqlConnectStr))
                    {
                        connection.Open();
                        using (var cmd = new SqlCommand(@"
                SELECT a.EmployeeID, a.AccessLevel, e.HireDate
                FROM Auth a
                INNER JOIN Employees e ON a.EmployeeID = e.ID
                WHERE a.Login = @Login AND a.PasswordHash = @Password", connection))
                        {
                            cmd.Parameters.AddWithValue("@Login", login);
                            cmd.Parameters.AddWithValue("@Password", password);

                            using (var reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    int employeeId = reader.GetInt32("EmployeeID");
                                    int accessLevel = reader.GetInt32("AccessLevel");
                                    DateTime hireDate = reader.GetDateTime("HireDate");

                                    return new AuthResult(employeeId, accessLevel, hireDate);
                                }
                            }
                        }
                    }
                }
                catch
                {
                    // В реальной системе логгируйте ошибку
                }
                return null;
            }

            // Вспомогательная структура
            public struct AuthResult
            {
                public int EmployeeID;
                public int AccessLevel;
                public DateTime HireDate;

                public AuthResult(int employeeId, int accessLevel, DateTime hireDate)
                {
                    EmployeeID = employeeId;
                    AccessLevel = accessLevel;
                    HireDate = hireDate;
                }
            }
        }
    }
}
