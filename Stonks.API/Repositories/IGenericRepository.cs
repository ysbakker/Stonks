using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Stonks.API.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        public Task<IEnumerable<TEntity>> GetAll();

        // TODO: make args an array so we can use composite keys
        public Task<TEntity> GetById(object id);

        public Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> filter = null);
        
        public void Insert(TEntity entity);

        public void Update(TEntity entityToUpdate);
        
        public void Delete(object id);

        public void Save();
    }
}