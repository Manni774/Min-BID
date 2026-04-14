using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Min_BID
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string login = txtLogin.Text.Trim();
            string password = txtPassword.Password;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                tbError.Text = "Введите логин и пароль";
                return;
            }

            btnLogin.IsEnabled = false;
            progressBar.Visibility = Visibility.Visible;
            tbError.Text = "";

            bool success = await System.Threading.Tasks.Task.Run(() => Authenticate(login, password));

            btnLogin.IsEnabled = true;
            progressBar.Visibility = Visibility.Collapsed;

            if (success)
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            else
            {
                tbError.Text = "Неверный логин или пароль";
            }
        }

        private bool Authenticate(string login, string password)
        {
            using (var context = new Entities1())
            {
                var user = context.Users.FirstOrDefault(u => u.Логин == login);
                if (user == null) return false;

                string hashedInput = HashPassword(password, user.Соль);
                return hashedInput == user.Хэш_пароль;
            }
        }

        private string HashPassword(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] combined = Encoding.UTF8.GetBytes(password + salt);
                byte[] hashBytes = sha256.ComputeHash(combined);
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}
