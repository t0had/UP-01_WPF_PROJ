using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
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
using _222_Goman_WPF_Project.DBModel;

namespace _222_Goman_WPF_Project.Pages
{
    /// <summary>
    /// Логика взаимодействия для CategoryTabPage.xaml
    /// </summary>
    public partial class CategoryTabPage : Page
    {
        public CategoryTabPage()
        {
            InitializeComponent();
            DataGridCategory.ItemsSource = Goman_DB_Payment0Entities.GetContext().Categories.ToList();
            this.IsVisibleChanged += Page_IsVisibleChanged;

        }
        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Goman_DB_Payment0Entities.GetContext().ChangeTracker.Entries().ToList().ForEach(x => x.Reload()); 
                DataGridCategory.ItemsSource = Goman_DB_Payment0Entities.GetContext().Categories.ToList();
            }
        }
        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new AddCategoryPage(null));
        }
        private void ButtonDel_Click(object sender, RoutedEventArgs e)
        {
            var categoryForRemoving =
            DataGridCategory.SelectedItems.Cast<Categories>().ToList();
            if (MessageBox.Show($"Вы точно хотите удалить записи в количестве { categoryForRemoving.Count()} элементов ? ", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    Goman_DB_Payment0Entities.GetContext().Categories.RemoveRange(categoryForRemoving);
                    Goman_DB_Payment0Entities.GetContext().SaveChanges();
                    MessageBox.Show("Данные успешно удалены!");
                    DataGridCategory.ItemsSource =
                    Goman_DB_Payment0Entities.GetContext().Categories.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }
        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Pages.AddCategoryPage((sender as Button).DataContext as Categories));
        }
    }
}
