using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program.scr.core.dbt
{
    public class DBT_RawMaterials
    {
        public int ID;
        public string Name;
        public string UnitOfMeasure;
        public decimal MinStockLevel;
        public decimal QuantityInStock;


        public static List<DBT_RawMaterials> GetAll()
        {
            var objs = new List<DBT_RawMaterials>();
            try
            {
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
                                var obj = new DBT_RawMaterials();

                                obj.ID = reader.GetInt32(0);
                                obj.Name = reader.GetString(1);
                                obj.UnitOfMeasure = reader.GetString(2);
                                obj.MinStockLevel = reader.GetDecimal(3);
                                obj.QuantityInStock = reader.GetDecimal(4);

                                objs.Add(obj);
                            }
                        }
                    }
                }
            }
            catch { objs = null; }
            return objs;
        }

        public static DBT_RawMaterials GetById(int id)
        {
            var obj = new DBT_RawMaterials();
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var query = connection.CreateCommand())
                    {
                        query.CommandText = "SELECT * FROM RawMaterials WHERE ID = @id";
                        query.Parameters.AddWithValue("@id", id);
                        using (var reader = query.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                obj.ID = reader.GetInt32(0);
                                obj.Name = reader.GetString(1);
                                obj.UnitOfMeasure = reader.GetString(2);
                                obj.MinStockLevel = reader.GetDecimal(3);
                                obj.QuantityInStock = reader.GetDecimal(4);
                            }
                        }
                    }
                }
            }
            catch { obj = null; }
            return obj;
        }

        public static int Create(DBT_RawMaterials obj)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var query = connection.CreateCommand())
                    {
                        query.CommandText = "INSERT INTO RawMaterials VALUES (@Name, @UnitOfMeasure, @MinStockLevel, @QuantityInStock);";
                        query.Parameters.AddWithValue("@Name", obj.Name);
                        query.Parameters.AddWithValue("@UnitOfMeasure", obj.UnitOfMeasure);
                        query.Parameters.AddWithValue("@MinStockLevel", obj.MinStockLevel);
                        query.Parameters.AddWithValue("@QuantityInStock", obj.QuantityInStock);
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
                        query.CommandText = "SELECT MAX(ID) FROM RawMaterials;";
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

        public static int Edit(DBT_RawMaterials obj)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var query = connection.CreateCommand())
                    {
                        query.CommandText = "UPDATE RawMaterials SET Name = @Name, UnitOfMeasure = @UnitOfMeasure, MinStockLevel = @MinStockLevel, QuantityInStock = @QuantityInStock WHERE ID = @id;";
                        query.Parameters.AddWithValue("@Name", obj.Name);
                        query.Parameters.AddWithValue("@UnitOfMeasure", obj.UnitOfMeasure);
                        query.Parameters.AddWithValue("@MinStockLevel", obj.MinStockLevel);
                        query.Parameters.AddWithValue("@QuantityInStock", obj.QuantityInStock);
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
                        query.CommandText = "DELETE FROM RawMaterials WHERE ID = @id;";
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
