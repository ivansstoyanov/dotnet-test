using ShishaWeb.MongoModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShishaWeb.Services.Interfaces
{
    public interface ITabaccoService
    {
        Task<IEnumerable<Tabacco>> GetAll();

        Task<IEnumerable<Tabacco>> GetRelated(string key);

        Task<IEnumerable<Tabacco>> GetTabaccosByIdentifier(string identifier);

        Task<Tabacco> Add(Tabacco tabacco);
    }
}
