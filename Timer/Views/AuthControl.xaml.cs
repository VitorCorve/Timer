using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Timer.Views
{
    /// <summary>
    /// Логика взаимодействия для AuthControl.xaml
    /// </summary>
    public partial class AuthControl : UserControl
    {
        public AuthControl()
        {
            InitializeComponent();
            LoginTextBox.Focus();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { ((dynamic)this.DataContext).SecurePassword = ((PasswordBox)sender).SecurePassword; }
        }

        private void ControlKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
            {
                if (LoginTextBox.IsFocused)
                    PasswordHidden.Focus();

                else
                    LoginTextBox.Focus();

                e.Handled = true;
            }
        }
    }
}
