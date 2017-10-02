namespace ShishaWeb.Models
{
    public class UserSettingsDTO
    {
        public bool IsProfileVisible { get; set; }

        public bool IsActivityVisible { get; set; }

        public bool IsNargilesVisible { get; set; }

        public bool AllowNotifications { get; set; }

        public string Language { get; set; }
    }
}
