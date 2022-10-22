using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Timer.Views
{
    /// <summary>
    /// Логика взаимодействия для RegisterControl.xaml
    /// </summary>
    public partial class RegisterControl : UserControl
    {
        public RegisterControl()
        {
            InitializeComponent();
            LoginTextBox.Focus();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { ((dynamic)this.DataContext).SecurePassword = ((PasswordBox)sender).SecurePassword; }
        }

        private void ConfirmPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { ((dynamic)this.DataContext).ConfirmSecurePassword = ((PasswordBox)sender).SecurePassword; }
        }

        private void ControlKeyDown(object sender, KeyEventArgs e)
        {
            if (!Keyboard.IsKeyDown(Key.LeftShift) && e.Key == Key.Tab)
            {
                e.Handled = true;

                if (LoginTextBox.IsFocused)
                {
                    PasswordHidden.Focus();
                    return;
                }

                if (PasswordHidden.IsFocused)
                {
                    ConfirmPasswordHidden.Focus();
                    return;
                }

                if (ConfirmPasswordHidden.IsFocused)
                {
                    LoginTextBox.Focus();
                    return;
                }
            }

            else if (Keyboard.IsKeyDown(Key.LeftShift) && e.Key == Key.Tab)
            {
                e.Handled = true;

                if (LoginTextBox.IsFocused)
                {
                    ConfirmPasswordHidden.Focus();
                    return;
                }

                if (PasswordHidden.IsFocused)
                {
                    LoginTextBox.Focus();
                    return;
                }

                if (ConfirmPasswordHidden.IsFocused)
                {
                    PasswordHidden.Focus();
                    return;
                }
            }
        }
    }
}
