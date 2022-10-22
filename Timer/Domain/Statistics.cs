using System;
using System.Configuration;
using System.Globalization;
using System.Windows;
using System.Windows.Threading;

using Timer.Domain.Services;
using Timer.Internal;
using Timer.ViewModel;

using TimerApiHelper.TimerApiHelper;

namespace Timer.Domain
{
    [Serializable]
    public class Statistics : PropertyChangedAbstract
    {
        public Statistics()
        {

        }
        public StatisticsConfiguration PrepareForSerialize()
        {
            StatisticsConfiguration model = new()
            {
                LastInitDay = _lastInitDay,
                LastInitMonth = _lastInitMonth,
                LastInitWeekNumber = _lastInitWeekNumber,
                SecondsMonthCount = _secondsMonthCount,
                SecondsTodayCount = _secondsTodayCount,
                SecondsTotalCount = _secondsTotalCount,
                SecondsWeekCount = _secondsWeekCount,
            };

            return model;
        }

        private string _spentToday;

        private int _hoursThisWeek,
                    _hoursThisMonth,
                    _hoursTotal;

        private int _secondsTotalCount,
                    _secondsWeekCount,
                    _secondsMonthCount,
                    _secondsTodayCount;

        private int _lastInitMonth,
                    _lastInitDay,
                    _lastInitWeekNumber;

        private Calendar _calendar;

        private System.Timers.Timer _timer;

        private Command _engageTimerCommand;

        public string SpentToday
        {
            get => _spentToday;
            set { _spentToday = value; OnPropertyChanged(); }
        }

        public int HoursThisWeek
        {
            get { return _hoursThisWeek; }
            set { _hoursThisWeek = value; OnPropertyChanged(); }
        }

        public int HoursThisMonth
        {
            get { return _hoursThisMonth; }
            set { _hoursThisMonth = value; OnPropertyChanged(); }
        }

        public int HoursTotal
        {
            get { return _hoursTotal; }
            set { _hoursTotal = value; OnPropertyChanged(); }
        }


        public Command EngageTimerCommand => _engageTimerCommand ??= new Command(obj =>
        {
            EngageTimer();
        });

        private void Configure(StatisticsConfiguration model)
        {
            _lastInitDay = model.LastInitDay;
            _lastInitMonth = model.LastInitMonth;
            _lastInitWeekNumber = model.LastInitWeekNumber;
            _secondsMonthCount = model.SecondsMonthCount;
            _secondsTodayCount = model.SecondsTodayCount;
            _secondsTotalCount = model.SecondsTotalCount;
            _secondsWeekCount = model.SecondsWeekCount;

            SpentToday = TimeSpan.FromSeconds(_secondsTodayCount).ToString();
            HoursThisWeek = (int)TimeSpan.FromSeconds(_secondsWeekCount).TotalHours;
            HoursThisMonth = (int)TimeSpan.FromSeconds(_secondsMonthCount).TotalHours;
            HoursTotal = (int)TimeSpan.FromSeconds(_secondsTotalCount).TotalHours;
        }

        public void Init(StatisticsConfiguration model)
        {
            Configure(model);

            _timer = new System.Timers.Timer(1000);

            _calendar = new CultureInfo("en-US").Calendar;

            int ticks = 0;

            _timer.Elapsed += (obj, val) =>
            {
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Send, new Action(async () =>
                {
                    _secondsTotalCount++;
                    _secondsWeekCount++;
                    _secondsMonthCount++;
                    _secondsTodayCount++;

                    int week = _calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                    int day = _calendar.GetDayOfYear(DateTime.Now);

                    SpentToday = TimeSpan.FromSeconds(_secondsTodayCount).ToString();
                    HoursThisWeek = (int)TimeSpan.FromSeconds(_secondsWeekCount).TotalHours;
                    HoursThisMonth = (int)TimeSpan.FromSeconds(_secondsMonthCount).TotalHours;
                    HoursTotal = (int)TimeSpan.FromSeconds(_secondsTotalCount).TotalHours;

                    if (DateTime.Now.Month > _lastInitMonth)
                    {
                        _lastInitMonth = DateTime.Now.Month;
                        _secondsMonthCount = 0;
                    }

                    if (week > _lastInitWeekNumber)
                    {
                        _lastInitWeekNumber = week;
                        _secondsWeekCount = 0;
                    }

                    if (day > _lastInitDay)
                    {
                        _lastInitDay = day;
                        _secondsTodayCount = 0;
                    }

                    await DataManager.Save(this.PrepareForSerialize());

                    ticks++;

                    if (ticks == 60)
                    {
                        await new Client(ConfigurationManager.AppSettings["BaseUrl"], token: UserData.Token).UpdateDataAsync(_secondsTodayCount);
                        ticks = 0;
                    }

                }));
            };
        }

        public void EngageTimer()
        {
            if (_timer.Enabled)
            {
                _timer.Stop();
            }

            else
                _timer.Start();
        }
    }
}
