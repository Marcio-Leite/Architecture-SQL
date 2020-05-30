using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Dapper;
using IdentityServerSQL.UnitOfWork;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;

namespace IdentityServerSQL.Repository
{
    public abstract class BaseRepositoryDapper<TEntity> : IRepository<TEntity> where TEntity : class
    {
        
        protected IDbTransaction Transaction { get; private set; }
        protected IDbConnection Connection { get { return Transaction.Connection; } }

        public BaseRepositoryDapper(IDbTransaction transaction)
        {
            Transaction = transaction;
        }
        
        
        public static IConfiguration Configuration { get; set;  }
        //protected string connectionString = Configuration.GetConnectionString("DefaultConnection");
        //private IUnitOfWork _uow;
        // public BaseRepositoryDapper()
        // {
        //     var builder = new ConfigurationBuilder()
        //         .AddJsonFile("appsettings.json", true, true);
        //
        //     builder.AddEnvironmentVariables();
        //     Configuration = builder.Build();
        //     connectionString = Configuration.GetConnectionString("DefaultConnection");
        //     SqlConnection connection = new SqlConnection(connectionString);        //teste
        // }
        
        // public BaseRepositoryDapper(IUnitOfWork uow)
        // {
        //     //Configuration = configuration;
        //     //SqlConnection connection = new SqlConnection(connectionString);        //teste\
        //     _uow = uow;
        // }
        
        

        public virtual void Add(TEntity entity)
        {
            var columns = GetColumns();
            var stringOfColumns = string.Join(", ", columns);
            var stringOfParameters = string.Join(", ", columns.Select(e => "@" + e));
            var query = $"insert into {typeof(TEntity).Name}s ({stringOfColumns}) values ({stringOfParameters})";
            
            // using (var connection = new SqlConnection(connectionString))
            // {
            //     connection.Open();
            Connection.Execute(query, entity, Transaction);
            //}
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

            // using (var connection = new SqlConnection(connectionString))
            // {
            //     connection.Open();
            Connection.Execute(query, entity, Transaction);
            //}
        }

        public virtual async Task<IEnumerable<TEntity>> Query(string where = null, object parameters = null)
        {
            var query = $"select * from {typeof(TEntity).Name}s ";

            if (!string.IsNullOrWhiteSpace(where))
                query += " where " + where;

            // using (var connection = new SqlConnection(connectionString))
            // {
            //     connection.Open();
                return await Connection.QueryAsync<TEntity>(
                    query, 
                    parameters, 
                    Transaction);
            //}
        }
        
        public virtual async Task<TEntity> GetById(string id)
        {
            var query = $"select * from {typeof(TEntity).Name}s where {typeof(TEntity).Name}Id = @id";
            
            // using (var connection = new SqlConnection(connectionString))
            // {
            //     connection.Open();
                return await Connection.QueryFirstAsync<TEntity>(query, new {id = id}, Transaction);
            //}
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
