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
using _222_Goman_WPF_Project.DBModel;

namespace _222_Goman_WPF_Project.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddCategoriesPage.xaml
    /// </summary>
    public partial class AddCategoryPage : Page
    {
        private Categories _currentCategories = new Categories();
        public AddCategoryPage(Categories selectedCategories)
        {
            InitializeComponent();
            if (selectedCategories != null)
                _currentCategories = selectedCategories;
            DataContext = _currentCategories;

        }
        private void ButtonSaveCategory_Click(object sender, RoutedEventArgs
        e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(_currentCategories.Name))
                errors.AppendLine("Укажите название категории!");
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            if (_currentCategories.ID == 0)
                Goman_DB_Payment0Entities.GetContext().Categories.Add(_currentCategories);
            try
            {
                Goman_DB_Payment0Entities.GetContext().SaveChanges();
                MessageBox.Show("Данные успешно сохранены!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        private void ButtonClean_Click(object sender, RoutedEventArgs e)
        {
            TBCategoryName.Text = "";
        }
    }
}
