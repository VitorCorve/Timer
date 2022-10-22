using System.ComponentModel.DataAnnotations;

namespace TimerContext.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public int StatisticsId { get; set; }
        public int RankId { get; set; }
        public string Username { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string UserStatus { get; set; }
        public byte[] UserPic { get; set; }
        public DateTime DateRegister { get; set; }
        public DateTime LastSeen { get; set; }
        public Guid UserIdentifier { get; set; }
    }
}
