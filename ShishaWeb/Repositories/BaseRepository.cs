using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using ShishaWeb.MongoModels;
using ShishaWeb.Repositories.Helper;
using ShishaWeb.Repositories.Interfaces;
using ShishaWeb.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace ShishaWeb.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity: class, new() //where TEntity : BaseEntity
  {
        private readonly MongoContext _context = null;
        private readonly IAuditProvider auditProvider = null;

        protected MongoContext Context
        {
            get
            {
                return this._context;
            }
        }

        public BaseRepository(IOptions<Settings> settings, IHttpContextAccessor _httpContextAccessor)
        {
            this._context = new MongoContext(settings);
            this.auditProvider = (IAuditProvider)_httpContextAccessor.HttpContext.RequestServices.GetService(typeof(IAuditProvider));
        }

        #region Get
        /// <summary>
        /// A generic GetOne method
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<GetOneResult<TEntity>> GetOne(string id)
        {
            var filter = Builders<TEntity>.Filter.Eq("Id", id);

            return await GetOne(filter);
        }

        /// <summary>
        /// A generic GetOne method
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<GetOneResult<TEntity>> GetOne(FilterDefinition<TEntity> filter)
        {
            var res = new GetOneResult<TEntity>();
            try
            {
                var collection = GetCollection();
                var entity = await collection.Find(filter).SingleOrDefaultAsync();

                if (entity != null)
                {
                    res.Entity = entity;
                }

                res.Success = true;
                return res;
            }
            catch (Exception ex)
            {
                res.Message = HelperService.NotifyException("GetOne", "Exception getting one " + typeof(TEntity).Name, ex);
                return res;
            }
        }

        /// <summary>
        /// A generic get many method
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<GetManyResult<TEntity>> GetMany(IEnumerable<string> ids)
        {
            try
            {
                var collection = GetCollection();
                var filter = Builders<TEntity>.Filter.In("Id", ids);

                return await GetMany(filter);
            }
            catch (Exception ex)
            {
                var res = new GetManyResult<TEntity>();
                res.Message = HelperService.NotifyException("GetMany", "Exception getting many " + typeof(TEntity).Name + "s", ex);

                return res;
            }
        }

        /// <summary>
        /// A generic get many method with filter
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<GetManyResult<TEntity>> GetMany(FilterDefinition<TEntity> filter)
        {
            var res = new GetManyResult<TEntity>();

            try
            {
                var collection = GetCollection();
                var entities = await collection.Find(filter).ToListAsync();

                if (entities != null)
                {
                    res.Entities = entities;
                }
                res.Success = true;

                return res;
            }
            catch (Exception ex)
            {
                res.Message = HelperService.NotifyException("GetMany", "Exception getting many " + typeof(TEntity).Name + "s", ex);
                return res;
            }
        }

        /// <summary>
        /// FindCursor
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="filter"></param>
        /// <returns>A cursor for the query</returns>
        public IFindFluent<TEntity, TEntity> FindCursor(FilterDefinition<TEntity> filter)
        {
            var res = new GetManyResult<TEntity>();
            var collection = GetCollection();
            var cursor = collection.Find(filter);

            return cursor;
        }

        /// <summary>
        /// A generic get all method
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public async Task<GetManyResult<TEntity>> GetAll()
        {
            var res = new GetManyResult<TEntity>();
            try
            {
                var collection = GetCollection();
                var entities = await collection.Find(new BsonDocument()).ToListAsync();

                if (entities != null)
                {
                  res.Entities = entities;
                }
                res.Success = true;

                return res;
            }
            catch (Exception ex)
            {
                res.Message = HelperService.NotifyException("GetAll", "Exception getting all " + typeof(TEntity).Name + "s", ex);
                return res;
            }
        }

        /// <summary>
        /// A generic Exists method
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<bool> Exists(string id)
        {
            var collection = GetCollection();
            var query = new BsonDocument("Id", id);
            var cursor = collection.Find(query);
            var count = await cursor.CountAsync();

            return (count > 0);
        }

        /// <summary>
        /// A generic count method
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<long> Count(string id)
        {
            var filter = new FilterDefinitionBuilder<TEntity>().Eq("Id", id);

            return await Count(filter);
        }

        /// <summary>
        /// A generic count method
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<long> Count(FilterDefinition<TEntity> filter)
        {
            var collection = GetCollection();
            var cursor = collection.Find(filter);
            var count = await cursor.CountAsync();

            return count;
        }
        #endregion Get

        #region Create
        /// <summary>
        /// A generic Add One method
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<Result> AddOne(TEntity item)
        {
            var res = new Result();
            try
            {
                this.SetCreatedTimeStamp(item);

                var collection = GetCollection();
                await collection.InsertOneAsync(item);
                res.Success = true;
                res.Message = "OK";

                return res;
            }
            catch (Exception ex)
            {
                res.Message = HelperService.NotifyException("AddOne", "Exception adding one " + typeof(TEntity).Name, ex);
                return res;
            }
        }
        #endregion Create

        #region Delete
        /// <summary>
        /// A generic delete one method
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Result> DeleteOne(string id)
        {
            var filter = new FilterDefinitionBuilder<TEntity>().Eq("Id", id);

            return await DeleteOne(filter);
        }

        /// <summary>
        /// A generic delete one method
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Result> DeleteOne(FilterDefinition<TEntity> filter)
        {
            var result = new Result();
            try
            {
                var collection = GetCollection();
                var deleteRes = await collection.DeleteOneAsync(filter);
                result.Success = true;
                result.Message = "OK";

                return result;
            }
            catch (Exception ex)
            {
                result.Message = HelperService.NotifyException("DeleteOne", "Exception deleting one " + typeof(TEntity).Name, ex);
                return result;
            }
        }

        /// <summary>
        /// A generic delete many method
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<Result> DeleteMany(IEnumerable<string> ids)
        {
            var filter = new FilterDefinitionBuilder<TEntity>().In("Id", ids);

            return await DeleteMany(filter);
        }

        /// <summary>
        /// A generic delete many method
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<Result> DeleteMany(FilterDefinition<TEntity> filter)
        {
            var result = new Result();
            try
            {
                var collection = GetCollection();
                var deleteRes = await collection.DeleteManyAsync(filter);

                if (deleteRes.DeletedCount < 1)
                {
                  var ex = new Exception();
                  result.Message = HelperService.NotifyException("DeleteMany", "Some " + typeof(TEntity).Name + "s could not be deleted.", ex);
                  return result;
                }
                result.Success = true;
                result.Message = "OK";

                return result;
            }
            catch (Exception ex)
            {
                result.Message = HelperService.NotifyException("DeleteMany", "Some " + typeof(TEntity).Name + "s could not be deleted.", ex);
                return result;
            }
        }
        #endregion Delete

        #region Update
        /// <summary>
        /// UpdateOne by id
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="id"></param>
        /// <param name="update"></param>
        /// <returns></returns>
        public async Task<Result> UpdateOne(string id, UpdateDefinition<TEntity> update)
        {
            var filter = new FilterDefinitionBuilder<TEntity>().Eq("Id", id);

            return await UpdateOne(filter, update);
        }

        /// <summary>
        /// UpdateOne with filter
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="filter"></param>
        /// <param name="update"></param>
        /// <returns></returns>
        public async Task<Result> UpdateOne(FilterDefinition<TEntity> filter, UpdateDefinition<TEntity> update)
        {
            var result = new Result();
            try
            {
                update = this.SetUpdatedTimeStamp(update);

                var collection = GetCollection();
                var updateRes = await collection.UpdateOneAsync(filter, update);

                if (updateRes.ModifiedCount < 1)
                {
                  var ex = new Exception();
                  result.Message = HelperService.NotifyException("UpdateOne", "ERROR: updateRes.ModifiedCount < 1 for entity: " + typeof(TEntity).Name, ex);
                  return result;
                }
                result.Success = true;
                result.Message = "OK";

                return result;
            }
            catch (Exception ex)
            {
                result.Message = HelperService.NotifyException("UpdateOne", "Exception updating entity: " + typeof(TEntity).Name, ex);
                return result;
            }
        }

        /// <summary>
        /// UpdateMany with Ids
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="id"></param>
        /// <param name="update"></param>
        /// <returns></returns>
        public async Task<Result> UpdateMany(IEnumerable<string> ids, UpdateDefinition<TEntity> update)
        {
            var filter = new FilterDefinitionBuilder<TEntity>().In("Id", ids);

            return await UpdateOne(filter, update);
        }

        /// <summary>
        /// UpdateMany with filter
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="filter"></param>
        /// <param name="update"></param>
        /// <returns></returns>
        public async Task<Result> UpdateMany(FilterDefinition<TEntity> filter, UpdateDefinition<TEntity> update)
        {
            var result = new Result();
            try
            {
                throw new Exception("TimeStamps are not implemented, please implement them first");

                var collection = GetCollection();
                var updateRes = await collection.UpdateManyAsync(filter, update);

                if (updateRes.ModifiedCount < 1)
                {
                  var ex = new Exception();
                  result.Message = HelperService.NotifyException("UpdateMany", "ERROR: updateRes.ModifiedCount < 1 for entities: " + typeof(TEntity).Name + "s", ex);
                  return result;
                }
                result.Success = true;
                result.Message = "OK";

                return result;
            }
            catch (Exception ex)
            {
                result.Message = HelperService.NotifyException("UpdateMany", "Exception updating entities: " + typeof(TEntity).Name + "s", ex);
                return result;
            }
        }
        #endregion Update

        #region Find And Update

        /// <summary>
        /// GetAndUpdateOne with filter
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="filter"></param>
        /// <param name="update"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task<GetOneResult<TEntity>> GetAndUpdateOne(FilterDefinition<TEntity> filter, UpdateDefinition<TEntity> update, FindOneAndUpdateOptions<TEntity, TEntity> options)
        {
            var result = new GetOneResult<TEntity>();
            try
            {
                throw new Exception("TimeStamps are not implemented, please implement them first");

                var collection = GetCollection();
                result.Entity = await collection.FindOneAndUpdateAsync(filter, update, options);
                result.Success = true;
                result.Message = "OK";

                return result;
            }
            catch (Exception ex)
            {
                result.Message = HelperService.NotifyException("GetAndUpdateOne", "Exception getting and updating entity: " + typeof(TEntity).Name, ex);
                return result;
            }
        }

        #endregion Find And Update

        /// <summary>
        /// The private GetCollection method
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        private IMongoCollection<TEntity> GetCollection()
        {
            return _context.GetCollection<TEntity>();
        }

        private void SetCreatedTimeStamp(TEntity item)
        {
            if (item is BaseAuditedEntity)
            {
                (item as BaseAuditedEntity).CreatedAt = DateTime.UtcNow;
                (item as BaseAuditedEntity).LastModifiedAt = DateTime.UtcNow;

                if (this.auditProvider != null && this.auditProvider.CurrentUserEmail != null)
                {
                    (item as BaseAuditedEntity).CreatedBy = this.auditProvider.CurrentUserEmail;
                    (item as BaseAuditedEntity).LastModifiedBy = this.auditProvider.CurrentUserEmail;
                }
            }
        }

        private UpdateDefinition<TEntity> SetUpdatedTimeStamp(UpdateDefinition<TEntity> update)
        {
            var updates = new List<UpdateDefinition<TEntity>>() { update };

            if ((update.GetType().GenericTypeArguments[0]).GetTypeInfo().IsSubclassOf(typeof(BaseAuditedEntity)))
            {
                updates.Add(Builders<TEntity>.Update.Set("LastModifiedAt", DateTime.UtcNow));

                if (this.auditProvider != null && this.auditProvider.CurrentUserEmail != null)
                {
                    updates.Add(Builders<TEntity>.Update.Set("LastModifiedBy", this.auditProvider.CurrentUserEmail));
                }
            }

            return Builders<TEntity>.Update.Combine(updates);
        }

    }
}
