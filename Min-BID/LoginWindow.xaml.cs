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

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string login = txtLogin.Text.Trim();
            string password = txtPassword.Password;

            using (var context = new Entities1())
            {
                var user = context.Users.Include("Role").FirstOrDefault(u => u.Логин == login);
                if (user == null)
                {
                    tbError.Text = "Неверный логин или пароль";
                    return;
                }

                string hashedInput = HashPassword(password, user.Соль);
                if (hashedInput == user.Хэш_пароль)
                {
                    App.CurrentUser = user;
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                }
                else
                {
                    tbError.Text = "Неверный логин или пароль";
                }
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