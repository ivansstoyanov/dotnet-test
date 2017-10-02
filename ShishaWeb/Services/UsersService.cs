using MongoDB.Driver;
using ShishaWeb.Config.Enumerations;
using ShishaWeb.MongoModels;
using ShishaWeb.Repositories.Interfaces;
using ShishaWeb.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShishaWeb.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository usersRepository = null;
        private readonly IActivityService activityService = null;
        private readonly IAuditProvider auditProvider = null;

        public UsersService(IUsersRepository usersRepository, IAuditProvider auditProvider, IActivityService activityService)
        {
            this.usersRepository = usersRepository;
            this.activityService = activityService;
            this.auditProvider = auditProvider;
        }

        public async Task<User> GetByEmail(string email)
        {
            var filter = Builders<User>.Filter.Eq("Email", email);
            var user = await usersRepository.GetOne(filter);

            return user.Entity;
        }

        public async Task<User> Get(string id)
        {
            var user = await usersRepository.GetOne(id);

            return user.Entity;
        }

        public async Task<User> GetCurrentUser()
        {
            if (this.auditProvider.CurrentUserEmail == null)
            {
                return null;
            }

            return await this.GetByEmail(this.auditProvider.CurrentUserEmail);
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            var users = await usersRepository.GetAll();

            return users.Entities;
        }

        public async Task<IEnumerable<User>> GetUserFollowers(string id)
        {
            var user = await this.Get(id);
            var filter = Builders<User>.Filter.In(x => x.Id, user.Followers);
            var users = await usersRepository.GetMany(filter);

            return users.Entities;
        }

        public async Task<IEnumerable<User>> GetUserFollowing(string id)
        {
            var user = await this.Get(id);
            var filter = Builders<User>.Filter.In(x => x.Id, user.Following);
            var users = await usersRepository.GetMany(filter);

            return users.Entities;
        }

        //TODO ask for user permisiion first(user settings) - save request in database document
        public async Task<IEnumerable<string>> AddFollower(string myId, string followedId)
        {
            var myUser = await this.Get(myId);
            var followedUser = await this.Get(followedId);

            var myFollowing = myUser.Following.ToList();
            if (!myFollowing.Contains(followedId))
            {
                myFollowing.Add(followedId);

                var update = Builders<User>.Update.Set("Following", myFollowing.ToArray());
                await this.usersRepository.UpdateOne(myId, update);

#pragma warning disable 4014
                this.activityService.Add(new Activity() {
                  UserId = myId,
                  Type = ActivityTypeEnum.StartFollowingUser
                });
#pragma warning restore 4014
            }

            var followedFollowers = followedUser.Followers.ToList();
            if (!followedFollowers.Contains(myId))
            {
                followedFollowers.Add(myId);

                var update = Builders<User>.Update.Set("Followers", followedFollowers.ToArray());
                await this.usersRepository.UpdateOne(followedId, update);

#pragma warning disable 4014
                this.activityService.Add(new Activity()
                {
                    UserId = followedId,
                    ItemId = myId,
                    Type = ActivityTypeEnum.UserHaveBeenFollowed
                });
#pragma warning restore 4014
            }

            return myFollowing;
        }

        public async Task<IEnumerable<string>> RemoveFollower(string myId, string followedId)
        {
            var myUser = await this.Get(myId);
            var followedUser = await this.Get(followedId);

            var myFollowing = myUser.Following.ToList();
            if (myFollowing.Contains(followedId))
            {
                myFollowing.Remove(followedId);

                var update = Builders<User>.Update.Set("Following", myFollowing.ToArray());
                await this.usersRepository.UpdateOne(myId, update);
            }

            var followedFollowers = followedUser.Followers.ToList();
            if (followedFollowers.Contains(myId))
            {
                followedFollowers.Remove(myId);

                var update = Builders<User>.Update.Set("Followers", followedFollowers.ToArray());
                await this.usersRepository.UpdateOne(followedId, update);
            }

            return myFollowing;
        }

        public async Task<User> Add(User user)
        {
            var result = await usersRepository.AddOne(user);

            //return created User!?
            return user;
        }

        public async Task<User> UpdateFields(string id, Dictionary<string, object> changes)
        {
            var updateBuilders = new List<UpdateDefinition<User>>();
            foreach (KeyValuePair<string, object> entry in changes)
            {
                updateBuilders.Add(Builders<User>.Update.Set(entry.Key, (dynamic)(Convert.ChangeType(entry.Value, entry.Value.GetType()))));
            }
            var update = Builders<User>.Update.Combine(updateBuilders);

            await this.usersRepository.UpdateOne(id, update);

            return await this.Get(id);
        }

        //TODO ask for user permisiion first(user settings) - save request in database document
        //TODO consider removing this method and using only UpdateFields
        public async Task<UserSettings> UpdateUserSettings(string myId, UserSettings userSettings)
        {
            await this.UpdateFields(myId, new Dictionary<string, object>
            {
                { "Settings", userSettings }
            });

            return userSettings;
        }
    }
}
