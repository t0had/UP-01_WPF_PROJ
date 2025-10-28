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

namespace _222_Goman_WPF_Project
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.IsEnabled = true;
            timer.Tick += (o, t) => {
                DateTimeNow.Text = DateTime.Now.ToString();
            };
            timer.Start();
        }
        void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите закрыть окно?", "Message",
            MessageBoxButton.YesNo) == System.Windows.MessageBoxResult.No)
                e.Cancel = true;
            else

                e.Cancel = false;
        }

        private void ButtonChangeTheme_Click(object sender, RoutedEventArgs e)
        {
            // определяем путь к файлу ресурсов
            var uriBlack = new Uri("DictionaryBlackTheme.xaml", UriKind.Relative);
            var uri = new Uri("Dictionary.xaml", UriKind.Relative);
            // загружаем словарь ресурсов
            ResourceDictionary resourceDict = Application.LoadComponent(uri) as ResourceDictionary;
            ResourceDictionary resourceDictBlack = Application.LoadComponent(uriBlack) as ResourceDictionary;
            //if (Application.Current.Resources.MergedDictionaries.Contains(resourceDictBlack))
            //{
            //    // очищаем коллекцию ресурсов приложения
            //    Application.Current.Resources.Clear();
            //    // добавляем загруженный словарь ресурсов
            //    Application.Current.Resources.MergedDictionaries.Add(resourceDict);
            //    MessageBox.Show("(((((((((");
            //}
            //else if (Application.Current.Resources.MergedDictionaries.Contains(resourceDict))
            //{
            //    // очищаем коллекцию ресурсов приложения
            //    Application.Current.Resources.Clear();
            //    // добавляем загруженный словарь ресурсов
            //    Application.Current.Resources.MergedDictionaries.Add(resourceDictBlack);
            //    MessageBox.Show(")))))))))))");
            //}
            Application.Current.Resources.Clear();
            //добавляем загруженный словарь ресурсов
            Application.Current.Resources.MergedDictionaries.Add(resourceDictBlack);
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
