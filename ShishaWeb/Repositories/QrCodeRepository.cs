using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using ShishaWeb.MongoModels;
using ShishaWeb.Repositories.Interfaces;

namespace ShishaWeb.Repositories
{
  public class QrCodeRepository : BaseRepository<QrCode>, IQrCodeRepository
  {
      public QrCodeRepository(IOptions<Settings> settings, IHttpContextAccessor _httpContextAccessor) : base(settings, _httpContextAccessor)
      {

      }
  }
}
