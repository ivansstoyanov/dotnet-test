using MongoDB.Driver;
using ShishaWeb.MongoModels;
using ShishaWeb.Repositories.Interfaces;
using ShishaWeb.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShishaWeb.Services
{
    public class TabaccoService : ITabaccoService
    {
        private readonly ITabaccoRepository tabaccoRepository = null;
        private readonly ITabaccoRelationRepository tabaccoRelationRepository = null;

        public TabaccoService(ITabaccoRepository tabaccoRepository, ITabaccoRelationRepository tabaccoRelationRepository)
        {
            this.tabaccoRepository = tabaccoRepository;
            this.tabaccoRelationRepository = tabaccoRelationRepository;
        }

        public async Task<IEnumerable<Tabacco>> GetAll()
        {
            var tabaccos = await tabaccoRepository.GetAll();

            return tabaccos.Entities;
        }

        public async Task<IEnumerable<Tabacco>> GetRelated(string key)
        {
            if(string.IsNullOrWhiteSpace(key))
            {
                return await this.GetAll();
            }

            var filter = Builders<TabaccoRelation>.Filter.Eq("Identifier", key);
            var tabaccoRelation = await tabaccoRelationRepository.GetOne(filter);

            var tabaccos = tabaccoRelation.Entity.RelatedTabaccos;

            return tabaccos;
        }

        public async Task<IEnumerable<Tabacco>> GetTabaccosByIdentifier(string identifier)
        {
            var filter = Builders<Tabacco>.Filter.In("Identifier", identifier.Split(','));
            var tabaccos = await this.tabaccoRepository.GetMany(filter);

            return tabaccos.Entities;
        }

        public async Task<Tabacco> Add(Tabacco tabacco)
        {
            var result = await tabaccoRepository.AddOne(tabacco);

            return tabacco;
        }
    }
}
