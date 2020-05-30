using System.Threading.Tasks;
using Domain.Domains;

namespace Repository.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<bool> CheckIfProductExistsByDescription(string description);
    }
}