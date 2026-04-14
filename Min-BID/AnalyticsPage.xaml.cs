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
    /// Логика взаимодействия для AnalyticsPage.xaml
    /// </summary>
    public partial class AnalyticsPage : Page
    {
        public AnalyticsPage()
        {
            InitializeComponent();
            LoadStats();
        }

        private void LoadStats()
        {
            using (var context = new Entities1())
            {
                var stats = context.LeaseAgreements
                    .GroupBy(l => l.PlotStatus.Статус)
                    .Select(g => new { Status = g.Key, Count = g.Count() })
                    .ToList();
                dgStats.ItemsSource = stats;
            }
        }
    }
}
