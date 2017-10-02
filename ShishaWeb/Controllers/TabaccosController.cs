using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ShishaWeb.Services.Interfaces;
using ShishaWeb.Models;
using ShishaWeb.Services;

namespace ShishaWeb.Controllers
{
    [Authorize("Bearer")]
    [Route("api/[controller]")]
    public class TabaccosController : BaseController
    {
        private readonly ITabaccoService tabaccosService;

        public TabaccosController(ITabaccoService tabaccosService)
        {
            this.tabaccosService = tabaccosService;
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<TabaccoDTO>> GetAll()
        {
            var result = await this.tabaccosService.GetAll();

            return result.ToDto();
            //TODO order by most used
        }

        [HttpGet("[action]/{key?}")]
        public async Task<IEnumerable<TabaccoDTO>> GetRelated(string key)
        {
            var result = await this.tabaccosService.GetRelated(key);

            return result.ToDto();
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody]TabaccoDTO tabacco)
        {
            var result = Mapper.ToDto(await this.tabaccosService.Add(Mapper.ToEntity(tabacco)));

            return Created($"{Request.Path}/{result.Id}", result);
        }
    }
}
