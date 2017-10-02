using ShishaWeb.MongoModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShishaWeb.Services.Interfaces
{
    public interface IActivityService
    {
        Task<IEnumerable<Activity>> GetUserActivity(string id);

        Task<Activity> Add(Activity activity);
    }
}
