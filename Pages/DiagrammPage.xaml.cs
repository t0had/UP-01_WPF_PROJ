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
using System.Windows.Forms.DataVisualization.Charting;
using Word = Microsoft.Office.Interop.Word;
using Excel = Microsoft.Office.Interop.Excel;
using _222_Goman_WPF_Project.DBModel;

namespace _222_Goman_WPF_Project.Pages
{
    /// <summary>
    /// Логика взаимодействия для DiagrammPage.xaml
    /// </summary>
    public partial class DiagrammPage : Page
    {
        private Goman_DB_Payment0Entities _context = new Goman_DB_Payment0Entities();
        public DiagrammPage()
        {
            InitializeComponent();
            ChartPayments.ChartAreas.Add(new ChartArea("Main"));
            var currentSeries = new Series("Платежи")
            {
                IsValueShownAsLabel = true
            };
            ChartPayments.Series.Add(currentSeries);
            CmbBxUsers.ItemsSource = _context.Users.ToList(); //ФИО пользователей
            CmbBxDiagramm.ItemsSource = Enum.GetValues(typeof(SeriesChartType)); //Типы диаграммы
        }

        private void UpdateChart(object sender, SelectionChangedEventArgs e)
        {
            if (CmbBxUsers.SelectedItem is Users currentUser && CmbBxDiagramm.SelectedItem is SeriesChartType currentType)
            {
                Series currentSeries = ChartPayments.Series.FirstOrDefault();

                currentSeries.ChartType = currentType;
                currentSeries.Points.Clear();

                var categoriesList = _context.Categories.ToList();
                foreach (var category in categoriesList)
                {
                    currentSeries.Points.AddXY(category.Name, _context.Payments.ToList().Where(u => u.Users == currentUser && u.Categories == category).Sum(u => u.Price * u.Num));
                }
            }
        }
    }
}
