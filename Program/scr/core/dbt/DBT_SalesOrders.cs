using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program.scr.core.dbt
{
    public class DBT_SalesOrders
    {
        public int ID;
        public int? ClientID;
        public int? ManagerID;
        public int? CookID;
        public DateTime OrderDate;
        public DateTime? CompletionDate;
        public decimal TotalAmount;
        public string Status;


        public static List<DBT_SalesOrders> GetAll()
        {
            var objs = new List<DBT_SalesOrders>();
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var query = connection.CreateCommand())
                    {
                        query.CommandText = "SELECT * FROM SalesOrders";
                        using (var reader = query.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var obj = new DBT_SalesOrders();

                                obj.ID = reader.GetInt32(0);
                                if (reader.IsDBNull(1)) obj.ClientID = null;
                                else obj.ClientID = reader.GetInt32(1);
                                if (reader.IsDBNull(2)) obj.ManagerID = null;
                                else obj.ManagerID = reader.GetInt32(2);
                                if (reader.IsDBNull(3)) obj.CookID = null;
                                else obj.CookID = reader.GetInt32(3);
                                obj.OrderDate = DateTime.Parse(reader.GetValue(4).ToString());
                                if (reader.IsDBNull(5)) obj.CompletionDate = null;
                                else obj.CompletionDate = DateTime.Parse(reader.GetValue(5).ToString());
                                obj.TotalAmount = reader.GetDecimal(6);
                                obj.Status = reader.GetString(7);

                                objs.Add(obj);
                            }
                        }
                    }
                }
            }
            catch { objs = null; }
            return objs;
        }

        public static DBT_SalesOrders GetById(int id)
        {
            var obj = new DBT_SalesOrders();
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var query = connection.CreateCommand())
                    {
                        query.CommandText = "SELECT * FROM SalesOrders WHERE ID = @id";
                        query.Parameters.AddWithValue("@id", id);
                        using (var reader = query.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                obj.ID = reader.GetInt32(0);
                                if (reader.IsDBNull(1)) obj.ClientID = null;
                                else obj.ClientID = reader.GetInt32(1);
                                if (reader.IsDBNull(2)) obj.ManagerID = null;
                                else obj.ManagerID = reader.GetInt32(2);
                                if (reader.IsDBNull(3)) obj.CookID = null;
                                else obj.CookID = reader.GetInt32(3);
                                obj.OrderDate = DateTime.Parse(reader.GetValue(4).ToString());
                                if (reader.IsDBNull(5)) obj.CompletionDate = null;
                                else obj.CompletionDate = DateTime.Parse(reader.GetValue(5).ToString());
                                obj.TotalAmount = reader.GetDecimal(6);
                                obj.Status = reader.GetString(7);
                            }
                        }
                    }
                }
            }
            catch { obj = null; }
            return obj;
        }

        public static int Create(DBT_SalesOrders obj)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var query = connection.CreateCommand())
                    {
                        query.CommandText = "INSERT INTO SalesOrders VALUES (@ClientID, @ManagerID, @CookID, @OrderDate, @CompletionDate, @TotalAmount, @Status);";
                        query.Parameters.AddWithValue("@ClientID", obj.ClientID);
                        query.Parameters.AddWithValue("@ManagerID", obj.ManagerID);
                        query.Parameters.AddWithValue("@CookID", obj.CookID);
                        query.Parameters.AddWithValue("@OrderDate", obj.OrderDate);
                        query.Parameters.AddWithValue("@CompletionDate", obj.CompletionDate);
                        query.Parameters.AddWithValue("@TotalAmount", obj.TotalAmount);
                        query.Parameters.AddWithValue("@Status", obj.Status);
                        query.ExecuteNonQuery();
                    }
                }
            }
            catch { return -1; }
            int _id = -1;
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var query = connection.CreateCommand())
                    {
                        query.CommandText = "SELECT MAX(ID) FROM SalesOrders;";
                        using (var reader = query.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                _id = reader.GetInt32(0);
                            }
                        }
                    }
                }
            }
            catch { return -1; }
            return _id;
        }

        public static int Edit(DBT_SalesOrders obj)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var query = connection.CreateCommand())
                    {
                        query.CommandText = "UPDATE SalesOrders SET ClientID = @ClientID, ManagerID = @ManagerID, CookID = @CookID, OrderDate = @OrderDate, CompletionDate = @CompletionDate, TotalAmount = @TotalAmount, Status = @Status WHERE ID = @id;";
                        query.Parameters.AddWithValue("@ClientID", obj.ClientID);
                        query.Parameters.AddWithValue("@ManagerID", obj.ManagerID);
                        query.Parameters.AddWithValue("@CookID", obj.CookID);
                        query.Parameters.AddWithValue("@OrderDate", obj.OrderDate);
                        query.Parameters.AddWithValue("@CompletionDate", obj.CompletionDate);
                        query.Parameters.AddWithValue("@TotalAmount", obj.TotalAmount);
                        query.Parameters.AddWithValue("@Status", obj.Status);
                        query.Parameters.AddWithValue("@id", obj.ID);
                        query.ExecuteNonQuery();
                    }
                }
            }
            catch { return -1; }
            return 0;
        }

        public static int Remove(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var query = connection.CreateCommand())
                    {
                        query.CommandText = "DELETE FROM SalesOrders WHERE ID = @id;";
                        query.Parameters.AddWithValue("@id", id);
                        query.ExecuteNonQuery();
                    }
                }
            }
            catch { return -1; }
            return 0;
        }

    }
}
