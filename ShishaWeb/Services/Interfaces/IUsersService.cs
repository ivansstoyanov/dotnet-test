using ShishaWeb.MongoModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShishaWeb.Services.Interfaces
{
    public interface IUsersService
    {
        Task<User> GetByEmail(string email);

        Task<User> Get(string id);

        Task<User> GetCurrentUser();

        Task<IEnumerable<User>> GetAll();

        Task<IEnumerable<User>> GetUserFollowers(string id);

        Task<IEnumerable<User>> GetUserFollowing(string id);

        Task<IEnumerable<string>> AddFollower(string myId, string followedId);

        Task<IEnumerable<string>> RemoveFollower(string myId, string followedId);

        Task<User> Add(User user);

        Task<User> UpdateFields(string id, Dictionary<string, object> changes);

        Task<UserSettings> UpdateUserSettings(string myId, UserSettings userSettings);
    }
}
