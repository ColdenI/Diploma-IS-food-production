using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program.scr.core.dbt
{
    public class DBT_Products
    {
        public int ID;
        public string Name;
        public string Category;
        public string UnitOfMeasure;
        public int ShelfLifeDays;
        public decimal PricePerUnit;


        public static List<DBT_Products> GetAll()
        {
            var objs = new List<DBT_Products>();
            try
            {
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
                                var obj = new DBT_Products();

                                obj.ID = reader.GetInt32(0);
                                obj.Name = reader.GetString(1);
                                obj.Category = reader.GetString(2);
                                obj.UnitOfMeasure = reader.GetString(3);
                                obj.ShelfLifeDays = reader.GetInt32(4);
                                obj.PricePerUnit = reader.GetDecimal(5);

                                objs.Add(obj);
                            }
                        }
                    }
                }
            }
            catch { objs = null; }
            return objs;
        }

        public static DBT_Products GetById(int id)
        {
            var obj = new DBT_Products();
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var query = connection.CreateCommand())
                    {
                        query.CommandText = "SELECT * FROM Products WHERE ID = @id";
                        query.Parameters.AddWithValue("@id", id);
                        using (var reader = query.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                obj.ID = reader.GetInt32(0);
                                obj.Name = reader.GetString(1);
                                obj.Category = reader.GetString(2);
                                obj.UnitOfMeasure = reader.GetString(3);
                                obj.ShelfLifeDays = reader.GetInt32(4);
                                obj.PricePerUnit = reader.GetDecimal(5);
                            }
                        }
                    }
                }
            }
            catch { obj = null; }
            return obj;
        }

        public static int Create(DBT_Products obj)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var query = connection.CreateCommand())
                    {
                        query.CommandText = "INSERT INTO Products VALUES (@Name, @Category, @UnitOfMeasure, @ShelfLifeDays, @PricePerUnit);";
                        query.Parameters.AddWithValue("@Name", obj.Name);
                        query.Parameters.AddWithValue("@Category", obj.Category);
                        query.Parameters.AddWithValue("@UnitOfMeasure", obj.UnitOfMeasure);
                        query.Parameters.AddWithValue("@ShelfLifeDays", obj.ShelfLifeDays);
                        query.Parameters.AddWithValue("@PricePerUnit", obj.PricePerUnit);
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
                        query.CommandText = "SELECT MAX(ID) FROM Products;";
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

        public static int Edit(DBT_Products obj)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var query = connection.CreateCommand())
                    {
                        query.CommandText = "UPDATE Products SET Name = @Name, Category = @Category, UnitOfMeasure = @UnitOfMeasure, ShelfLifeDays = @ShelfLifeDays, PricePerUnit = @PricePerUnit WHERE ID = @id;";
                        query.Parameters.AddWithValue("@Name", obj.Name);
                        query.Parameters.AddWithValue("@Category", obj.Category);
                        query.Parameters.AddWithValue("@UnitOfMeasure", obj.UnitOfMeasure);
                        query.Parameters.AddWithValue("@ShelfLifeDays", obj.ShelfLifeDays);
                        query.Parameters.AddWithValue("@PricePerUnit", obj.PricePerUnit);
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
                        query.CommandText = "DELETE FROM Products WHERE ID = @id;";
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
