using MongoDB.Driver;
using ShishaWeb.Config.Enumerations;
using ShishaWeb.Exceptions;
using ShishaWeb.MongoModels;
using ShishaWeb.Repositories.Interfaces;
using ShishaWeb.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShishaWeb.Services
{
    public class ShishasService : IShishasService
    {
        private readonly IUsersService usersService = null;
        private readonly IActivityService activityService = null;
        private readonly ITabaccoService tabaccoService = null;
        private readonly IShishasRepository shishasRepository = null;
        private readonly IShishaSmokersRepository shishaSmokersRepository = null;

        public ShishasService(
            IShishasRepository shishasRepository, 
            IShishaSmokersRepository shishaSmokersRepository, 
            ITabaccoService tabaccoService, 
            IUsersService usersService, 
            IActivityService activityService)
        {
            this.usersService = usersService;
            this.activityService = activityService;
            this.tabaccoService = tabaccoService;
            this.shishasRepository = shishasRepository;
            this.shishaSmokersRepository = shishaSmokersRepository;
        }

        public async Task<Shisha> Get(string id)
        {
            var shisha = await shishasRepository.GetOne(id);

            return shisha.Entity;
        }

        public async Task<IEnumerable<Shisha>> GetMany(string[] ids)
        {
            var shishas = await shishasRepository.GetMany(ids);

            return shishas.Entities;
        }

        public async Task<IEnumerable<Shisha>> GetPublicShishas()
        {
            var filter = Builders<Shisha>.Filter.Eq("IsPublic", true);
            var publicShishas = await shishasRepository.GetMany(filter);

            return publicShishas.Entities;
        }

        //only for admin(global) shishas
        public async Task<Shisha> Add(Shisha shisha)
        {
            var currentUser = await this.usersService.GetCurrentUser();
            var shishaKey = this.GetShishaIdentifier(shisha);
            //TODO allow only for admin to continue

            var publicShishas = await this.GetPublicShishas();

            var findPublicShisha = publicShishas?.FirstOrDefault(s => s.Identifier == shishaKey);
            if (findPublicShisha != null)
            {
                throw new ItemAlreadyExistException(shisha);
            }

            var newShisha = new Shisha()
            {
                Identifier = shishaKey,
                PublicId = null,
                UserId = currentUser.Id,
                Name = shisha.Name,
                //Description = shisha.Description,
                //Type = shisha.Type,
                //Tag = shisha.Tag,
                IsPublic = true,
                IsWish = false,
                IsFavourite = false,
                IsSeasonal = false, //TODO maybe
                Power = this.GetShishaPower(shisha),
                Tabaccos = shisha.Tabaccos,
                TotalRating = 1,
                OwnRating = 0,
            };

            await shishasRepository.AddOne(newShisha);

#pragma warning disable 4014
            this.activityService.Add(new Activity()
            {
                UserId = currentUser.Id,
                ItemId = newShisha.Id,
                Type = ActivityTypeEnum.GlobalShishaCreated
            });
#pragma warning restore 4014

            return newShisha;
        }

        public async Task<Shisha> AddToWishList(Shisha shisha)
        {
            var currentUser = await this.usersService.GetCurrentUser();
            var shishaIdentifier = this.GetShishaIdentifier(shisha);

            var findSmokedShisha = await this.FindShishaByIdentifier(currentUser.ShishasSmokedList, shishaIdentifier);
            if (findSmokedShisha)
            {
                throw new ItemAlreadyExistException(shisha, MessageCodes.ALREADY_EXISTS_IN_SMOKED);
            }

            var findWishedShisha = await this.FindShishaByIdentifier(currentUser.ShishasWishedList, shishaIdentifier);
            if (findWishedShisha)
            {
                throw new ItemAlreadyExistException(shisha, MessageCodes.ALREADY_EXISTS_IN_WISHED);
            }

            var newShisha = new Shisha()
            {
                Identifier = shishaIdentifier,
                PublicId = null,
                UserId = currentUser.Id,
                Name = shisha.Name,
                //Description = shisha.Description,
                //Type = shisha.Type,
                //Tag = shisha.Tag,
                SmokedCount = 0,
                IsPublic = false,
                IsWish = true,
                IsFavourite = false,
                IsSeasonal = false,
                Power = this.GetShishaPower(shisha),
                Tabaccos = shisha.Tabaccos,
                //  TotalRating
                //  OwnRating
            };

            await shishasRepository.AddOne(newShisha);

            var newWished = currentUser.ShishasWishedList.ToList();
            newWished.Add(newShisha.Id);
            await this.usersService.UpdateFields(currentUser.Id, new Dictionary<string, object>
            {
                { "ShishasWishedList", newWished.ToArray() }
            });

#pragma warning disable 4014
            this.activityService.Add(new Activity()
            {
                UserId = currentUser.Id,
                ItemId = newShisha.Id,
                Type = ActivityTypeEnum.UserShishaCreated
            });
#pragma warning restore 4014

            return newShisha;
        }

        public async Task<Shisha> AddToSmokeList(Shisha shisha, string publicId, string newIdentifier)
        {
            if(newIdentifier != null)
            {
                shisha = await this.CreateShishaFromIdentifier(newIdentifier);
            }
            var currentUser = await this.usersService.GetCurrentUser();
            var shishaIdentifier = this.GetShishaIdentifier(shisha);

            var findSmokedShisha = await this.FindShishaByIdentifier(currentUser.ShishasSmokedList, shishaIdentifier);
            if (findSmokedShisha)
            {
                throw new ItemAlreadyExistException(shisha, MessageCodes.ALREADY_EXISTS_IN_SMOKED);
            }

            var newShisha = new Shisha()
            {
                Identifier = shishaIdentifier,
                PublicId = publicId ?? null,
                UserId = currentUser.Id,
                Name = shisha.Name,
                //Description = shisha.Description,
                //Type = shisha.Type,
                //Tag = shisha.Tag,
                SmokedCount = 1,
                IsPublic = false,
                IsWish = false,
                IsFavourite = false,
                IsSeasonal = false,
                Power = this.GetShishaPower(shisha),
                Tabaccos = shisha.Tabaccos,
                //  TotalRating
                //  OwnRating
            };

            await shishasRepository.AddOne(newShisha);

            var newSmoked = currentUser.ShishasSmokedList.ToList();
            newSmoked.Add(newShisha.Id);
            await this.usersService.UpdateFields(currentUser.Id, new Dictionary<string, object>
            {
                { "ShishasSmokedList", newSmoked.ToArray() }
            });

            return newShisha;
        }

        private async Task<Shisha> CreateShishaFromIdentifier(string identifier)
        {
            var tabaccos = await this.tabaccoService.GetTabaccosByIdentifier(identifier);
            return new Shisha()
            {
                Identifier = identifier,
                PublicId = null,
                Name = this.GenerateNameFromTabaccos(tabaccos),
                //Description = shisha.Description,
                //Type = shisha.Type,
                //Tag = shisha.Tag,
                SmokedCount = 0,
                IsPublic = false,
                IsWish = false,
                IsFavourite = false,
                IsSeasonal = false,
                Power = 0,
                Tabaccos = tabaccos.ToArray()
                //  TotalRating
                //  OwnRating
            };
        }

        public string GenerateNameFromTabaccos(IEnumerable<Tabacco> tabaccos)
        {
            string result = "";
            foreach (var item in tabaccos)
            {
                result += item.Name + '-';
            }

            return result;
        }

        public async Task AddPublicShishaSmoker(string userId, string shishaId, bool isWish = false)
        {
            await this.shishaSmokersRepository.AddOne(new ShishaSmokers() {
                UserId = userId,
                ShishaId = shishaId,
                IsWish = isWish
            });
        }

        public async Task<Shisha> UpdateCount(Shisha shisha)
        {
            var update = Builders<Shisha>.Update.Set("Count", shisha.SmokedCount + 1);
            await this.shishasRepository.UpdateOne(shisha.Id, update);

            return shisha;
        }

        public async Task<Shisha> MoveWishedToSmoked(Shisha shisha)
        {
            var update = Builders<Shisha>.Update.Set("IsWish", false).Set("Count", 1);
            await this.shishasRepository.UpdateOne(shisha.Id, update);

            var currentUser = await this.usersService.GetCurrentUser();
            var myWished = currentUser.ShishasWishedList.ToList();
            if (myWished.Contains(shisha.Id))
            {
                myWished.Remove(shisha.Id);
                
                await this.usersService.UpdateFields(currentUser.Id, new Dictionary<string, object>
                {
                    { "ShishasWishedList", myWished.ToArray() }
                });
            }

            var mySmoked = currentUser.ShishasSmokedList.ToList();
            if (!mySmoked.Contains(shisha.Id))
            {
                mySmoked.Add(shisha.Id);

                await this.usersService.UpdateFields(currentUser.Id, new Dictionary<string, object>
                {
                    { "ShishasSmokedList", mySmoked.ToArray() }
                });
            }

            return shisha;
        }

        public string GetShishaIdentifier(Shisha shisha)
        {
            List<int> tabaccos = new List<int>();
            foreach (var tabacco in shisha.Tabaccos)
            {
                tabaccos.Add(tabacco.InternalId);
            }

            if(tabaccos.GroupBy(n => n).Any(c => c.Count() > 1))
            {
                throw new DuplicateItemsKeyException(shisha);
            }

            tabaccos.Sort();
            return string.Join(",", tabaccos);
        }

        public double GetShishaPower(Shisha shisha)
        {
            double power = 0;
            int count = shisha.Tabaccos.Length;
            foreach (var tabacco in shisha.Tabaccos)
            {
                power += tabacco.Power;
            }
            return power / count;
        }

        public async Task<bool> FindShishaByIdentifier(string[] shishas, string shishaIdentifier)
        {
            if (shishas == null || shishas.Length == 0)
            {
                return false;
            }

            var smokedShishas = await this.GetMany(shishas);
            if (smokedShishas.FirstOrDefault(s => s.Identifier == shishaIdentifier) != null)
            {
                return true;
            }

            return false;
        }
    }
}
