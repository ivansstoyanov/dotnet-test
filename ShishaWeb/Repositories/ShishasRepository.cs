using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using ShishaWeb.MongoModels;
using ShishaWeb.Repositories.Interfaces;

namespace ShishaWeb.Repositories
{
  public class ShishasRepository : BaseRepository<Shisha>, IShishasRepository
  {
      public ShishasRepository(IOptions<Settings> settings, IHttpContextAccessor _httpContextAccessor) : base(settings, _httpContextAccessor)
      {

      }
  }
}
