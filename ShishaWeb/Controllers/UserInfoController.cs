using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ShishaWeb.Services.Interfaces;
using ShishaWeb.Models;
using ShishaWeb.Services;
using System.Net.Http;

namespace ShishaWeb.Controllers
{
    [Authorize("Bearer")]
    [Route("api/[controller]")]
    public class UserInfoController : BaseController
    {
        private readonly IUsersService usersService;
        private readonly IAuditProvider auditProvider;

        public UserInfoController(IUsersService usersService, IAuditProvider auditProvider)
        {
            this.usersService = usersService;
            this.auditProvider = auditProvider;
        }

        [HttpGet]
        public async Task<IEnumerable<UserShortInfoDTO>> GetAll()
        {
            var currentUser =  await this.usersService.Get(this.auditProvider.CurrentUserId);

            var result = await this.usersService.GetAll();

            return result.ToDto(true, currentUser.Following);
        }

        [HttpGet("{id}")]
        public async Task<UserShortInfoDTO> Get(string id)
        {
            //TODO if current user is seld or current user is admin -> then return all data
            var currentUser = await this.usersService.Get(this.auditProvider.CurrentUserId);

            var result = await this.usersService.Get(id);

            return result.ToDto(true, currentUser.Following);
        }

        [HttpGet("[action]/{id?}")]
        public async Task<IEnumerable<UserShortInfoDTO>> GetUserFollowers(string id)
        {
            id = id ?? this.auditProvider.CurrentUserId;
            var user = await this.usersService.Get(id);

            var result = await this.usersService.GetUserFollowers(id);

            return result.ToDto(true, user.Following);
        }

        [HttpGet("[action]/{id?}")]
        public async Task<IEnumerable<UserShortInfoDTO>> GetUserFollowing(string id)
        {
            id = id ?? this.auditProvider.CurrentUserId;

            var result = await this.usersService.GetUserFollowing(id);

            return result.ToDto(true, new string[0]);
        }

        [HttpPost("[action]")]
        public async Task<IEnumerable<string>> AddFollowing([FromBody]string id)
        {
            var result = await this.usersService.AddFollower(this.auditProvider.CurrentUserId, id);

            return result;
        }

        [HttpPost("[action]")]
        public async Task<IEnumerable<string>> RemoveFollowing([FromBody] string value)
        {
            var result = await this.usersService.RemoveFollower(this.auditProvider.CurrentUserId, value);

            return result;
        }
    }
}
