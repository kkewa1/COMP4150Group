using System;
using System.Data;
using System.Windows.Controls;

namespace Program.Views
{
    public partial class OrderStatusView : UserControl
    {
        private readonly DBAOrder _orders = new DBAOrder();
        private DataTable _dt;

        // expose these for XAML ElementName binding
        public string[] OrderStatuses { get; } = { "Received", "Preparing", "Ready", "Cancelled" };
        public string[] PaymentStatuses { get; } = { "Pending", "Paid", "Refunded" };

        public OrderStatusView()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            _dt = _orders.GetRecentOrders(100);
            GridOrders.ItemsSource = _dt.DefaultView;
        }

        private void BtnRefresh_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            LoadData();
        }

        private void BtnSave_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (GridOrders.SelectedItem == null) return;
            var row = (DataRowView)GridOrders.SelectedItem;
            int orderId = Convert.ToInt32(row["OrderID"]);
            string orderStatus = Convert.ToString(row["OrderStatus"]);
            string paymentStatus = Convert.ToString(row["PaymentStatus"]);
            _orders.UpdateOrderStatus(orderId, orderStatus);
            _orders.UpdatePaymentStatus(orderId, paymentStatus);
        }

        private void BtnReady_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (GridOrders.SelectedItem == null) return;
            var row = (DataRowView)GridOrders.SelectedItem;
            int orderId = Convert.ToInt32(row["OrderID"]);
            _orders.UpdateOrderStatus(orderId, "Ready"); // fires trigger
            row["OrderStatus"] = "Ready";
        }
    }
}
