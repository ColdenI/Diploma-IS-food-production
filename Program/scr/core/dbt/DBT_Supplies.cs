using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program.scr.core.dbt
{
    public class DBT_Supplies
    {
        public int ID;
        public int? SupplierID;
        public int RawMaterialID;
        public int? EmployeeID;
        public decimal Quantity;
        public DateTime SupplyDate;


        public static List<DBT_Supplies> GetAll()
        {
            var objs = new List<DBT_Supplies>();
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var query = connection.CreateCommand())
                    {
                        query.CommandText = "SELECT * FROM Supplies";
                        using (var reader = query.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var obj = new DBT_Supplies();

                                obj.ID = reader.GetInt32(0);
                                if (reader.IsDBNull(1)) obj.SupplierID = null;
                                else obj.SupplierID = reader.GetInt32(1);
                                obj.RawMaterialID = reader.GetInt32(2);
                                if (reader.IsDBNull(3)) obj.EmployeeID = null;
                                else obj.EmployeeID = reader.GetInt32(3);
                                obj.Quantity = reader.GetDecimal(4);
                                obj.SupplyDate = DateTime.Parse(reader.GetValue(5).ToString());

                                objs.Add(obj);
                            }
                        }
                    }
                }
            }
            catch { objs = null; }
            return objs;
        }

        public static DBT_Supplies GetById(int id)
        {
            var obj = new DBT_Supplies();
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var query = connection.CreateCommand())
                    {
                        query.CommandText = "SELECT * FROM Supplies WHERE ID = @id";
                        query.Parameters.AddWithValue("@id", id);
                        using (var reader = query.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                obj.ID = reader.GetInt32(0);
                                if (reader.IsDBNull(1)) obj.SupplierID = null;
                                else obj.SupplierID = reader.GetInt32(1);
                                obj.RawMaterialID = reader.GetInt32(2);
                                if (reader.IsDBNull(3)) obj.EmployeeID = null;
                                else obj.EmployeeID = reader.GetInt32(3);
                                obj.Quantity = reader.GetDecimal(4);
                                obj.SupplyDate = DateTime.Parse(reader.GetValue(5).ToString());
                            }
                        }
                    }
                }
            }
            catch { obj = null; }
            return obj;
        }

        public static int Create(DBT_Supplies obj)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var query = connection.CreateCommand())
                    {
                        query.CommandText = "INSERT INTO Supplies VALUES (@SupplierID, @RawMaterialID, @EmployeeID, @Quantity, @SupplyDate);";
                        query.Parameters.AddWithValue("@SupplierID", obj.SupplierID);
                        query.Parameters.AddWithValue("@RawMaterialID", obj.RawMaterialID);
                        query.Parameters.AddWithValue("@EmployeeID", obj.EmployeeID);
                        query.Parameters.AddWithValue("@Quantity", obj.Quantity);
                        query.Parameters.AddWithValue("@SupplyDate", obj.SupplyDate);
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
                        query.CommandText = "SELECT MAX(ID) FROM Supplies;";
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

        public static int Edit(DBT_Supplies obj)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var query = connection.CreateCommand())
                    {
                        query.CommandText = "UPDATE Supplies SET SupplierID = @SupplierID, RawMaterialID = @RawMaterialID, EmployeeID = @EmployeeID, Quantity = @Quantity, SupplyDate = @SupplyDate WHERE ID = @id;";
                        query.Parameters.AddWithValue("@SupplierID", obj.SupplierID);
                        query.Parameters.AddWithValue("@RawMaterialID", obj.RawMaterialID);
                        query.Parameters.AddWithValue("@EmployeeID", obj.EmployeeID);
                        query.Parameters.AddWithValue("@Quantity", obj.Quantity);
                        query.Parameters.AddWithValue("@SupplyDate", obj.SupplyDate);
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
                        query.CommandText = "DELETE FROM Supplies WHERE ID = @id;";
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
