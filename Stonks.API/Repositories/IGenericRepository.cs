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

        public Task<TEntity> GetById(object id);

        public void Insert(TEntity entity);

        public void Update(TEntity entityToUpdate);
        
        public void Delete(object id);

        public void Save();
    }
}