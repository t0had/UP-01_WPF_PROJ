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
using System.Runtime.ConstrainedExecution;

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

        private void ButtonExportWord_Click(object sender, RoutedEventArgs e)
        {
            var allUsers = _context.Users.ToList();
            var allCategories = _context.Categories.ToList();

            var application = new Word.Application();
            Word.Document document = application.Documents.Add();

            foreach (var user in allUsers)
            {
                Word.Paragraph userParagraph = document.Paragraphs.Add();
                Word.Range userRange = userParagraph.Range;
                userRange.Text = user.FIO;
                userParagraph.set_Style("Заголовок");
                userRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                userRange.InsertParagraphAfter();
                document.Paragraphs.Add(); //Пустая строка

                Word.Paragraph tableParagraph = document.Paragraphs.Add();
                Word.Range tableRange = tableParagraph.Range;
                Word.Table paymentsTable = document.Tables.Add(tableRange, allCategories.Count() + 1, 2);
                paymentsTable.Borders.InsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                paymentsTable.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                paymentsTable.Range.Cells.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;

                Word.Range cellRange;

                cellRange = paymentsTable.Cell(1, 1).Range;
                cellRange.Text = "Категория";
                cellRange = paymentsTable.Cell(1, 2).Range;
                cellRange.Text = "Сумма расходов";

                paymentsTable.Rows[1].Range.Font.Name = "Times New Roman";
                paymentsTable.Rows[1].Range.Font.Size = 14;
                paymentsTable.Rows[1].Range.Bold = 1;
                paymentsTable.Rows[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

                for (int i = 0; i < allCategories.Count(); i++)
                {
                    var currentCategory = allCategories[i];
                    cellRange = paymentsTable.Cell(i + 2, 1).Range;
                    cellRange.Text = currentCategory.Name;
                    cellRange.Font.Name = "Times New Roman";
                    cellRange.Font.Size = 12;

                    cellRange = paymentsTable.Cell(i + 2, 2).Range;
                    cellRange.Text = user.Payments.ToList().Where(u => u.Categories == currentCategory).Sum(u => u.Num * u.Price).ToString() + " руб."; //тут был ToString("N2")
                    cellRange.Font.Name = "Times New Roman";

                    cellRange.Font.Size = 12;
                } //завершение цикла по строкам таблицы
                document.Paragraphs.Add(); //пустая строка

                Payments maxPayment = user.Payments.OrderByDescending(u => u.Price * u.Num).FirstOrDefault();
                if (maxPayment != null)
                {
                    Word.Paragraph maxPaymentParagraph = document.Paragraphs.Add();
                    Word.Range maxPaymentRange = maxPaymentParagraph.Range;
                    maxPaymentRange.Text = $"Самый дорогостоящий платеж - { maxPayment.Name} за { (maxPayment.Price * maxPayment.Num).ToString()}" + $"руб.от { maxPayment.Date.ToString()}"; //тут был ToString("N2") и был ToString("dd.MM.yyyy")
                    maxPaymentParagraph.set_Style("Подзаголовок");
                    maxPaymentRange.Font.Color = Word.WdColor.wdColorDarkRed;
                    maxPaymentRange.InsertParagraphAfter();
                }
                document.Paragraphs.Add(); //пустая строка

                Payments minPayment = user.Payments.OrderBy(u => u.Price * u.Num).FirstOrDefault();
                if (maxPayment != null)
                {
                    Word.Paragraph minPaymentParagraph = document.Paragraphs.Add();
                    Word.Range minPaymentRange = minPaymentParagraph.Range;
                    minPaymentRange.Text = $"Самый дешевый платеж - {minPayment.Name} за { (minPayment.Price * minPayment.Num).ToString()} " + $"руб.от { minPayment.Date.ToString()}"; //тут был ToString("N2") и был ToString("dd.MM.yyyy")
                    minPaymentParagraph.set_Style("Подзаголовок");
                    minPaymentRange.Font.Color = Word.WdColor.wdColorDarkGreen;
                    minPaymentRange.InsertParagraphAfter();
                }

                if (user != allUsers.LastOrDefault()) document.Words.Last.InsertBreak(Word.WdBreakType.wdPageBreak);

                application.Visible = true;

                //НУЖНО СДЕЛАТЬ ТАК, ЧТОБЫ ПОЛЬЗОВАТЕЛЬ САМ ВЫБИРАЛ, КУДА И ЧТО ЭКСПОРТИРОВАТЬ
                document.SaveAs2(@"D:\Payments.docx");
                document.SaveAs2(@"D:\Payments.pdf",
                Word.WdExportFormat.wdExportFormatPDF);
                //завершение цикла по пользователям
            }

            //Страница 82 из инструкции. Эту всю хуйню нужно проверить!!!!!!
            document.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageFooter; Object oMissing = System.Reflection.Missing.Value; Object TotalPages = Microsoft.Office.Interop.Word.WdFieldType.wdFieldNumPages; Object CurrentPage = Microsoft.Office.Interop.Word.WdFieldType.wdFieldPage;
            document.ActiveWindow.Selection.Fields.Add(document.ActiveWindow.Selection.Range, ref CurrentPage, ref oMissing, ref oMissing);
            document.ActiveWindow.Selection.TypeText(" из ");
            document.ActiveWindow.Selection.Fields.Add(document.ActiveWindow.Selection.Range, ref TotalPages, ref oMissing, ref oMissing);

            foreach (Microsoft.Office.Interop.Word.Section section in document.Sections)
            {
                // Получаем диапазон заголовка и добавляем данные в верхний колонтитул.
                Microsoft.Office.Interop.Word.Range headerRange = section.Headers[Microsoft.Office.Interop.Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range; headerRange.Fields.Add(headerRange, Microsoft.Office.Interop.Word.WdFieldType.wdFieldPage);
                headerRange.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter; headerRange.Font.ColorIndex = Microsoft.Office.Interop.Word.WdColorIndex.wdBlack;
                headerRange.Font.Size = 10; headerRange.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }
        }

        private void ButtonExportExcel_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
