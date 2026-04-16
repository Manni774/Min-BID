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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.Entity;

namespace Min_BID
{
    /// <summary>
    /// Логика взаимодействия для AddEditUserPage.xaml
    /// </summary>
    public partial class AddEditUserWindow : Window
    {

        private User _currentUser;
        private bool _isEditMode;
        public AddEditUserWindow()
        {
            InitializeComponent();
            _isEditMode = false;
            _currentUser = new User();
            LoadRoles();
            // В режиме добавления всегда требуется ввод пароля
            chkChangePassword.Visibility = Visibility.Collapsed;
        }
        // Конструктор для редактирования существующего пользователя
        public AddEditUserWindow(User user)
        {
            InitializeComponent();
            _isEditMode = true;
            _currentUser = user;
            LoadRoles();
            LoadUserData();
            // Показываем чекбокс для возможности смены пароля
            chkChangePassword.Visibility = Visibility.Visible;
            chkChangePassword.IsChecked = false;
            // Блокируем логин при редактировании (логин менять нельзя)
            txtLogin.IsEnabled = false;
        }

        private void LoadRoles()
        {
            using (var context = new Entities1())
            {
                cmbRole.ItemsSource = context.Roles.ToList();
                if (cmbRole.Items.Count > 0)
                    cmbRole.SelectedIndex = 0;
            }
        }

        private void LoadUserData()
        {
            if (_currentUser != null)
            {
                txtLogin.Text = _currentUser.Логин;
                if (_currentUser.RolesID != 0)
                    cmbRole.SelectedValue = _currentUser.RolesID;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // Валидация
            if (string.IsNullOrWhiteSpace(txtLogin.Text))
            {
                MessageBox.Show("Логин не может быть пустым.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtLogin.Focus();
                return;
            }

            // Проверка уникальности логина (при добавлении или если логин изменился)
            using (var context = new Entities1())
            {
                bool loginExists = context.Users.Any(u => u.Логин == txtLogin.Text && u.ID != _currentUser.ID);
                if (loginExists)
                {
                    MessageBox.Show("Пользователь с таким логином уже существует.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtLogin.Focus();
                    return;
                }
            }

            // Обработка пароля
            string password = txtPassword.Password;
            string passwordConfirm = txtPasswordConfirm.Password;
            if (!_isEditMode) // Новый пользователь – пароль обязателен
            {
                if (string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("Введите пароль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtPassword.Focus();
                    return;
                }
                if (password != passwordConfirm)
                {
                    MessageBox.Show("Пароли не совпадают.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtPasswordConfirm.Focus();
                    return;
                }
                // Генерируем соль и хэш
                string salt = GenerateSalt();
                string hash = HashPassword(password, salt);
                _currentUser.Хэш_пароль = hash;
                _currentUser.Соль = salt;
            }
            else // Редактирование
            {
                if (chkChangePassword.IsChecked == true)
                {
                    if (string.IsNullOrEmpty(password))
                    {
                        MessageBox.Show("Введите новый пароль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        txtPassword.Focus();
                        return;
                    }
                    if (password != passwordConfirm)
                    {
                        MessageBox.Show("Пароли не совпадают.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        txtPasswordConfirm.Focus();
                        return;
                    }
                    string salt = GenerateSalt();
                    string hash = HashPassword(password, salt);
                    _currentUser.Хэш_пароль = hash;
                    _currentUser.Соль = salt;
                }
                // Если пароль не меняем – оставляем старые значения
            }

            // Устанавливаем логин и роль
            _currentUser.Логин = txtLogin.Text;
            if (cmbRole.SelectedValue != null)
                _currentUser.RolesID = (int)cmbRole.SelectedValue;

            // Сохраняем в базу
            using (var context = new Entities1())
            {
                try
                {
                    if (!_isEditMode)
                    {
                        context.Users.Add(_currentUser);
                    }
                    else
                    {
                        // Прикрепляем сущность и помечаем как изменённую
                        context.Users.Attach(_currentUser);
                        context.Entry(_currentUser).State = System.Data.EntityState.Modified;
                    }
                    context.SaveChanges();
                    DialogResult = true;
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении: {ex.Message}\n{ex.InnerException?.Message}",
                                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
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
