namespace ShishaWeb.Models
{
    public class UserDTO : BaseAuditDTO
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Username { get; set; }

        public string PhotoUrl { get; set; }

        public int ShishasSmoked { get; set; }

        public int ShishasCreated { get; set; }

        public string[] ShishasSmokedList { get; set; }

        public string[] ShishasFavouriteList { get; set; }

        public string[] ShishasWishedList { get; set; }

        public string[] Following { get; set; }

        public string[] Followers { get; set; }

        public int FollowersCount { get; set; }

        public int FollowingCount { get; set; }

        public UserBadgeDTO[] Badges { get; set; }

        public string UserGroup { get; set; }

        public UserSettingsDTO Settings { get; set; }
    }
}
