using _222_Goman_WPF_Project.Pages;
using System;
using System.Windows;
using System.Windows.Controls;

namespace _222_Goman_WPF_Project
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool themeSwitch = false;
        private Page page = new AuthPage();
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
            if (MessageBox.Show("Вы уверены, что хотите закрыть окно?", "Предупреждение",
            MessageBoxButton.YesNo, MessageBoxImage.Information) == System.Windows.MessageBoxResult.No)
                e.Cancel = true;
            else

                e.Cancel = false;
        }

        private void ButtonChangeTheme_Click(object sender, RoutedEventArgs e)
        {
            var uriBlack = new Uri("DictionaryBlackTheme.xaml", UriKind.Relative);
            var uri = new Uri("Dictionary.xaml", UriKind.Relative);
            ResourceDictionary resourceDict = Application.LoadComponent(uri) as ResourceDictionary;
            ResourceDictionary resourceDictBlack = Application.LoadComponent(uriBlack) as ResourceDictionary;
            if (themeSwitch == true)
            {
                Application.Current.Resources.Clear();
                Application.Current.Resources.MergedDictionaries.Add(resourceDict);
                themeSwitch = false;
            }
            else if (themeSwitch == false)
            {
                Application.Current.Resources.Clear();
                Application.Current.Resources.MergedDictionaries.Add(resourceDictBlack);
                themeSwitch = true;
            }
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            if (MainFrame.NavigationService.CanGoBack)
            {
                MainFrame.NavigationService?.GoBack();
            }
        }

        private void ButtonSignIn_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(page);
        }
    }
}
