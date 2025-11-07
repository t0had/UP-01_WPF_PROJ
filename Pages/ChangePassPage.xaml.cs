using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
    /// Логика взаимодействия для ChangePassPage.xaml
    /// </summary>
    public partial class ChangePassPage : Page
    {
        public ChangePassPage()
        {
            InitializeComponent();
        }
        public static string GetHash(String password)
        {
            using (var hash = SHA1.Create())
            {
                return
                string.Concat(hash.ComputeHash(Encoding.UTF8.GetBytes(password)).Select(x => x.ToString("X2")));
            }
        }

        private void txtbxLog_TextChanged(object sender, TextChangedEventArgs e)
        {
            lblLogHitn.Visibility = Visibility.Visible;
            if (txtbxLog.Text.Length > 0)
            {
                lblLogHitn.Visibility = Visibility.Hidden;
            }
        }
        private void passBxOld_PasswordChanged(object sender, RoutedEventArgs e)
        {
            lblPassOldHitn.Visibility = Visibility.Visible;
            if (passBxOld.Password.Length > 0)
            {
                lblPassOldHitn.Visibility = Visibility.Hidden;
            }
        }

        private void passBxFrst_PasswordChanged(object sender, RoutedEventArgs e)
        {
            lblPassHitn.Visibility = Visibility.Visible;
            if (passBxFrst.Password.Length > 0)
            {
                lblPassHitn.Visibility = Visibility.Hidden;
            }
        }

        private void passBxScnd_PasswordChanged(object sender, RoutedEventArgs e)
        {
            lblPassSecHitn.Visibility = Visibility.Visible;
            if (passBxScnd.Password.Length > 0)
            {
                lblPassSecHitn.Visibility = Visibility.Hidden;
            }
        }

        private void changePassButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtbxLog.Text) || string.IsNullOrEmpty(passBxOld.Password) || string.IsNullOrEmpty(passBxFrst.Password) || string.IsNullOrEmpty(passBxScnd.Password))
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }
            else
            {
                using (var db = new Goman_DB_Payment0Entities())
                {
                    var user = db.Users.AsNoTracking().FirstOrDefault(u => u.Login == txtbxLog.Text);

                    if (user != null)

                    {
                        MessageBox.Show("Пользователь с таким логином уже существует!");
                        return;
                    }
                    if (passBxFrst.Password.Length >= 6)
                    {
                        bool en = true;
                        bool number = false;

                        for (int i = 0; i < passBxFrst.Password.Length; i++)
                        {
                            if (passBxFrst.Password[i] >= '0' && passBxFrst.Password[i] <= '9') number = true;
                            else if (!((passBxFrst.Password[i] >= 'A' && passBxFrst.Password[i] <= 'Z') || (passBxFrst.Password[i] >= 'a' && passBxFrst.Password[i] <= 'z'))) en = false;
                        }

                        if (!en) MessageBox.Show("Используйте только английскую расскладку!");
                        else if (!number) MessageBox.Show("Добавьте хотябы одну цифру!");
                        if (en && number)
                        {
                            if (passBxFrst.Password != passBxScnd.Password)
                            {
                                MessageBox.Show("Пароли не совпадают!");
                            }
                            else
                            {
                                Users userObject = new Users
                                {
                                    Login = txtbxLog.Text,
                                    Password = GetHash(passBxFrst.Password),

                                };
                                db.Users.Add(userObject);
                                db.SaveChanges();
                                MessageBox.Show("Пользователь успешно зарегистрирован!");
                                txtbxLog.Clear();
                                passBxFrst.Clear();
                                passBxScnd.Clear();
                                return;
                            }
                        }
                    }
                    else MessageBox.Show("Пароль слишком короткий, должно быть минимум 6 символов!");
                }
            }
        }
        private void lblLogHitn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            txtbxLog.Focus();
        }

        private void lblPassHitn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            passBxFrst.Focus();
        }

        private void lblPassSecHitn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            passBxScnd.Focus();
        } 

        private void lblPassOldHitn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            passBxOld.Focus();
        }
    }
}
