using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace моггейм
{
    public partial class AuthWindow : Window
    {
        Dictionary<string, string> users;
        bool isRegister;

        public string User { get; private set; }

        public AuthWindow(Dictionary<string, string> users, bool isRegister)
        {
            InitializeComponent();
            this.users = users;
            SetMode(isRegister);
        }

        void SetMode(bool register)
        {
            isRegister = register;
            Brush active = new SolidColorBrush(Color.FromRgb(0x9D, 0x3E, 0xEE));

            LoginTab.Background = register ? Brushes.Transparent : active;
            RegisterTab.Background = register ? active : Brushes.Transparent;
            ConfirmPanel.Visibility = register ? Visibility.Visible : Visibility.Collapsed;
            SubmitButton.Content = register ? "Зарегистрироваться" : "Войти";
            ErrorText.Text = "";
            PasswordBox.Clear();
            ConfirmPasswordBox.Clear();
        }

        void LoginTab_Click(object sender, RoutedEventArgs e)
        {
            SetMode(false);
        }

        void RegisterTab_Click(object sender, RoutedEventArgs e)
        {
            SetMode(true);
        }

        void Submit_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginBox.Text.Trim();
            string password = PasswordBox.Password;

            if (isRegister)
            {
                if (login.Length < 3)
                    ErrorText.Text = "Логин должен содержать минимум 3 символа.";
                else if (password.Length < 6)
                    ErrorText.Text = "Пароль должен содержать минимум 6 символов.";
                else if (password != ConfirmPasswordBox.Password)
                    ErrorText.Text = "Пароли не совпадают.";
                else if (users.ContainsKey(login))
                    ErrorText.Text = "Такой логин уже существует.";
                else
                {
                    users.Add(login, password);
                    User = login;
                    DialogResult = true;
                }
            }
            else
            {
                if (users.ContainsKey(login) && users[login] == password)
                {
                    User = login;
                    DialogResult = true;
                }
                else
                    ErrorText.Text = "Неверный логин или пароль.";
            }
        }
    }
}
