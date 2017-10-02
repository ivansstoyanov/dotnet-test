using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ShishaWeb.MongoModels;
using ShishaWeb.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShishaWeb.Repositories
{
  public class ArticlesRepository : BaseRepository<Article>, IArticlesRepository
  {
      public ArticlesRepository(IOptions<Settings> settings, IHttpContextAccessor _httpContextAccessor) : base(settings, _httpContextAccessor)
      {

      }
  }
}
