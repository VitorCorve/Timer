using System.Windows;

namespace Timer.Views
{
    /// <summary>
    /// Логика взаимодействия для LadderWindow.xaml
    /// </summary>
    public partial class LadderWindow : Window
    {
        public LadderWindow()
        {
            InitializeComponent();
        }

        private void Minimize(object sender, RoutedEventArgs e) => this.WindowState = WindowState.Minimized;

        private void Border_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();

        }

        private void Exit(object sender, RoutedEventArgs e) => this.Close();
    }
}
