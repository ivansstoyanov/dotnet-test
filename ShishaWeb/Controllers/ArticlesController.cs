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
    public class ArticlesController : BaseController
    {
        private readonly IArticlesService articlesService;

        public ArticlesController(IArticlesService articlesService)
        {
            this.articlesService = articlesService;
        }

        [HttpGet]
        public async Task<IEnumerable<ArticleDTO>> Get()
        {
            var result = await this.articlesService.GetAll();

            return result.ToDto();
        }

        [HttpGet("{id}")]
        public async Task<string> Get(int id)
        {
            var result = await this.articlesService.GetAll();

            return result.ToString();
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody]ArticleDTO article)
        {
            var result = Mapper.ToDto(await this.articlesService.Add(Mapper.ToEntity(article)));

            return Created($"{Request.Path}/{result.Id}", result);
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
