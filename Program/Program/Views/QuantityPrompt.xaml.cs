using System.Windows;

namespace Program.Views
{
    public partial class QuantityPrompt : Window
    {
        public int Quantity { get; private set; } = 1;

        public QuantityPrompt()
        {
            InitializeComponent();
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(quantityBox.Text, out int qty))
                Quantity = qty;

            DialogResult = true;
            Close();
        }
    }
}
