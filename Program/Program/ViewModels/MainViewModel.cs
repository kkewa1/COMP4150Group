using Program.Views;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Diagnostics;

namespace Program.ViewModels
{
    public class MainViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        public ObservableCollection<ItemData> MenuItems { get; set; }
        public ObservableCollection<OrderedItemData> OrderItems { get; set; }

        private ItemData _selectedMenuItem;
        public ItemData SelectedMenuItem
        {
            get => _selectedMenuItem;
            set
            {
                _selectedMenuItem = value;
                OnPropertyChanged();

                if (value != null)
                    AddItemToOrder(value);
            }
        }

        public int SelectedTableID { get; set; }
        public int SelectedStaffID { get; set; }
        public ICommand CreateOrderCommand { get; }
        public MainViewModel()
        {
            DBAItem db = new DBAItem();
            MenuItems = new ObservableCollection<ItemData>(db.GetAllItems());
            OrderItems = new ObservableCollection<OrderedItemData>();
            SelectedStaffID = 1;
            SelectedTableID = 100;
            CreateOrderCommand = new RelayCommand(_ => SubmitOrder());
        }
        private void AddItemToOrder(ItemData item)
        {
            var prompt = new QuantityPrompt();
            bool? result = prompt.ShowDialog();

            if (result == true)
            {
                int qty = prompt.Quantity;

                OrderItems.Add(new OrderedItemData
                {
                    ItemID = item.ItemID,
                    Name = item.Name,
                    Quantity = qty
                });
            }
        }

        private void SubmitOrder()
        {
            if (OrderItems == null || OrderItems.Count == 0)
            {
                MessageBox.Show("No items in order.");
                return;
            }

            try
            {
                DBAOrder db = new DBAOrder();

                int orderID = db.CreateOrder(SelectedTableID, SelectedStaffID);

                foreach (var item in OrderItems)
                {
                    db.AddItemToOrder(item.ItemID, orderID, item.Quantity);
                }

                MessageBox.Show($"Order {orderID} created!");
                OrderItems.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error creating order: " + ex.Message);
            }
        }

    }
}
