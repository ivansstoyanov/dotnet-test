using MongoDB.Driver;
using ShishaWeb.MongoModels;
using ShishaWeb.Repositories.Interfaces;
using ShishaWeb.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShishaWeb.Services
{
    public class ActivityService : IActivityService
    {
        private readonly IActivityRepository activityRepository = null;

        public ActivityService(IActivityRepository activityRepository)
        {
            this.activityRepository = activityRepository;
        }

        public async Task<IEnumerable<Activity>> GetUserActivity(string id)
        {
            var filter = Builders<Activity>.Filter.Eq("Id", id);
            var activities = await activityRepository.GetMany(filter);

            return activities.Entities;
        }

        //public async Task<User> GetUserRelatedActivity(string id)
        //{
        //    var user = await usersRepository.GetOne(id);

        //    return user.Entity;
        //}

        public async Task<Activity> Add(Activity activity)
        {
            var result = await activityRepository.AddOne(activity);

            //TODO return created activity!?
            return activity;
        }
    }
}
