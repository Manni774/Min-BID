using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
            dgUsers.ItemsSource = Entities1.GetContext().Users.ToList();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditUserWindow((sender as Button).DataContext as User));
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditUserWindow(null));
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var userRemove = dgUsers.SelectedItems.Cast<User>().ToList();

            if (userRemove.Any())
            {
                if (MessageBox.Show($"Вы точно хотите удалить следующие {userRemove.Count()} элементов?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        var context = Entities1.GetContext();
                        foreach (var item in userRemove)
                        {
                            context.Users.Remove(item);
                        }
                        context.SaveChanges();
                        MessageBox.Show("Данные удалены.");
                        dgUsers.ItemsSource = new Entities1().Users.ToList();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var context = Entities1.GetContext();
            foreach (var entry in context.ChangeTracker.Entries().ToList())
            {
                if (entry.State != EntityState.Added)
                {
                    entry.Reload();
                }
            }
            dgUsers.ItemsSource = context.Users.ToList();
        }

    }
}
