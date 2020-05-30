using System;
using System.Threading.Tasks;
using IdentityServerSQL.Repository;

namespace IdentityServerSQL.UnitOfWork
{
    public interface IUnitOfWork: IDisposable
    {
        IUserRepository UserRepository { get; }
        IRoleRepository RoleRepository { get; }
        IUserRoleRepository UserRoleRepository { get; }
        
        bool Commit();
    }
}