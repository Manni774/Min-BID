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
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        private LeaseAgreement _currentAgreement = new LeaseAgreement();
        public AddEditPage(LeaseAgreement selectedLeaseAgreemnt)
        {
            InitializeComponent();
            if (selectedLeaseAgreemnt != null)
                _currentAgreement = selectedLeaseAgreemnt;
            DataContext = _currentAgreement;

            CmbLandPlot.ItemsSource = Entities1.GetContext().LandPlots.ToList();
            CmbStatus.ItemsSource = Entities1.GetContext().PlotStatuses.ToList();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_currentAgreement.Название_компании))
                errors.AppendLine("Укажите название компании.");

            if (string.IsNullOrWhiteSpace(_currentAgreement.Арендатор))
                errors.AppendLine("Укажите арендатора компании.");

            if (_currentAgreement.Дата_начала == null)
                errors.AppendLine("Укажите дату начала подписания договора.");

            if (_currentAgreement.Дата_конца == null)
                errors.AppendLine("Укажите дату расторжения/завершения договора.");

            if (_currentAgreement.LandPlotsID == 0)
                errors.AppendLine("Укажите земельный участок.");

            if (_currentAgreement.StatusID == 0)
                errors.AppendLine("Укажите статус договора.");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (_currentAgreement.ID == 0)
                Entities1.GetContext().LeaseAgreements.Add(_currentAgreement);

            try
            {
                Entities1.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}\n{ex.InnerException?.Message}", "Ошибка сохранения");
            }
        }
    }
}
