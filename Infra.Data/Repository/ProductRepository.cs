using System;
using System.Linq;
using System.Threading.Tasks;
using Domain.Domains;
using Infra.Data.RepositoryInterfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using Repository.Interfaces;
using Repository.Repository;
using ServiceStack;

namespace Infra.Data.Repository
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(IMongoContext context) : base(context)
        {
        }

        public async Task<bool> CheckIfProductExistsByDescription(string description)
        {
            DbSet = _context.GetCollection<Product>(typeof(Product).Name);
            //DbSet.Find(new BsonDocument {{"Description", description}}).FirstAsync().Result;
            //var data = ; --> como alternativa
            var data = await DbSet.Find(x => x.Description == description).FirstOrDefaultAsync();
            
            //var data2 = await DbSet.FindAsync(Builders<Product>.Filter.Eq("Description", description)).FirstOrDefaultAsync();

            return data != null;
        }
    }
}