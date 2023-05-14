using System.ComponentModel;

namespace BeSaraha.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        [DisplayName("Profile Picture")]
        public string Picture { get; set; }
        public string ProfileUrl { get; set; } = Random.Shared.Next(10000, 100000).ToString();
        public int followersCount { get; set; } = 0;
        public int followingCount { get; set; } = 0;
        public int messagesCount { get; set; } = 0;

    }
}
