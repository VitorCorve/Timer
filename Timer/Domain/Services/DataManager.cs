using Newtonsoft.Json;

using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;

using TimerApiHelper.TimerApiHelper;

namespace Timer.Domain.Services
{
    public class DataManager
    {
        private const string Filename = "data.json";
        private static string _directory = string.Empty;
        public async static Task Save(StatisticsConfiguration statistics)
        {
            InitDir();

            string filepath = Path.Combine(_directory, Filename);

            using StreamWriter sw = new(filepath);

            await sw.WriteLineAsync(JsonConvert.SerializeObject(statistics, Formatting.Indented));
        }

        public async static Task<StatisticsConfiguration> Load()
        {
            InitDir();

            string filepath = Path.Combine(_directory, Filename);

            if (File.Exists(filepath))
            {
                using StreamReader sr = new(filepath);
                string json = await sr.ReadToEndAsync();

                StatisticsConfiguration statistics = JsonConvert.DeserializeObject<StatisticsConfiguration>(json);

                return statistics ?? new StatisticsConfiguration();
            }
            else
                return new StatisticsConfiguration();

        }

        public async static Task<StatisticsConfiguration> Download()
        {
            StatisticsComposite response = await new Client(ConfigurationManager.AppSettings["BaseUrl"], token: UserData.Token).UserInfoAsync();

            StatisticsConfiguration configuration = new()
            {
                LastInitDay = response.LastInitDay,
                LastInitMonth = response.LastInitMonth,
                LastInitWeekNumber = response.LastInitWeekNumber,
                SecondsMonthCount = response.SecondsMonthCount,
                SecondsTodayCount = response.SecondsTodayCount,
                SecondsTotalCount = response.SecondsTotalCount,
                SecondsWeekCount = response.SecondsWeekCount
            };

            return configuration;
        }

        private static void InitDir()
        {
            _directory = Path.Combine(Environment.CurrentDirectory, "Data");

            if (Directory.Exists(_directory))
            {
                return;
            }
            else
                Directory.CreateDirectory(_directory);
        }
    }
}
