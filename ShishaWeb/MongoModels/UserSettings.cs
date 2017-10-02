using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ShishaWeb.MongoModels
{
    public class UserSettings
    {
        public bool IsProfileVisible { get; set; } = true;

        public bool IsActivityVisible { get; set; } = true;

        public bool IsNargilesVisible { get; set; } = true;

        public bool AllowNotifications { get; set; } = true;

        public string Language { get; set; } = "en";
    }
}
