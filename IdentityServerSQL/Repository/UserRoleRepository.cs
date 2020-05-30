using System.Data;
using IdentityServerSQL.Models;

namespace IdentityServerSQL.Repository
{
    public class UserRoleRepository : BaseRepositoryDapper<ApplicationUserRole>, IUserRoleRepository
    {
        public UserRoleRepository(IDbTransaction transaction)
            : base(transaction)
        {
        }
    }
}