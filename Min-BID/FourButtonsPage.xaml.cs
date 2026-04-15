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

namespace Min_BID
{
    /// <summary>
    /// Логика взаимодействия для FourButtonsPage.xaml
    /// </summary>
    public partial class FourButtonsPage : Page
    {
        public FourButtonsPage()
        {
            InitializeComponent();
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
            NavigationManager.Navigate(new MinPage());
        }

        private void btnLandPlots_Click(object sender, RoutedEventArgs e)
        {
            NavigationManager.Navigate(new LandPlotsPage());
        }

        private void btnUsers_Click(object sender, RoutedEventArgs e)
        {
            NavigationManager.Navigate(new UsersPage());
        }

        private void btnAnalytics_Click(object sender, RoutedEventArgs e)
        {
            NavigationManager.Navigate(new AnalyticsPage());
        }
    }
}
