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
    /// Логика взаимодействия для AddUserPage.xaml
    /// </summary>
    public partial class AddUserPage : Page
    {
        private Users _currentUser = new Users();
        public AddUserPage(Users selectedUser)
        {
            InitializeComponent();

            if (selectedUser != null) _currentUser = selectedUser; //откуда взять йоба селектед юзер
            DataContext = _currentUser;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(_currentUser.Login)) errors.AppendLine("Укажите логин!");
            if (string.IsNullOrWhiteSpace(_currentUser.Password)) errors.AppendLine("Укажите пароль!");
            if ((_currentUser.Role == null) || (cmbRole.Text == "")) errors.AppendLine("Выберите роль!");
            else
                _currentUser.Role = cmbRole.Text;
            if (string.IsNullOrWhiteSpace(_currentUser.FIO)) errors.AppendLine("Укажите ФИО");
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            if (_currentUser.ID == 0) Goman_DB_Payment0Entities.GetContext().Users.Add(_currentUser);
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
            TBLogin.Text = "";
            TBPass.Text = "";
            cmbRole.Items.Clear();
            TBFio.Text = "";
            TBPhoto.Text = "";
        }
    }
}
