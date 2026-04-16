using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
using System.Data.Entity.Validation;

namespace Min_BID
{
    /// <summary>
    /// Логика взаимодействия для UsersPage.xaml
    /// </summary>

    public partial class UsersPage : Page
    {
        public UsersPage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            using (var context = new Entities1())
            {
                // Загружаем пользователей вместе с ролью (для отображения названия)
                var users = context.Users.Include("Role").ToList();
                dgUsers.ItemsSource = users;
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var user = (sender as Button)?.DataContext as User;
            if (user == null) return;
            var window = new AddEditUserWindow(user);
            window.Owner = Window.GetWindow(this);
            if (window.ShowDialog() == true) { LoadData(); }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var window = new AddEditUserWindow(null);
            window.Owner = Window.GetWindow(this);
            if (window.ShowDialog() == true) { LoadData(); }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selected = dgUsers.SelectedItems.Cast<User>().ToList();
            if (!selected.Any()) return;

            if (MessageBox.Show($"Удалить {selected.Count()} пользователей?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                using (var context = new Entities1())
                {
                    foreach (var item in selected)
                    {
                        // Запрещаем удалять самого себя
                        if (App.CurrentUser != null && item.ID == App.CurrentUser.ID)
                        {
                            MessageBox.Show("Нельзя удалить самого себя.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                            continue;
                        }
                        context.Users.Attach(item);
                        context.Users.Remove(item);
                    }
                    context.SaveChanges();
                }
                LoadData();
                MessageBox.Show("Данные удалены.");
            }
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
                LoadData();
        }
    }
}