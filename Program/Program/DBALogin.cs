using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    public class DBALogin
    {
        public UserData Login(string username, string password)
        {
            SqlConnection conn = new SqlConnection(
                Properties.Settings.Default.COMP4150DatabaseConnectionString);
            try
            {
                UserData userData = new UserData();

                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM Staff WHERE Name = @username AND Password = @password";
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);
                conn.Open();
                var reader = cmd.ExecuteReader();
                reader.Read();
                userData.ID = reader.GetInt32(0);
                userData.username = reader.GetString(1);
                userData.password = reader.GetInt32(2);
                userData.staffType = reader.GetString(3);
                conn.Close();
                return userData;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return new UserData();
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }
    }
}
