using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Repository.Interfaces;
namespace Repository.Repository
{
    public abstract class BaseRepositoryDapper<TEntity> : IRepository<TEntity> where TEntity : class
    {
        public static IConfiguration Configuration { get; set;  }
        
        public BaseRepositoryDapper() { }
        
        public BaseRepositoryDapper(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        protected string connectionString = Configuration.GetConnectionString("DefaultConnection");

        public virtual void Add(TEntity entity)
        {
            var columns = GetColumns();
            var stringOfColumns = string.Join(", ", columns);
            var stringOfParameters = string.Join(", ", columns.Select(e => "@" + e));
            var query = $"insert into {typeof(TEntity).Name}s ({stringOfColumns}) values ({stringOfParameters})";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                connection.ExecuteAsync(query, entity);
            }
        }
        

        public virtual void Delete(TEntity entity)
        {
            var query = $"delete from {typeof(TEntity).Name}s where Id = @Id";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                connection.ExecuteAsync(query, entity);
            }
        }

        public virtual void Update(TEntity entity)
        {
            var columns = GetColumns();
            var stringOfColumns = string.Join(", ", columns.Select(e => $"{e} = @{e}"));
            var query = $"update {typeof(TEntity).Name}s set {stringOfColumns} where Id = @Id";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                connection.ExecuteAsync(query, entity);
            }
        }

        public virtual async Task<IEnumerable<TEntity>> Query(string where = null)
        {
            var query = $"select * from {typeof(TEntity).Name}s ";

            if (!string.IsNullOrWhiteSpace(where))
                query += where;

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return await connection.QueryAsync<TEntity>(query);
            }
        }
        
        public virtual async Task<TEntity> GetById(string id)
        {
            var query = $"select * from {typeof(TEntity).Name}s where id = {id}";
            
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return await connection.QueryFirstAsync<TEntity>(query);
            }
        }

        private IEnumerable<string> GetColumns()
        {
            return typeof(TEntity)
                    .GetProperties()
                    .Where(e => e.Name != "Id" && !e.PropertyType.GetTypeInfo().IsGenericType)
                    .Select(e => e.Name);
        }

        public void Dispose()
        {
            Dispose();
        }
    }
}
