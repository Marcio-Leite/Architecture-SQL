using System.Collections.Generic;
using System.Data;
using IdentityServerSQL.Models;

namespace IdentityServerSQL.Repository
{
    public class UserRepository: BaseRepositoryDapper<ApplicationUser>, IUserRepository
    {
        public UserRepository(IDbTransaction transaction)
            : base(transaction)
        {
        }
    }
}