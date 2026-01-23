using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program.scr.core.dbt
{
    public class DBT_OrderItems
    {
        public int ID;
        public int OrderID;
        public int ProductID;
        public decimal Quantity;
        public decimal Subtotal;


        public static List<DBT_OrderItems> GetAll()
        {
            var objs = new List<DBT_OrderItems>();
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var query = connection.CreateCommand())
                    {
                        query.CommandText = "SELECT * FROM OrderItems";
                        using (var reader = query.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var obj = new DBT_OrderItems();

                                obj.ID = reader.GetInt32(0);
                                obj.OrderID = reader.GetInt32(1);
                                obj.ProductID = reader.GetInt32(2);
                                obj.Quantity = reader.GetDecimal(3);
                                obj.Subtotal = reader.GetDecimal(4);

                                objs.Add(obj);
                            }
                        }
                    }
                }
            }
            catch { objs = null; }
            return objs;
        }

        public static DBT_OrderItems GetById(int id)
        {
            var obj = new DBT_OrderItems();
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var query = connection.CreateCommand())
                    {
                        query.CommandText = "SELECT * FROM OrderItems WHERE ID = @id";
                        query.Parameters.AddWithValue("@id", id);
                        using (var reader = query.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                obj.ID = reader.GetInt32(0);
                                obj.OrderID = reader.GetInt32(1);
                                obj.ProductID = reader.GetInt32(2);
                                obj.Quantity = reader.GetDecimal(3);
                                obj.Subtotal = reader.GetDecimal(4);
                            }
                        }
                    }
                }
            }
            catch { obj = null; }
            return obj;
        }

        public static int Create(DBT_OrderItems obj)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var query = connection.CreateCommand())
                    {
                        query.CommandText = "INSERT INTO OrderItems VALUES (@OrderID, @ProductID, @Quantity, @Subtotal);";
                        query.Parameters.AddWithValue("@OrderID", obj.OrderID);
                        query.Parameters.AddWithValue("@ProductID", obj.ProductID);
                        query.Parameters.AddWithValue("@Quantity", obj.Quantity);
                        query.Parameters.AddWithValue("@Subtotal", obj.Subtotal);
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
                        query.CommandText = "SELECT MAX(ID) FROM OrderItems;";
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

        public static int Edit(DBT_OrderItems obj)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var query = connection.CreateCommand())
                    {
                        query.CommandText = "UPDATE OrderItems SET OrderID = @OrderID, ProductID = @ProductID, Quantity = @Quantity, Subtotal = @Subtotal WHERE ID = @id;";
                        query.Parameters.AddWithValue("@OrderID", obj.OrderID);
                        query.Parameters.AddWithValue("@ProductID", obj.ProductID);
                        query.Parameters.AddWithValue("@Quantity", obj.Quantity);
                        query.Parameters.AddWithValue("@Subtotal", obj.Subtotal);
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
                        query.CommandText = "DELETE FROM OrderItems WHERE ID = @id;";
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
