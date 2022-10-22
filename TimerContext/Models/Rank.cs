using System.ComponentModel.DataAnnotations;

namespace TimerContext.Models
{
    public class Rank
    {
        [Key]
        public int RankId { get; set; }
        public string Name { get; set; } = string.Empty;
        public byte[] Picture { get; set; }
    }
}
