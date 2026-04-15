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
    /// Логика взаимодействия для AddEditLandPlotsPage.xaml
    /// </summary>
    public partial class AddEditLandPlotsPage : Page
    {
        private LandPlot _currentPlot = new LandPlot();
        public AddEditLandPlotsPage(LandPlot selectedLandPlot)
        {
            InitializeComponent();
            if (selectedLandPlot != null)
                _currentPlot = selectedLandPlot;
            DataContext = _currentPlot;

            using (var context = new Entities1())
            {
                CmbRegion.ItemsSource = context.Regions.ToList();
                CmbStatus.ItemsSource = context.PlotStatuses.ToList();
                CmbCategoryName.ItemsSource = context.LandCategories.ToList();
            }
        }
        
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            // Валидация (добавьте свои проверки)
            if (string.IsNullOrWhiteSpace(_currentPlot.Кадастровый_номер))
            {
                MessageBox.Show("Введите кадастровый номер");
                return;
            }

            using (var context = new Entities1())
            {
                if (_currentPlot.ID == 0)
                    context.LandPlots.Add(_currentPlot);
                else
                {
                    context.LandPlots.Attach(_currentPlot);
                    context.Entry(_currentPlot).State = EntityState.Modified;
                }

                try
                {
                    context.SaveChanges();
                    MessageBox.Show("Сохранено");
                    if (NavigationService.CanGoBack) NavigationService.GoBack();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException valEx)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("Ошибка валидации данных:");
                    foreach (var error in valEx.EntityValidationErrors)
                    {
                        foreach (var detail in error.ValidationErrors)
                        {
                            sb.AppendLine($"  Поле: {detail.PropertyName}, Ошибка: {detail.ErrorMessage}");
                        }
                    }
                    MessageBox.Show(sb.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                    if (ex.InnerException != null)
                        msg += "\n\n" + ex.InnerException.Message;
                    MessageBox.Show(msg, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
