namespace Timer.Domain
{
    public class StatisticsConfiguration
    {
        public int LastInitMonth { get; set; }
        public int LastInitDay { get; set; }
        public int LastInitWeekNumber { get; set; }
        public int SecondsTotalCount { get; set; }
        public int SecondsWeekCount { get; set; }
        public int SecondsMonthCount { get; set; }
        public int SecondsTodayCount { get; set; }
    }
}
