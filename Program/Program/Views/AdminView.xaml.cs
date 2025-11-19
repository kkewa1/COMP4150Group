using Program.ViewModels;
using System.Windows.Controls;

namespace Program.Views
{
    /// <summary>
    /// Interaction logic for AdminView.xaml
    /// </summary>
    public partial class AdminView : UserControl
    {
        public AdminView()
        {
            InitializeComponent();
            DataContext = new AdminViewModel();
        }
    }
}
