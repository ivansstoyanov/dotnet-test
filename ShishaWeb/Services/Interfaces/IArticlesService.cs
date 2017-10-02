using ShishaWeb.MongoModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShishaWeb.Services.Interfaces
{
    public interface IArticlesService
    {
        Task<IEnumerable<Article>> GetAll();

        Task<Article> Add(Article article);
    }
}
