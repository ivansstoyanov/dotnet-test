using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ShishaWeb.Services.Interfaces;
using ShishaWeb.Models;
using ShishaWeb.Services;
using ShishaWeb.Filters;

namespace ShishaWeb.Controllers
{
    [Authorize("Bearer")]
    [Route("api/[controller]")]
    [ItemManagementExceptionFilter]
    public class ShishasController : BaseController
    {
        private readonly IShishasService shishasService;
        private readonly IUsersService usersService;
        private readonly IAuditProvider auditProvider;

        public ShishasController(IShishasService shishasService, IUsersService usersService, IAuditProvider auditProvider)
        {
            this.shishasService = shishasService;
            this.usersService = usersService;
            this.auditProvider = auditProvider;
        }

        [HttpGet]
        public async Task<ShishaDTO> Get(string id)
        {
            var shisha =  await this.shishasService.Get(id);

            return shisha.ToDto();
        }

        [HttpGet("[action]/{id?}")]
        public async Task<IEnumerable<ShishaDTO>> GetWished(string id)
        {
            id = id ?? this.auditProvider.CurrentUserId;
            var user = await this.usersService.Get(id);

            var shishas = await this.shishasService.GetMany(user.ShishasWishedList);

            return shishas.ToDto();
        }

        [HttpGet("[action]/{id?}")]
        public async Task<IEnumerable<ShishaDTO>> GetSmoked(string id)
        {
            id = id ?? this.auditProvider.CurrentUserId;
            var user = await this.usersService.Get(id);

            var shishas = await this.shishasService.GetMany(user.ShishasSmokedList);

            return shishas.ToDto();
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody]ShishaDTO shisha)
        {
            var currentUser = await this.usersService.GetCurrentUser();
            var result = new MongoModels.Shisha();

            if (this.IsCurrentUserAdmin(currentUser) && shisha.IsPublic)
            {
                result = await this.shishasService.Add(Mapper.ToEntity(shisha));
            }
            else
            {
                result = await this.shishasService.AddToWishList(Mapper.ToEntity(shisha));
            }

            return Created($"{Request.Path}/{result.Id}", result.ToDto());
        }
    }
}
