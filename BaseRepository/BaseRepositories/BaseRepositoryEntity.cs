// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Linq.Dynamic.Core;
// using System.Threading.Tasks;
// using Microsoft.EntityFrameworkCore;
// using Repository.Interfaces;
// using Repository.Persistence;
//
// namespace Repository.Repository
// {
//     public class BaseRepositoryEntity<TEntity>: IRepository<TEntity> where TEntity : class
//     {
//         protected readonly PersistenceDbContext Db;
//         protected readonly DbSet<TEntity> DbSet;
//
//         public BaseRepositoryEntity(PersistenceDbContext context)
//         {
//             Db = context;
//             DbSet = Db.Set<TEntity>();
//         }
//
//         public virtual void Add(TEntity obj)
//         {
//             DbSet.AddAsync(obj);
//         }
//         
//         public virtual async Task<IEnumerable<TEntity>> Query(string where = null, object parameters = null)
//         {
//             return await DbSet.Where(where, parameters).ToListAsync();
//         }
//         
//         public virtual async Task<TEntity> GetById(Guid id)
//         {
//             return await DbSet.Where(string.Format("id = @id", id)).FirstOrDefaultAsync();
//         }
//
//         public virtual IQueryable<TEntity> GetAll()
//         {
//             return DbSet;
//         }
//
//         public virtual void Update(TEntity obj)
//         {
//             DbSet.Update(obj);
//         }
//
//         public virtual void Delete(TEntity entity)
//         {
//             DbSet.Remove(entity);
//         }
//
//         public async Task<int>  SaveChanges()
//         {
//             return await Db.SaveChangesAsync();
//         }
//
//         public void Dispose()
//         {
//             Db.Dispose();
//             GC.SuppressFinalize(this);
//         }
//     }
// }
