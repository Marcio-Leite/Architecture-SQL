using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityServerSQL.Repository
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        void Add(TEntity obj);
        Task<IEnumerable<TEntity>> Query(string query, object parameters);
        
        Task<TEntity> GetById(string id);
        void Update(TEntity obj);
        void Delete(TEntity obj);
    }
}