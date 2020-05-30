using Domain.Domains;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Repository.Map;

namespace Repository.Persistence
{
    public class PersistenceDbContext: DbContext
    {
        private readonly IHostingEnvironment _env;
        
        public PersistenceDbContext(DbContextOptions<PersistenceDbContext> options) : base(options) { }

        public PersistenceDbContext(IHostingEnvironment env)
        {
            _env = env;
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // get the configuration from the app settings
            var config = new ConfigurationBuilder()
                .SetBasePath(_env.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .Build();
            
            // define the database to use
            optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
        }
        
        
        //public PersistenceDbContext(DbContextOptions<PersistenceDbContext> options) : base(options) { }
        
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProductMap());
                        
            base.OnModelCreating(modelBuilder);
        }
    }
}
