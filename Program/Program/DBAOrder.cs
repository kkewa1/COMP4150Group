using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace Program
{
    public class DBAOrder
    {
        private string connStr => Properties.Settings.Default.COMP4150DatabaseConnectionString;

        public int CreateOrder(int tableID, int staffID)
        {
            SqlConnection conn = new SqlConnection(connStr);
            SqlCommand cmd = new SqlCommand("CreateOrder", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@tableID", tableID);
            cmd.Parameters.AddWithValue("@staffID", staffID);

            SqlParameter returnParam = new SqlParameter("@orderID", SqlDbType.Int)
            {
                Direction = ParameterDirection.ReturnValue
            };
            cmd.Parameters.Add(returnParam);

            conn.Open();
            cmd.ExecuteNonQuery();

            return (int)returnParam.Value;
        }

        public void AddItemToOrder(int itemID, int orderID, int quantity)
        {
            SqlConnection conn = new SqlConnection(connStr);
            SqlCommand cmd = new SqlCommand("AddItemToOrder", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@itemID", itemID);
            cmd.Parameters.AddWithValue("@orderID", orderID);
            cmd.Parameters.AddWithValue("@quantity", quantity);

            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
