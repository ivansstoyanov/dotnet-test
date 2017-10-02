using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using ShishaWeb.MongoModels;
using ShishaWeb.Repositories.Interfaces;

namespace ShishaWeb.Repositories
{
  public class ActivityRepository : BaseRepository<Activity>, IActivityRepository
  {
      public ActivityRepository(IOptions<Settings> settings, IHttpContextAccessor _httpContextAccessor) : base(settings, _httpContextAccessor)
      {

      }
  }
}
