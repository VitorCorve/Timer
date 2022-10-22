using System.ComponentModel.DataAnnotations;

namespace TimerContext.Models
{
    public class Statistics
    {
        [Key]
        public int StatisticsId { get; set; }
        public int LastInitDay { get; set; }
        public int LastInitMonth { get; set; }
        public int LastInitWeekNumber { get; set; }
        public int SecondsMonthCount { get; set; }
        public int SecondsTodayCount { get; set; }
        public int SecondsTotalCount { get; set; }
        public int SecondsWeekCount { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public Guid LastUpdateToken { get; set; }
    }
}
