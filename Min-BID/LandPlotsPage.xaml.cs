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
    /// Логика взаимодействия для LandPlotsPage.xaml
    /// </summary>
    public partial class LandPlotsPage : Page
    {
        public LandPlotsPage()
        {
            InitializeComponent();
            dgLandPlots.ItemsSource = Entities1.GetContext().LandPlots.ToList();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditLandPlotsPage((sender as Button).DataContext as LandPlot));
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditLandPlotsPage(null));
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var plotRemove = dgLandPlots.SelectedItems.Cast<LandPlot>().ToList();

            if (plotRemove.Any())
            {
                if (MessageBox.Show($"Вы точно хотите удалить следующие {plotRemove.Count()} элементов?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        var context = Entities1.GetContext();
                        foreach (var item in plotRemove)
                        {
                            context.LandPlots.Remove(item);
                        }
                        context.SaveChanges();
                        MessageBox.Show("Данные удалены.");
                        dgLandPlots.ItemsSource = new Entities1().LandPlots.ToList();
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
            dgLandPlots.ItemsSource = context.LandPlots.ToList();
        }

    }
}
