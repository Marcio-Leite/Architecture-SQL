using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Domains;
using Repository.Interfaces;

namespace Infra.Data.RepositoryInterfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<bool> CheckIfProductExistsByDescription(string description);
    }
}