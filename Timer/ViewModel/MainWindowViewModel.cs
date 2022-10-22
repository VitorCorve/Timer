using System;
using System.Threading.Tasks;
using System.Windows;

using Timer.Domain;
using Timer.Domain.Services;
using Timer.Internal;
using Timer.Internal.Models;
using Timer.Internal.Services;
using Timer.Views;

namespace Timer.ViewModel
{
    public class MainWindowViewModel : PropertyChangedAbstract
    {
        public MainWindowViewModel()
        {
            ShowAuth.Execute(null);

            InitDateClock();

            Messenger.Register(new MessengerEntity(this, "MainViewModel"));

            LoadingVisibility = Visibility.Hidden;

        }

        private int _hour, 
                    _minute, 
                    _second;

        private string _currentDate;

        private Visibility _authVisibility,
                           _luaVisibility,
                           _registerVisibility,
                           _loadingVisibility,
                           _clocksVisibility,
                           _optionsVisibility;

        private Statistics _statistics;

        private Command _showAuth,
                        _showLua,
                        _showRegister,
                        _showClocks,
                        _handleLoading,
                        _exit,
                        _showOptions,
                        _showLadder,
                        _initialize;

        public int Hour
        {
            get { return _hour; }
            set { _hour = value; OnPropertyChanged(); }
        }

        public int Minute
        {
            get { return _minute; }
            set { _minute = value; OnPropertyChanged(); }
        }

        public int Second
        {
            get { return _second; }
            set { _second = value; OnPropertyChanged(); }
        }

        public string CurrentDate
        {
            get => _currentDate;
            set { _currentDate = value; OnPropertyChanged(); }
        }

        public Visibility AuthVisibility
        {
            get => _authVisibility;
            set { _authVisibility = value; OnPropertyChanged(); }
        }

        public Visibility LuaVisibility
        {
            get => _luaVisibility;
            set { _luaVisibility = value; OnPropertyChanged(); }
        }

        public Visibility RegisterVisibility
        {
            get => _registerVisibility;
            set { _registerVisibility = value; OnPropertyChanged(); }
        }
        
        public Visibility LoadingVisibility
        {
            get => _loadingVisibility;
            set { _loadingVisibility = value; OnPropertyChanged(); }
        }
        
        public Visibility ClocksVisibility
        {
            get => _clocksVisibility;
            set { _clocksVisibility = value; OnPropertyChanged(); }
        }
              
        public Visibility OptionsVisibility
        {
            get => _optionsVisibility;
            set { _optionsVisibility = value; OnPropertyChanged(); }
        }

        public Statistics Statistics
        {
            get => _statistics;
            set { _statistics = value; OnPropertyChanged(); }
        }

        public Command ShowAuth => _showAuth ??= new Command(obj =>
        {
            AuthVisibility = Visibility.Visible;
            LuaVisibility = Visibility.Hidden;
            RegisterVisibility = Visibility.Hidden;
            ClocksVisibility = Visibility.Hidden;
            OptionsVisibility = Visibility.Hidden;
        });

        public Command ShowLua => _showLua ??= new Command(obj =>
        {
            AuthVisibility = Visibility.Hidden;
            LuaVisibility = Visibility.Visible;
            RegisterVisibility = Visibility.Hidden;
            ClocksVisibility = Visibility.Hidden;
            OptionsVisibility = Visibility.Hidden;
        });

        public Command ShowRegister => _showRegister ??= new Command(obj =>
        {
            AuthVisibility = Visibility.Hidden;
            LuaVisibility = Visibility.Hidden;
            RegisterVisibility = Visibility.Visible;
            ClocksVisibility = Visibility.Hidden;
            OptionsVisibility = Visibility.Hidden;
        });
        
        public Command HandleLoading => _handleLoading ??= new Command(obj =>
        {
            LoadingVisibility = LoadingVisibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
        });
        
        public Command ShowClocks => _showClocks ??= new Command(obj =>
        {
            AuthVisibility = Visibility.Hidden;
            LuaVisibility = Visibility.Hidden;
            RegisterVisibility = Visibility.Hidden;
            ClocksVisibility = Visibility.Visible;
            OptionsVisibility = Visibility.Hidden;

            Initialize.Execute(null);
        });
              
        public Command Exit => _exit ??= new Command(obj =>
        {

        });
              
        public Command ShowOptions => _showOptions ??= new Command(obj =>
        {
            AuthVisibility = Visibility.Hidden;
            LuaVisibility = Visibility.Hidden;
            RegisterVisibility = Visibility.Hidden;
            ClocksVisibility = Visibility.Hidden;
            OptionsVisibility = Visibility.Visible;

            Messenger.CallCommand("getUser", "OptionsViewModel");
        });
              
        public Command ShowLadder => _showLadder ??= new Command(obj =>
        {
            LadderWindow window = new();
            window.Show();
        });

        public Command Initialize => _initialize ??= new Command(async obj =>
        {
            System.Timers.Timer timer = new(1000);

            Statistics = new Statistics();

            StatisticsConfiguration configuration = await DataManager.Download();

            Statistics.Init(configuration);

            timer.Elapsed += (sender, e) => Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Send, new Action(() =>
            {
                InitDateClock();
            }));

            timer.Start();

        });

        private void InitDateClock()
        {
            Hour = DateTime.Now.Hour % 12;
            Minute = DateTime.Now.Minute;
            Second = DateTime.Now.Second;
            CurrentDate = Convert(DateTime.Now);
        }
        private static string Convert(DateTime date)
        {
            string description = date.Day.ToString() + " ";

            description = date.DayOfWeek switch
            {
                DayOfWeek.Monday => description + "MON ",
                DayOfWeek.Tuesday => description + "TUE ",
                DayOfWeek.Wednesday => description + "WED ",
                DayOfWeek.Thursday => description + "THU ",
                DayOfWeek.Friday => description + "FRI ",
                DayOfWeek.Saturday => description + "SAT ",
                _ => description + "SUN ",
            };

            description += date.Month.ToString() + " ";

            description += date.ToString("yy");

            return description;
        }

        public override void ExecuteExternalCommand(string command)
        {
            switch (command)
            {
                case "auth":
                    ShowAuth.Execute(null);
                    break;
                case "register":
                    ShowRegister.Execute(null);
                    break;
                case "lua":
                    ShowLua.Execute(null);
                    break;
                case "clocks":
                    ShowClocks.Execute(null);
                    break;
                case "options":
                    ShowOptions.Execute(null);
                    break;
                case "loading":
                    HandleLoading.Execute(null);
                    break;
                default:
                    break;
            }
        }
        public override void ExecuteExternalCommand(string command, string commandParameter)
        {
        }
    }
}
