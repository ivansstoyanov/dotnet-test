using ShishaWeb.MongoModels;
using ShishaWeb.Repositories;
using ShishaWeb.Repositories.Interfaces;
using ShishaWeb.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShishaWeb.Services
{
    public class ArticlesService : IArticlesService
    {
        private readonly IArticlesRepository articleRepository = null;

        public ArticlesService(IArticlesRepository articleRepository)
        {
            this.articleRepository = articleRepository;
        }

        public async Task<Article> Get(string id)
        {
            var notes = await articleRepository.GetOne(id);

            return new Article();
        }

        public async Task<IEnumerable<Article>> GetAll()
        {
            var articles = await articleRepository.GetAll();

            return articles.Entities;
        }

        public async Task<Article> Add(Article article)
        {
            var result = await articleRepository.AddOne(article);

            return article;
        }
    }
}
