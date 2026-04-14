using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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
    /// Логика взаимодействия для MinPage.xaml
    /// </summary>
    public partial class MinPage : Page
    {
        public MinPage()
        {
            InitializeComponent();
            DGridMindash.ItemsSource = Entities1.GetContext().LeaseAgreements.ToList();
        }

        private void LoadStatuses()
        {
            using (var context = new Entities1())
            {
                var statuses = context.PlotStatuses.ToList();
                statuses.Insert(0, new PlotStatus { ID = 0, Статус = "Все" });
                cmbStatusFilter.ItemsSource = statuses;
                cmbStatusFilter.SelectedValue = 0;
            }
        }

        private void Filter_Changed(object sender, RoutedEventArgs e)
        {
            using (var context = new Entities1())
            {
                var query = context.LeaseAgreements.Include(l => l.LandPlot)
                                                   .Include(l => l.PlotStatus)
                                                   .AsQueryable();

                if (!string.IsNullOrWhiteSpace(txtCompanyFilter.Text))
                    query = query.Where(l => l.Название_компании.Contains(txtCompanyFilter.Text));

                if (!string.IsNullOrWhiteSpace(txtTenantFilter.Text))
                    query = query.Where(l => l.Арендатор.Contains(txtTenantFilter.Text));

                if (cmbStatusFilter.SelectedValue != null && (int)cmbStatusFilter.SelectedValue != 0)
                {
                    int statusId = (int)cmbStatusFilter.SelectedValue;
                    query = query.Where(l => l.StatusID == statusId);
                }

                DGridMindash.ItemsSource = query.ToList();
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage((sender as Button).DataContext as LeaseAgreement));
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage(null));
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var agreementRemove = DGridMindash.SelectedItems.Cast<LeaseAgreement>().ToList();

            if (agreementRemove.Any())
            {
                if (MessageBox.Show($"Вы точно хотите удалить следующие {agreementRemove.Count()} элементов?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        var context = Entities1.GetContext();
                        foreach (var item in agreementRemove)
                        {
                            context.LeaseAgreements.Remove(item);
                        }
                        context.SaveChanges();
                        MessageBox.Show("Данные удалены.");
                        DGridMindash.ItemsSource = new Entities1().LeaseAgreements.ToList();
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
            DGridMindash.ItemsSource = context.LeaseAgreements.ToList();
        }
    }

}
