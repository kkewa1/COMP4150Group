using System;
using System.Data;
using System.Windows.Controls;
using System.Windows;
using Program.ViewModels;

namespace Program.Views
{
    public partial class OrderStatusView : UserControl
    {
        private OrderStatusViewModel VM => DataContext as OrderStatusViewModel;
        public OrderStatusView()
        {
            InitializeComponent();
            DataContext = new OrderStatusViewModel();
        }
        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            VM.LoadData();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (GridOrders.SelectedItem is DataRowView row)
                VM.SaveData(row);
        }

        private void BtnReady_Click(object sender, RoutedEventArgs e)
        {
            if (GridOrders.SelectedItem is DataRowView row)
                VM.MarkReady(row);
        }
    }
}
