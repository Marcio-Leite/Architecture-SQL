using System.Threading.Tasks;
using Repository.Interfaces;
using Repository.Persistence;

namespace Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PersistenceDbContext _context;

        public UnitOfWork(PersistenceDbContext context)
        {
            _context = context;
        }
        
        public async Task<bool> Commit()
        {
            var changeAmount = await _context.SaveChangesAsync();

            return changeAmount > 0;
        }
        
        public bool CommitDapper()
        {
            var changeAmount = _context.SaveChanges();

            return changeAmount > 0;
        }
        
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}