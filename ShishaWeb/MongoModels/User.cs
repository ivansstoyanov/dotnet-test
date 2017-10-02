using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ShishaWeb.Config.Enumerations;
using ShishaWeb.Models;
namespace ShishaWeb.MongoModels
{
    public class User : BaseAuditedEntity
    {
        public User()
        {
            this.ShishasSmokedList = new string[0];
            this.ShishasFavouriteList = new string[0];
            this.ShishasWishedList = new string[0];
            this.Following = new string[0];
            this.Followers = new string[0];
            this.Settings = new UserSettings();
            this.Badges = new UserBadge[0];
            this.UserGroup = UserGroupEnum.User;
        }

        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Username { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PhotoUrl { get; set; } = string.Empty;

        public string Picture { get; set; } = string.Empty;

        public int ShishasSmoked { get; set; } = 0;

        public int ShishasCreated { get; set; } = 0;

        public string[] ShishasSmokedList { get; set; }

        public string[] ShishasFavouriteList { get; set; }

        public string[] ShishasWishedList { get; set; }

        public string[] Following { get; set; }

        public string[] Followers { get; set; }

        public UserBadge[] Badges { get; set; }

        public UserGroupEnum UserGroup { get; set; }

        public UserSettings Settings { get; set; }

        public string FirebaseToken { get; set; } = string.Empty;

        public FirebaseUser FirebaseInfo { get; set; }
    }
}
