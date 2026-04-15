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
    /// Логика взаимодействия для LandPlotsPage.xaml
    /// </summary>

    public partial class LandPlotsPage : Page
    {
        public LandPlotsPage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            using (var context = new Entities1())
            {
                dgLandPlots.ItemsSource = context.LandPlots.ToList();
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var plot = (sender as Button)?.DataContext as LandPlot;
            if (plot == null) return;
            Manager.MainFrame.Navigate(new AddEditLandPlotsPage(plot));
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditLandPlotsPage(null));
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selected = dgLandPlots.SelectedItems.Cast<LandPlot>().ToList();
            if (!selected.Any()) return;

            if (MessageBox.Show($"Удалить {selected.Count()} записей?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                using (var context = new Entities1())
                {
                    foreach (var item in selected)
                    {
                        context.LandPlots.Attach(item);
                        context.LandPlots.Remove(item);
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