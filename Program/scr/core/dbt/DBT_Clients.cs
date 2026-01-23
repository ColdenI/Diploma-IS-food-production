using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program.scr.core.dbt
{
    public class DBT_Clients
    {
        public int ID;
        public string FullName;
        public string? ContactPerson;
        public string? Phone;
        public string? Email;


        public static List<DBT_Clients> GetAll()
        {
            var objs = new List<DBT_Clients>();
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var query = connection.CreateCommand())
                    {
                        query.CommandText = "SELECT * FROM Clients";
                        using (var reader = query.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var obj = new DBT_Clients();

                                obj.ID = reader.GetInt32(0);
                                obj.FullName = reader.GetString(1);
                                if (reader.IsDBNull(2)) obj.ContactPerson = string.Empty;
                                else obj.ContactPerson = reader.GetString(2);
                                if (reader.IsDBNull(3)) obj.Phone = string.Empty;
                                else obj.Phone = reader.GetString(3);
                                if (reader.IsDBNull(4)) obj.Email = string.Empty;
                                else obj.Email = reader.GetString(4);

                                objs.Add(obj);
                            }
                        }
                    }
                }
            }
            catch { objs = null; }
            return objs;
        }

        public static DBT_Clients GetById(int id)
        {
            var obj = new DBT_Clients();
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var query = connection.CreateCommand())
                    {
                        query.CommandText = "SELECT * FROM Clients WHERE ID = @id";
                        query.Parameters.AddWithValue("@id", id);
                        using (var reader = query.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                obj.ID = reader.GetInt32(0);
                                obj.FullName = reader.GetString(1);
                                if (reader.IsDBNull(2)) obj.ContactPerson = string.Empty;
                                else obj.ContactPerson = reader.GetString(2);
                                if (reader.IsDBNull(3)) obj.Phone = string.Empty;
                                else obj.Phone = reader.GetString(3);
                                if (reader.IsDBNull(4)) obj.Email = string.Empty;
                                else obj.Email = reader.GetString(4);
                            }
                        }
                    }
                }
            }
            catch { obj = null; }
            return obj;
        }

        public static DBT_Clients GetByEmail(string id)
        {
            var obj = new DBT_Clients();
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var query = connection.CreateCommand())
                    {
                        query.CommandText = "SELECT * FROM Clients WHERE Email = @id";
                        query.Parameters.AddWithValue("@id", id);
                        using (var reader = query.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                obj.ID = reader.GetInt32(0);
                                obj.FullName = reader.GetString(1);
                                if (reader.IsDBNull(2)) obj.ContactPerson = string.Empty;
                                else obj.ContactPerson = reader.GetString(2);
                                if (reader.IsDBNull(3)) obj.Phone = string.Empty;
                                else obj.Phone = reader.GetString(3);
                                if (reader.IsDBNull(4)) obj.Email = string.Empty;
                                else obj.Email = reader.GetString(4);

                                
                            }
                        }
                    }
                    if (obj.Email == null) return null;
                }
            }
            catch { obj = null; }
            return obj;
        }

        public static int Create(DBT_Clients obj)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var query = connection.CreateCommand())
                    {
                        query.CommandText = "INSERT INTO Clients VALUES (@FullName, @ContactPerson, @Phone, @Email);";
                        query.Parameters.AddWithValue("@FullName", obj.FullName);
                        query.Parameters.AddWithValue("@ContactPerson", obj.ContactPerson);
                        query.Parameters.AddWithValue("@Phone", obj.Phone);
                        query.Parameters.AddWithValue("@Email", obj.Email);
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
                        query.CommandText = "SELECT MAX(ID) FROM Clients;";
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

        public static int Edit(DBT_Clients obj)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var query = connection.CreateCommand())
                    {
                        query.CommandText = "UPDATE Clients SET FullName = @FullName, ContactPerson = @ContactPerson, Phone = @Phone, Email = @Email WHERE ID = @id;";
                        query.Parameters.AddWithValue("@FullName", obj.FullName);
                        query.Parameters.AddWithValue("@ContactPerson", obj.ContactPerson);
                        query.Parameters.AddWithValue("@Phone", obj.Phone);
                        query.Parameters.AddWithValue("@Email", obj.Email);
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
                        query.CommandText = "DELETE FROM Clients WHERE ID = @id;";
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
