using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;

namespace Program
{
    public class DBAItem
    {
        public List<ItemData> GetAllItems()
        {
            List<ItemData> items = new List<ItemData>();
            SqlConnection conn = new SqlConnection(
                Properties.Settings.Default.COMP4150DatabaseConnectionString);

            try
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM Item";

                conn.Open();
                Debug.WriteLine("Connection opened successfully");

                SqlDataReader reader = cmd.ExecuteReader();
                int count = 0;
                while (reader.Read())
                {
                    ItemData item = new ItemData
                    {
                        ItemID = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Description = reader.IsDBNull(2) ? "" : reader.GetString(2),
                        Price = reader.GetDecimal(3)
                        
                    };

                    items.Add(item);
                    count++;
                    Debug.WriteLine("Loaded item: " + item.Name);
                }
                Debug.WriteLine("Total items loaded: " + count);

                conn.Close();
                return items;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("DB ERROR: " + ex.ToString());
                return new List<ItemData>();
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
        }

    }
}
