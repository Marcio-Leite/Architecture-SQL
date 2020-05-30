using System;
using System.Collections;
using System.Collections.Generic;
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

        public async Task<bool> CheckIfProductExistsByDescription(string description)
        {
            IEnumerable<Product> products = null;
            var query = $"select * from {typeof(Product).Name}s ";

            if (!string.IsNullOrWhiteSpace(description))
                query += String.Format("where Description = @description", description);
            else
                return false;
            
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                products = await connection.QueryAsync<Product>(query);
            }

            return products.Any();
        }
    }
}