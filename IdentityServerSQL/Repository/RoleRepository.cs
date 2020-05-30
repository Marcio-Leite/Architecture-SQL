using System.Data;
using IdentityServerSQL.Models;

namespace IdentityServerSQL.Repository
{
    public class RoleRepository: BaseRepositoryDapper<ApplicationRole>, IRoleRepository
    {
        public RoleRepository(IDbTransaction transaction)
            : base(transaction)
        {
        }
    }
}