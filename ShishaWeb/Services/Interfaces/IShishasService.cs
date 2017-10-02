using ShishaWeb.MongoModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShishaWeb.Services.Interfaces
{
    public interface IShishasService
    {
        Task<Shisha> Get(string id);

        Task<IEnumerable<Shisha>> GetMany(string[] ids);

        Task<IEnumerable<Shisha>> GetPublicShishas();
            
        Task<Shisha> Add(Shisha shisha);

        Task<Shisha> AddToWishList(Shisha shisha);

        Task<Shisha> AddToSmokeList(Shisha shisha, string publicId, string newIdentifier);

        Task AddPublicShishaSmoker(string userId, string shishaId, bool isWish = false);

        Task<Shisha> UpdateCount(Shisha shisha);

        Task<Shisha> MoveWishedToSmoked(Shisha shisha);

        string GetShishaIdentifier(Shisha shisha);

        double GetShishaPower(Shisha shisha);

        Task<bool> FindShishaByIdentifier(string[] shishas, string shishaKey);
    }
}
