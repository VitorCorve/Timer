using System.Windows;

namespace Timer.Views
{
    /// <summary>
    /// Логика взаимодействия для NotifyWindow.xaml
    /// </summary>
    public partial class NotifyWindow : Window
    {
        public NotifyWindow()
        {
            InitializeComponent();
        }

        public NotifyWindow(string message)
        {
            InitializeComponent();
            NotifyWindowTextBlock.Text = message;
        }

        private void Exit(object sender, RoutedEventArgs e) => this.Close();
    }
}
