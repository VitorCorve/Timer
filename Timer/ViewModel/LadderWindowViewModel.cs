using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Windows;

using Timer.Domain;
using Timer.Internal;

using TimerApiHelper.TimerApiHelper;

namespace Timer.ViewModel
{
    public class LadderWindowViewModel : PropertyChangedAbstract
    {

        public LadderWindowViewModel()
        {
            StartObserving();
        }

        private void StartObserving()
        {
            Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Send, new Action(() =>
            {
                UpdateLadder.Execute(null);
            }));

            System.Timers.Timer timer = new(60000);

            timer.Elapsed += (o, e) => Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Send, new Action(() =>
            {
                UpdateLadder.Execute(null);
            }));

            timer.Start();
        }

        private Command _updateLadder;

        private ICollection<StatisticsComposite> _statisticsComposites;

        private ObservableCollection<StatisticsComposite> _statistics;

        public ObservableCollection<StatisticsComposite> Statistics
        {
            get => _statistics;
            set => Set(ref _statistics, value);
        }
        public Command UpdateLadder => _updateLadder ??= new Command(async obj =>
        {
            _statisticsComposites = await new Client(ConfigurationManager.AppSettings["BaseUrl"], token: UserData.Token).GetStatisticsAsync();

            UpdateStatistics(_statisticsComposites);
        });

        private void UpdateStatistics(ICollection<StatisticsComposite> statistics)
        {
            Statistics = Statistics ??= new ObservableCollection<StatisticsComposite>();
            Statistics.Clear();

            double daysIn = 0.0;
            int secondsPerDay = 0;

            foreach (StatisticsComposite item in statistics)
            {
                daysIn = (DateTime.Now - item.DateRegister).TotalDays;
                secondsPerDay = (int)(item.SecondsTotalCount / daysIn);
                item.AverageInDay = TimeSpan.FromSeconds(secondsPerDay).ToString();

                Statistics.Add(item);
            }

        }
    }
}
