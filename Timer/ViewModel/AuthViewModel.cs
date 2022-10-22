using System;
using System.Configuration;
using System.Net;
using System.Security;

using Timer.Domain;
using Timer.Internal;
using Timer.Internal.Models;
using Timer.Internal.Services;
using Timer.Views;

using TimerApiHelper.TimerApiHelper;

namespace Timer.ViewModel
{
    public class AuthViewModel : PropertyChangedAbstract
    {
        public AuthViewModel()
        {
            Messenger.Register(new MessengerEntity(this, "AuthViewModel"));
        }

        private bool _CanAttemptToAuthorize;

        private string _UserLogin;

        private Command _login,
                        _register;

        public bool CanAttemptToAuthorize
        {
            get => _CanAttemptToAuthorize;
            set => Set(ref _CanAttemptToAuthorize, value);
        }

        private SecureString _SecurePassword;

        public string UserLogin
        {
            get => _UserLogin;
            set { Set(ref _UserLogin, value); CheckCanAttemptToAuthorize(); }
        }

        public SecureString SecurePassword
        {
            get => _SecurePassword;
            set { Set(ref _SecurePassword, value); CheckCanAttemptToAuthorize(); }
        }

        public Command Login => _login ??= new Command(async obj =>
        {
            try
            {
                Messenger.CallCommand("loading", "MainViewModel");

                UserDto userDto = new UserDto
                {
                    Username = UserLogin,
                    Password = new NetworkCredential("", SecurePassword).Password,
                    Token = string.Empty,
                    UserIdentifier = string.Empty
                };

                var response = await new Client(baseUrl: ConfigurationManager.AppSettings["BaseUrl"]).LoginAsync(userDto);
                UserData.Token = response.Token;

                Messenger.CallCommand("clocks", "MainViewModel");
            }
            catch (Exception e)
            {
                NotifyWindow window = new NotifyWindow("Incorrect user name or password");
                window.Show();
            }
            finally
            {
                Messenger.CallCommand("loading", "MainViewModel");
            }
        });

        public Command Register => _register ??= new Command(async obj =>
        {
            try
            {
                Messenger.CallCommand("lua", "MainViewModel");
            }
            catch (Exception)
            {
                NotifyWindow window = new NotifyWindow("Incorrect username or password");
                window.Show();
            }

        });

        private void CheckCanAttemptToAuthorize() => CanAttemptToAuthorize = SecurePassword != null && 
            SecurePassword.Length > 5 && !String.IsNullOrEmpty(UserLogin);
    }
}
