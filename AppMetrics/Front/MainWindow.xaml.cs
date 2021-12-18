using System.Windows;
using AppMetricsCSharp.ViewModels;
using ConsoleApp1;

// ReSharper disable CheckNamespace

namespace AppMetricsCSharp.View
{
    public partial class MainWindow
    {
        public void Display()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        public MainWindow()
        {
            ConsoleUtility.AllocConsole();
            Display();
        }

        private void Hide_Title(object sender, RoutedEventArgs e)
        {   
            
            
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(MainWindow))
                {
                    ((MainWindow)window).TitlePanel.Visibility = Visibility.Hidden;
                }
            }
        }
    }
}