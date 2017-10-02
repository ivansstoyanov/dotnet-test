namespace ShishaWeb.Models
{
    public class UserShortInfoDTO
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Username { get; set; }

        public string PhotoUrl { get; set; }

        public int FollowersCount { get; set; }

        public int FollowingCount { get; set; }

        public bool IsFollowed { get; set; }
    }
}
