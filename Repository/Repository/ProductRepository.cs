using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Domain.Domains;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;
using Repository.Persistence;

namespace Repository.Repository
{
    public class ProductRepository : BaseRepositoryDapper<Product>, IProductRepository
    {

        protected IDbTransaction _transaction { get; private set; }
        //protected IDbConnection _connection { get { return Transaction.Connection; } }

        public ProductRepository(IDbTransaction transaction) : base(transaction)
        {
            _transaction = transaction;
        }
        
        public async Task<bool> CheckIfProductExistsByDescription(string description)
        {
            IEnumerable<Product> products = null;
            var query = $"select * from {typeof(Product).Name}s ";
        
            if (!string.IsNullOrWhiteSpace(description))
                query += "where Description = @Description";
            else
                return false;
            
            products = await Connection.QueryAsync<Product>(query, new {Description = description}, _transaction);

            return products.Any();
        }
    }
}