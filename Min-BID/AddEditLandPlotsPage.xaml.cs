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

            CmbCadastralNumber.ItemsSource = Entities1.GetContext().LandPlots.ToList();
            CmbStatus.ItemsSource = Entities1.GetContext().PlotStatuses.ToList();
            CmbCategoryName.ItemsSource = Entities1.GetContext().LandCategories.ToList();

        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            /*if (string.IsNullOrWhiteSpace(_currentPlots.Название_компании))
                errors.AppendLine("Укажите название компании.");

            if (string.IsNullOrWhiteSpace(_currentPlots.Арендатор))
                errors.AppendLine("Укажите арендатора компании.");

            if (_currentPlots.Дата_начала == null)
                errors.AppendLine("Укажите дату начала подписания договора.");

            if (_currentPlots.Дата_конца == null)
                errors.AppendLine("Укажите дату расторжения/завершения договора.");

            if (_currentPlots.LandPlotsID == 0)
                errors.AppendLine("Укажите земельный участок.");

            if (_currentPlots.StatusID == 0)
                errors.AppendLine("Укажите статус договора.");*/

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (_currentPlot.ID == 0)
                Entities1.GetContext().LandPlots.Add(_currentPlot);

            try
            {
                Entities1.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена.");
            }
            catch (Exception ex)
            {

                MessageBox.Show("Произошла ошибка при сохранении данных.");
            }
        }
    }
}
