using ShishaWeb.MongoModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShishaWeb.Services.Interfaces
{
    public interface IQrCodeService
    {
        Task<IEnumerable<QrCode>> GetAll();

        Task<QrCode> Add(string qrValue);

        Task Validate(string qrCodeId, string qrCodeValue);
    }
}
