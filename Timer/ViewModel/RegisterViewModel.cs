using Microsoft.Win32;

using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Timer.Internal;
using Timer.Internal.Models;
using Timer.Internal.Services;
using Timer.Views;

using TimerApiHelper.TimerApiHelper;
using System.Reflection;
using System.Linq;

namespace Timer.ViewModel
{
    public class RegisterViewModel : PropertyChangedAbstract
    {
        public RegisterViewModel()
        {
            Messenger.Register(new MessengerEntity(this, "RegisterViewModel"));
        }

        private bool _CanAttemptToRegister;

        private string _UserLogin, _Nickname, _picturePath;

        private SecureString _SecurePassword,
                             _ConfirmSecurePassword;

        private ImageSource _picture;

        private Command _register, _back, _selectPicture;

        public bool CanAttemptToRegister
        {
            get => _CanAttemptToRegister;
            set { _CanAttemptToRegister = value; OnPropertyChanged(); }
        }

        public string UserLogin
        {
            get => _UserLogin;
            set { Set(ref _UserLogin, value); CheckCanAttemptToRegister(); }
        }

        public string Nickname
        {
            get => _Nickname;
            set { Set(ref _Nickname, value); CheckCanAttemptToRegister(); }
        }

        public SecureString SecurePassword
        {
            get => _SecurePassword;
            set { Set(ref _SecurePassword, value); CheckCanAttemptToRegister(); }
        }

        public SecureString ConfirmSecurePassword
        {
            get => _ConfirmSecurePassword;
            set { Set(ref _ConfirmSecurePassword, value); CheckCanAttemptToRegister(); }
        }

        public ImageSource Picture
        {
            get => _picture;
            set => Set(ref _picture, value);
        }

        public Command Register => _register ??= new Command(async obj =>
        {
            try
            {
                Messenger.CallCommand("loading", "MainViewModel");

                string password = new NetworkCredential("", SecurePassword).Password;
                string passwordConfirm = new NetworkCredential("", ConfirmSecurePassword).Password;

                if (password.Length < 6)
                {
                    NotifyWindow window = new("Passwords must contains atleast 6 characters");
                    window.Show();
                    return;
                }

                else
                {
                    if (password.Equals(passwordConfirm))
                    {
                        try
                        {
                            UserDto userDto = new()
                            {
                                Username = UserLogin,
                                Password = password,
                                Token = String.Empty,
                                UserPic = string.IsNullOrEmpty(_picturePath) ? GetDefaultPictureBytes() : TryExtractPictureBytes(_picturePath)
                            };

                            await new Client(baseUrl: ConfigurationManager.AppSettings["BaseUrl"]).RegisterAsync(userDto);

                            NotifyWindow okWindow = new("Successful");
                            okWindow.Show();

                            Messenger.CallCommand("auth", "MainViewModel");
                        }
                        catch (Exception e)
                        {
                            if (e is ApiException exception)
                            {
                                NotifyWindow window = new(exception.Response);
                                window.Show();
                            }
                            else
                                throw;
                        }
                    }
                    else
                    {
                        NotifyWindow window = new("Passwords not match");
                        window.Show();
                        return;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                Messenger.CallCommand("loading", "MainViewModel");
            }
        });

        public Command Back => _back ??= new Command(obj =>
        {
            Messenger.CallCommand("auth", "MainViewModel");
        });
        
        public Command SelectPicture => _selectPicture ??= new Command(obj =>
        {
            try
            {
                OpenFileDialog dialog = new();
                dialog.DefaultExt = ".png";
                dialog.Filter = "JPG(*.jpg)|*.jpg|PNG(*.png)|*.png|JPEG(*.jpeg)|*.jpeg|BMP(*.bmp)|*.bmp";
                dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                dialog.Title = "Choose image";
                dialog.Multiselect = false;
                dialog.ShowDialog();

                if (string.IsNullOrEmpty(dialog.FileName))
                {
                    return;
                }
                else
                {
                    _picturePath = dialog.FileName;
                    Picture = new BitmapImage(new Uri(dialog.FileName));
                }
            }
            catch (Exception)
            {
                NotifyWindow window = new("Incorrect image. Check file format");
                window.Show();
            }
        });

        private void CheckCanAttemptToRegister()
        {
            CanAttemptToRegister = SecurePassword != null &&
                ConfirmSecurePassword != null &&
                SecurePassword.Length > 5 &&
                ConfirmSecurePassword.Length > 5 &&
                SecureStringToString(SecurePassword) == SecureStringToString(ConfirmSecurePassword) &&
                !String.IsNullOrEmpty(UserLogin);
        }

        private static string SecureStringToString(SecureString value)
        {
            IntPtr valuePtr = IntPtr.Zero;
            try
            {
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(value);
                return Marshal.PtrToStringUni(valuePtr) ?? string.Empty;
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
        }

        private static byte[]? TryExtractPictureBytes(string path)
        {
            try
            {
                if (File.Exists(path))
                    return File.ReadAllBytes(path);

                else
                    return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static byte[]? GetDefaultPictureBytes()
        {
            try
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                string iconName = assembly.GetManifestResourceNames().Where(x => x.Contains("defaultPic")).FirstOrDefault();

                using Stream resourceStream = assembly.GetManifestResourceStream(iconName);
                byte[] bytes = new byte[resourceStream.Length];
                resourceStream.Read(bytes, 0, bytes.Length);

                return bytes;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
