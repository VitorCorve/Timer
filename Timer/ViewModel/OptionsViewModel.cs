using Microsoft.Win32;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Timer.Domain;
using Timer.Internal;
using Timer.Internal.Models;
using Timer.Internal.Services;
using Timer.Views;

using TimerApiHelper.TimerApiHelper;

namespace Timer.ViewModel
{
    public class OptionsViewModel : PropertyChangedAbstract
    {
        private string _userStatus;

        private Command _getCurrentUser, _save, _back, _selectPicture;

        private byte[] _picture;

        public byte[] Picture
        {
            get => _picture;
            set => Set(ref _picture, value);
        }

        public string UserStatus
        {
            get => _userStatus;
            set => Set(ref _userStatus, CutStatusString(value));
        }
        public OptionsViewModel()
        {
            Messenger.Register(new MessengerEntity(this, "OptionsViewModel"));
        }

        public Command GetCurrentUser => _getCurrentUser ??= new Command(async obj =>
        {
            StatisticsComposite response = await new Client(ConfigurationManager.AppSettings["BaseUrl"], token: UserData.Token).UserInfoAsync();
            UserStatus = response.UserStatus;
            Picture = response.UserPic;
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
                    Picture = File.ReadAllBytes(dialog.FileName);
                }
            }
            catch (Exception)
            {
                NotifyWindow window = new("Incorrect image. Check file format");
                window.Show();
            }
        });

        public Command Save => _save ??= new Command(async obj =>
        {
            await new Client(ConfigurationManager.AppSettings["BaseUrl"], token: UserData.Token).UpdateAsync(UserStatus, Picture);

            NotifyWindow window = new("Successfull");
            window.Show();
        });

        public Command Back => _back ??= new Command(async obj =>
        {
            Messenger.CallCommand("clocks", "MainViewModel");
        });

        public override void ExecuteExternalCommand(string command)
        {
            switch (command)
            {
                case "getUser":
                    GetCurrentUser.Execute(null);
                    break;
            }
        }

        private static string CutStatusString(string status) => status.Length > 60 ? status[..60] : status;
    }
}
