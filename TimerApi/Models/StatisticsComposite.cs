namespace TimerApi.Models
{
    public class StatisticsComposite
    {
        public string Username { get; set; }
        public byte[] UserPic { get; set; }
        public DateTime DateRegister { get; set; }
        public DateTime LastSeen { get; set; }
        public int SecondsMonthCount { get; set; }
        public int SecondsTodayCount { get; set; }
        public int SecondsTotalCount { get; set; }
        public int SecondsWeekCount { get; set; }
        public int LastInitDay { get; set; }
        public int LastInitMonth { get; set; }
        public int LastInitWeekNumber { get; set; }
        public string UserStatus { get; set; }
    }
}
