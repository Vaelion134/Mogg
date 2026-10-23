using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace моггейм
{
    public partial class MainWindow : Window
    {
        Dictionary<string, string> users = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        ObservableCollection<Game> cart = new ObservableCollection<Game>();
        string currentUser;
        string category = "Все игры";

        public MainWindow()
        {
            InitializeComponent();
            AllGamesRadio.IsChecked = true;
        }

        Visibility Show(bool visible)
        {
            return visible ? Visibility.Visible : Visibility.Collapsed;
        }

        bool FilterCards(Panel panel)
        {
            bool any = false;
            foreach (Border card in panel.Children)
            {
                bool visible = category == "Все игры" || ((string)card.Tag).Contains(category);
                card.Visibility = Show(visible);
                if (visible)
                    any = true;
            }
            return any;
        }

        void Category_Checked(object sender, RoutedEventArgs e)
        {
            category = (string)((RadioButton)sender).Tag;

            bool sale = FilterCards(SaleList);
            bool big = FilterCards(BigList);
            bool newGames = FilterCards(NewList);
            bool popular = FilterCards(PopularList);

            SaleSection.Visibility = Show(sale || big);
            NewSection.Visibility = Show(newGames);
            PopularSection.Visibility = Show(popular);
            EmptyText.Visibility = Show(!sale && !big && !newGames && !popular);

            MainScroll.ScrollToTop();
            NewScroller.ScrollToHorizontalOffset(0);
        }

        void NewScroller_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var scroller = (ScrollViewer)sender;
            scroller.ScrollToHorizontalOffset(scroller.HorizontalOffset - e.Delta);
            e.Handled = true;
        }

        bool ShowAuth(bool register)
        {
            var window = new AuthWindow(users, register) { Owner = this };
            if (window.ShowDialog() != true)
                return false;

            currentUser = window.User;
            UpdateHeader();
            return true;
        }

        void Login_Click(object sender, RoutedEventArgs e)
        {
            ShowAuth(false);
        }

        void Register_Click(object sender, RoutedEventArgs e)
        {
            ShowAuth(true);
        }

        void Logout_Click(object sender, RoutedEventArgs e)
        {
            currentUser = null;
            cart.Clear();
            UpdateHeader();
        }

        void UpdateHeader()
        {
            GuestPanel.Visibility = Show(currentUser == null);
            UserPanel.Visibility = Show(currentUser != null);
            UserNameText.Text = currentUser;
            CartButton.Content = cart.Count == 0 ? "Корзина" : "Корзина (" + cart.Count + ")";
        }

        void Buy_Click(object sender, RoutedEventArgs e)
        {
            string[] data = ((string)((Button)sender).Tag).Split(';');
            var game = new Game { Title = data[0], Price = int.Parse(data[1]) };

            if (currentUser == null)
            {
                MessageBox.Show("Чтобы купить игру, войдите в аккаунт.");
                if (!ShowAuth(false))
                    return;
            }

            if (cart.Any(g => g.Title == game.Title))
            {
                MessageBox.Show("Эта игра уже в корзине.");
                return;
            }

            cart.Add(game);
            UpdateHeader();
        }

        void Cart_Click(object sender, RoutedEventArgs e)
        {
            new CartWindow(cart) { Owner = this }.ShowDialog();
            UpdateHeader();
        }
    }
}
