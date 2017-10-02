using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShishaWeb.Auth;
using ShishaWeb.Models;
using System.Security.Claims;
using System.Security.Principal;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using ShishaWeb.Services.Interfaces;
using ShishaWeb.MongoModels;
using ShishaWeb.Services;

namespace ShishaWeb.Controllers
{
    [Route("api/[controller]")]
    public class TokenAuthController : BaseController
    {
        private readonly IUsersService usersService;

        public TokenAuthController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        [HttpPost]
        public async Task<RequestResult> GetAuthToken([FromBody]FirebaseClientData user)
        {
            FirebaseContactInfo contactInfo = await FirebaseAuth.GetAccountInfo(user.accessToken);
            if (contactInfo != null)
            {
                var currentFirebaseUser = contactInfo.users.FirstOrDefault(u => u.email == user.email);
                var ownUser = await this.usersService.GetByEmail(user.email);
                if (ownUser != null)
                {
                    //TODO refresh(store new) firebase access token?
                    return AuthenticateUser(ownUser);
                }
                else
                {
                    var newUser = await this.usersService.Add(new User()
                    {
                        Name = currentFirebaseUser.displayName != null ? currentFirebaseUser.displayName : currentFirebaseUser.email,
                        Email = currentFirebaseUser.email,
                        PhotoUrl = currentFirebaseUser.photoUrl,
                        Username = currentFirebaseUser.displayName != null ? currentFirebaseUser.displayName : currentFirebaseUser.email, //TODO ensure username is unique
                        FirebaseToken = user.accessToken,
                        FirebaseInfo = currentFirebaseUser
                    });

                    return AuthenticateUser(newUser);
                }
            }

            return new RequestResult
            {
                State = RequestState.Failed,
                Message = "Invalid User"
            };
        }

        private RequestResult AuthenticateUser(User user)
        {
            var requestAt = DateTime.Now;
            var expiresIn = requestAt + TokenAuthOption.ExpiresSpan;
            var token = GenerateToken(user, expiresIn);

            return new RequestResult
            {
                State = RequestState.Success,
                Data = new
                {
                    requertAt = requestAt,
                    expiresIn = TokenAuthOption.ExpiresSpan.TotalSeconds,
                    tokeyType = TokenAuthOption.TokenType,
                    accessToken = token
                }
            };
        }

        private string GenerateToken(User user, DateTime expires)
        {
            var handler = new JwtSecurityTokenHandler();

            ClaimsIdentity identity = new ClaimsIdentity(
                new GenericIdentity(user.Email, "TokenAuth"),
                new[] {
                    new Claim("Id", user.Id.ToString()),
                    new Claim("Name", user.Name.ToString()),
                    new Claim("Username", user.Username.ToString()),
                    new Claim("Email", user.Email.ToString())
                }
            );

            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = TokenAuthOption.Issuer,
                Audience = TokenAuthOption.Audience,
                SigningCredentials = TokenAuthOption.SigningCredentials,
                Subject = identity,
                Expires = expires
            });

            return handler.WriteToken(securityToken);
        }

        [HttpGet]
        [Authorize("Bearer")]
        public async Task<UserDTO> GetCurrentUserInfo()
        {
            var claimsMail = this.GetCurrentUserClaim("Email");

            return Mapper.ToDto(await this.usersService.GetByEmail(claimsMail));
        }

        [HttpPut("[action]")]
        [Authorize("Bearer")]
        public async Task<UserSettingsDTO> UpdateUserSettings([FromBody] UserSettingsDTO userSettings)
        {
            var currentUserId = this.GetCurrentUserClaim("Id");

            var result = await this.usersService.UpdateUserSettings(currentUserId, userSettings.ToEntity());

            return result.ToDto();
        }
    }
}
