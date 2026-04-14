using System;
using System.Collections.Generic;
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
using Min_BID;

namespace Min_BID
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new MinPage());
            Manager.MainFrame = MainFrame;
            CheckUserRole();
        }

        private void CheckUserRole()
        {
            // Если пользователь не администратор, скрываем кнопку "Пользователи"
            if (App.CurrentUser != null && App.CurrentUser.Role.Роль_пользователя != "Администратор")
            {
                btnUsers.Visibility = Visibility.Collapsed;
            }
        }

        private void btnLeaseAgreements_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new MinPage());
        }

        private void btnLandPlots_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new LandPlotsPage());
        }

        private void btnUsers_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new UsersPage());
        }

        private void btnAnalytics_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new AnalyticsPage());
        }
    

        private void Btnback_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.GoBack();
        }

        private void MainFrame_ContentRendered(object sender, EventArgs e)
        {
            if (MainFrame.CanGoBack)
            {
                Btnback.Visibility = Visibility.Visible;
            }
            else
            {
                Btnback.Visibility = Visibility.Hidden;
            }
        }
    }
}
