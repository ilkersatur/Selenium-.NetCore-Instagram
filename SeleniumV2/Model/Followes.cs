using System.Diagnostics;

namespace SeleniumV2.Model
{
    public class Followers
    {
        public List<string> Follower { get; set; }

        public int FollowerCount { get; set; } = 0;

        public TimeSpan Duration { get; set; }
    }
}
