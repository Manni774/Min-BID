using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Min_BID
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static User CurrentUser { get; set; }
    }

    public partial class App : Application
    {
        public App()
        {
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show($"Ошибка: {e.Exception.Message}\n{e.Exception.StackTrace}", "Критическая ошибка");
            e.Handled = true; // приложение не закроется
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show($"Ошибка: {(e.ExceptionObject as Exception)?.Message}");
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            // Автоматическое создание администратора, если нет пользователей
            using (var context = new Entities1())
            {
                if (!context.Users.Any())
                {
                    string salt = GenerateSalt();
                    string hash = HashPassword("admin", salt);
                    var adminRole = context.Roles.FirstOrDefault(r => r.Роль_пользователя == "Администратор");
                    if (adminRole == null)
                    {
                        adminRole = new Role { Роль_пользователя = "Администратор" };
                        context.Roles.Add(adminRole);
                        context.SaveChanges();
                    }
                    var admin = new User
                    {
                        Логин = "admin",
                        Хэш_пароль = hash,
                        Соль = salt,
                        RolesID = adminRole.ID
                    };
                    context.Users.Add(admin);
                    context.SaveChanges();
                    MessageBox.Show("Создан администратор: логин admin, пароль admin");
                }
            }
        }

        // Генерация случайной соли (16 символов)
        private string GenerateSalt()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 16).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        // Хэширование пароля с солью (SHA256)
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
