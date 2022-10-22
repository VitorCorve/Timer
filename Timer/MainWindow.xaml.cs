using System;
using System.Windows;

namespace Timer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            EngageButton.Focus();
        }

        private void Minimize(object sender, RoutedEventArgs e) => this.WindowState = WindowState.Minimized;

        private void Border_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
            EngageButton.Focus();
        }

        private void Exit(object sender, RoutedEventArgs e) =>this.Close();
    }
}
