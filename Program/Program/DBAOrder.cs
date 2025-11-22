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

        public DataTable GetRecentOrders(int take = 100)
        {
            SqlConnection conn = new SqlConnection(connStr);
            SqlCommand cmd = new SqlCommand(@"
                SELECT TOP (@take)
                       OrderID, TableID, StaffID, OrderStatus, PaymentStatus
                FROM dbo.Orders
                ORDER BY OrderID DESC;", conn);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add("@take", SqlDbType.Int).Value = take;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            conn.Open();
            da.Fill(dt);

            return dt;
        }

        public void UpdateOrderStatus(int orderID, string status)
        {
            SqlConnection conn = new SqlConnection(connStr);
            SqlCommand cmd = new SqlCommand(
                "UPDATE dbo.Orders SET OrderStatus=@s WHERE OrderID=@id;", conn);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add("@s", SqlDbType.NVarChar, 20).Value = status; // Received/Preparing/Ready/Cancelled
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = orderID;

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void UpdatePaymentStatus(int orderID, string paymentStatus)
        {
            SqlConnection conn = new SqlConnection(connStr);
            SqlCommand cmd = new SqlCommand(
                "UPDATE dbo.Orders SET PaymentStatus=@ps WHERE OrderID=@id;", conn);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add("@ps", SqlDbType.NVarChar, 20).Value = paymentStatus; // Pending/Paid/Refunded
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = orderID;

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void MarkReady(int orderID)
        {
            UpdateOrderStatus(orderID, "Ready");
        }
    }
}
