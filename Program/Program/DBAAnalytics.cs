using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;

namespace Program
{
    public class DBAAnalytics
    {
        public List<ItemAnalyticsData> GetItemAnalytics()
        {
            List<ItemAnalyticsData> items = new List<ItemAnalyticsData>();
            SqlConnection conn = new SqlConnection(Properties.Settings.Default.COMP4150DatabaseConnectionString);

            try
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM ItemAnalytics";

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    items.Add(new ItemAnalyticsData
                    {
                        ItemID = reader.GetInt32(0),
                        Year = reader.GetInt32(1),
                        Month = reader.GetString(2),
                        Revenue = reader.GetDecimal(3),
                        TimesOrdered = reader.GetInt32(4)
                    });
                }

                return items;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("DB ERROR (ItemAnalytics): " + ex.ToString());
                return new List<ItemAnalyticsData>();
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
        }

        public List<TableAnalyticsData> GetTableAnalytics()
        {
            List<TableAnalyticsData> tables = new List<TableAnalyticsData>();
            SqlConnection conn = new SqlConnection(Properties.Settings.Default.COMP4150DatabaseConnectionString);

            try
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM TableAnalytics";

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    tables.Add(new TableAnalyticsData
                    {
                        TableID = reader.GetInt32(0),
                        Year = reader.GetInt32(1),
                        Month = reader.GetString(2),
                        Revenue = reader.GetDecimal(3),
                        TimesUsed = reader.GetInt32(4)
                    });
                }

                return tables;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("DB ERROR (TableAnalytics): " + ex.ToString());
                return new List<TableAnalyticsData>();
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
        }
    }
}
