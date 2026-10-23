using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace моггейм
{
    public partial class CartWindow : Window
    {
        ObservableCollection<Game> cart;

        public CartWindow(ObservableCollection<Game> cart)
        {
            InitializeComponent();
            this.cart = cart;
            CartList.ItemsSource = cart;
            UpdateTotal();
        }

        void Remove_Click(object sender, RoutedEventArgs e)
        {
            cart.Remove((Game)((Button)sender).DataContext);
            UpdateTotal();
        }

        void Order_Click(object sender, RoutedEventArgs e)
        {
            cart.Clear();
            MessageBox.Show("Спасибо за покупку!");
            Close();
        }

        void UpdateTotal()
        {
            TotalText.Text = "Итого: " + cart.Sum(g => g.Price) + " ₽";
            EmptyText.Visibility = cart.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
            OrderButton.IsEnabled = cart.Count > 0;
        }
    }
}
