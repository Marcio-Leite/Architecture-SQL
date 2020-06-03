using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Dapper;
using Repository.Interfaces;

namespace Repository.Repository
{
    public abstract class BaseRepositoryDapper<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected IDbTransaction Transaction { get; private set; }
        protected IDbConnection Connection { get { return Transaction.Connection; } }

        public BaseRepositoryDapper(IDbTransaction transaction)
        {
            Transaction = transaction;
        }

        public virtual void Add(TEntity entity)
        {
            var columns = GetColumns();
            var stringOfColumns = string.Join(", ", columns);
            var stringOfParameters = string.Join(", ", columns.Select(e => "@" + e));
            var query = $"insert into {typeof(TEntity).Name}s ({stringOfColumns}) values ({stringOfParameters})";

            Connection.Execute(query, entity, Transaction);
        }

        public virtual void Delete(TEntity entity)
        {
            var query = $"delete from {typeof(TEntity).Name}s where Id = @Id";
            Connection.Execute(query, entity, Transaction);
        }

        public virtual void Update(TEntity entity)
        {
            var columns = GetColumns();
            var stringOfColumns = string.Join(", ", columns.Select(e => $"{e} = @{e}"));
            var query = $"update {typeof(TEntity).Name}s set {stringOfColumns} where Id = @Id";

            Connection.Execute(query, entity, Transaction);
        }

        public virtual async Task<IEnumerable<TEntity>> Query(string where = null, object parameters = null)
        {
            var query = $"select * from {typeof(TEntity).Name}s ";

            if (!string.IsNullOrWhiteSpace(where))
                query += " where " + where;
            
            return await Connection.QueryAsync<TEntity>(
                query, 
                parameters, 
                Transaction);
        }
        
        public virtual async Task<TEntity> GetById(Guid id)
        {
            var query = $"select * from {typeof(TEntity).Name}s where {typeof(TEntity).Name}Id = @id";
            
            return await Connection.QueryFirstAsync<TEntity>(query, new {id = id}, Transaction);
        }

        private IEnumerable<string> GetColumns()
        {
            return typeof(TEntity)
                    .GetProperties()
                    //.Where(e => e.Name != "Id" && !e.PropertyType.GetTypeInfo().IsGenericType)
                    .Where(e => e.Name != "Id" && e.PropertyType.GetTypeInfo().Namespace == "System" )
                    .Select(e => e.Name);  
        }

        public void Dispose()
        {
            Dispose();
        }
    }
}
