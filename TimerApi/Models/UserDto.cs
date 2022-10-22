namespace TimerApi.Models
{
    public class UserDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string UserIdentifier { get; set; } = string.Empty;
        public byte[]? UserPic { get; set; }
    }
}
