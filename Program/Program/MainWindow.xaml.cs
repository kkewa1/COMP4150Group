using Program.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Program
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        UserData userData;

        public MainWindow()
        {
            InitializeComponent();
        }

        void OnLoad(object sender, RoutedEventArgs e)
        {
            DataContext = new MainViewModel();
            userData = new UserData();
        }

        private void HomeButton_Clicked(object sender, RoutedEventArgs e)
        {
            DataContext = new MainViewModel();
        }

        private void AdminButton_Clicked(object sender, RoutedEventArgs e)
        {
            if (userData.ID > 0)
            {
                DataContext = new AdminViewModel();
            } else
            {
                MessageBox.Show("You cannot access the admin view.\nPlease try logging in.");
            }
        }

        private void ManageOrders_Click(object sender, RoutedEventArgs e)
        {
            if (userData.ID > 0)
            {
                DataContext = new OrderStatusViewModel();
            }
            else
            {
                MessageBox.Show("You cannot access the orders view.\nPlease try logging in.");
            }
        }

        private void LoginButton_Clicked(object sender, RoutedEventArgs e)
        {
            LoginDialog dlg = new LoginDialog();
            dlg.Owner = this;
            dlg.ShowDialog();
            if (dlg.DialogResult == true)
            {
                DBALogin dbLogin = new DBALogin();
                userData = dbLogin.Login(dlg.username.Text, dlg.password.Text);
                if (userData.ID > 0)
                {
                    LoginButton.Visibility = Visibility.Collapsed;
                    MessageBox.Show("You are logged in as StaffID " + userData.ID + ".");
                }else
                {
                    MessageBox.Show("Your login was unsuccessful.");
                }
            }
        }
    }
}
