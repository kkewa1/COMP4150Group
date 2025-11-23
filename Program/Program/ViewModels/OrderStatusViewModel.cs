using System;
using System.Data;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Program.ViewModels
{
    public class OrderStatusViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private readonly DBAOrder _orders = new DBAOrder();

        private DataView _ordersView;
        public DataView OrdersView
        {
            get => _ordersView;
            set
            {
                _ordersView = value;
                OnPropertyChanged(nameof(OrdersView));
            }
        }
        public string[] OrderStatuses { get; } = { "Received", "Preparing", "Ready", "Cancelled" };
        public string[] PaymentStatuses { get; } = { "Pending", "Paid", "Refunded" };

        public OrderStatusViewModel()
        {
            LoadData();
        }

        public void LoadData()
        {
            DataTable dt = _orders.GetRecentOrders(100);
            OrdersView = dt.DefaultView;
        }

        public void SaveData(DataRowView row)
        {
            int orderId = Convert.ToInt32(row["OrderID"]);
            string orderStatus = Convert.ToString(row["OrderStatus"]);
            string paymentStatus = Convert.ToString(row["PaymentStatus"]);
            _orders.UpdateOrderStatus(orderId, orderStatus);
            _orders.UpdatePaymentStatus(orderId, paymentStatus);
        }

        public void MarkReady(DataRowView row)
        {
            int orderId = Convert.ToInt32(row["OrderID"]);
            _orders.UpdateOrderStatus(orderId, "Ready");
            row["OrderStatus"] = "Ready";
        }
    }
}
