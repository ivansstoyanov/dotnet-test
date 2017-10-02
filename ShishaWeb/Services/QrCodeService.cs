using MongoDB.Driver;
using ShishaWeb.Config.Enumerations;
using ShishaWeb.Exceptions;
using ShishaWeb.Exceptions.QrCodeExceptions;
using ShishaWeb.MongoModels;
using ShishaWeb.Repositories.Interfaces;
using ShishaWeb.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShishaWeb.Services
{
    public class QrCodeService : IQrCodeService
    {
        private readonly string appName = "shishApp";
        private readonly int expirationMinutes = 20;
        private readonly int maxQrCodeRegisterAllowed = 5;

        private readonly IQrCodeRepository qrCodeRepository = null;
        private readonly IUsersService usersService = null;
        private readonly IShishasService shishasService = null;
        private readonly IActivityService activityService = null;
        private readonly IAuditProvider auditProvider = null;

        public QrCodeService(IQrCodeRepository qrCodeRepository, IUsersService usersService, IShishasService shishasService, IActivityService activityService, IAuditProvider auditProvider)
        {
            this.qrCodeRepository = qrCodeRepository;
            this.usersService = usersService;
            this.shishasService = shishasService;
            this.activityService = activityService;
            this.auditProvider = auditProvider;
        }

        public async Task<IEnumerable<QrCode>> GetAll()
        {
            var qrCodes = await this.qrCodeRepository.GetAll();

            return qrCodes.Entities;
        }

        public async Task<QrCode> Add(string qrValue)
        {
            string qr = $"{appName}://{qrValue}";
            QrCode qrc = new QrCode()
            {
                Value = qr,
                Count = 0
            };

            var result = await qrCodeRepository.AddOne(qrc);

            return qrc;
        }

        public async Task Validate(string qrCodeId, string qrCodeValue)
        {
            var qrCode = await qrCodeRepository.GetOne(qrCodeId);
            if (qrCode.Entity == null)
            {
                throw new InvalidIdException(qrCode);
            }

            if (qrCode.Entity.Value != $"{appName}://{qrCodeValue}")
            {
                throw new InvalidIdException(qrCode, MessageCodes.INVALID_CODE);
            }

            if (qrCode.Entity.Count > this.maxQrCodeRegisterAllowed)
            {
                throw new MaxRegisterReachedException(qrCode);
            }

            if (DateTime.UtcNow > qrCode.Entity.CreatedAt.Add(new TimeSpan(0, this.expirationMinutes, 0)))
            {
                throw new QrCodeExpiredException(qrCode);
            }

            await this.UpdateQrCodeCount(qrCode.Entity);

            var split = qrCode.Entity.Value.Split("//");
            var registeredShisha = await this.RegisterShisha(split[0]);
#pragma warning disable 4014
            this.activityService.Add(new Activity()
            {
                UserId = this.auditProvider.CurrentUserId,
                ItemId = registeredShisha.Id,
                Type = ActivityTypeEnum.UserSmokedShisha
            });
#pragma warning restore 4014
        }

        private async Task UpdateQrCodeCount(QrCode qrCode)
        {
            var update = Builders<QrCode>.Update.Set("Count", qrCode.Count + 1);
            await this.qrCodeRepository.UpdateOne(qrCode.Id, update);
        }

        private async Task<Shisha> RegisterShisha(string identifier)
        {
            var personalFound = false;
            var publicFound = false;

            var user = await this.usersService.GetCurrentUser();
            var smoked = await this.shishasService.GetMany(user.ShishasSmokedList);
            var wished = await this.shishasService.GetMany(user.ShishasWishedList);
            var publicShishas = await this.shishasService.GetPublicShishas();

            var userShisha = smoked.FirstOrDefault(s => s.Identifier == identifier);
            if (userShisha != null)
            {
                personalFound = true;
                await this.shishasService.UpdateCount(userShisha);
            }
            else
            {
                userShisha = wished.FirstOrDefault(s => s.Identifier == identifier);
                if (userShisha != null)
                {
                    personalFound = true;
                    await this.shishasService.MoveWishedToSmoked(userShisha);
                }
            }
            
            var publicShisha = publicShishas?.FirstOrDefault(s => s.Identifier == identifier);
            if (publicShisha != null)
            {
                publicFound = true;
#pragma warning disable 4014
                this.shishasService.UpdateCount(publicShisha);
                this.shishasService.AddPublicShishaSmoker(user.Id, publicShisha.Id);
#pragma warning restore 4014
            }

            if (publicFound && !personalFound)
            {
                userShisha = await this.shishasService.AddToSmokeList(publicShisha, publicShisha.Id, null);
            }
            else if (!publicFound && !personalFound)
            {
                userShisha = await this.shishasService.AddToSmokeList(null, null, identifier);
            }

            await this.usersService.UpdateFields(user.Id, new Dictionary<string, object>
            {
                { "ShishasSmoked", user.ShishasSmoked + 1 }
            });

            return userShisha;
        }
    }
}
