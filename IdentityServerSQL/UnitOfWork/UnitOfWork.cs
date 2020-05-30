using System;
using System.Data;
using System.Threading.Tasks;
using IdentityServerSQL.Repository;
using Microsoft.Data.SqlClient;
namespace IdentityServerSQL.UnitOfWork
{
    public class UnitOfWork: IUnitOfWork
    {
         //private readonly IdentityContext _context;
        // public SqlConnection connection;
        // private IDbTransaction _transaction;
        // private bool _disposed;
        
        private IDbConnection _connection;
        private IDbTransaction _transaction;
        
        private IUserRoleRepository _userRoleRepository;
        private IUserRepository _userRepository;
        private IRoleRepository _roleRepository;
        private bool _disposed;

        public UnitOfWork()
        {
            //_context = context;
            
            _connection = new SqlConnection( "Server=.\\SQLEXPRESS;Database=ArchitectureSQL;Trusted_Connection=True;User ID=marcioleite;Password=turambar;MultipleActiveResultSets=True"); 
            _connection.Open();
            _transaction = _connection.BeginTransaction();
        }

        public IUserRepository UserRepository
        {
            get { return _userRepository ?? (_userRepository = new UserRepository(_transaction)); }
        }
        
        public IRoleRepository RoleRepository
        {
            get { return _roleRepository ?? (_roleRepository = new RoleRepository(_transaction)); }
        }
        
        public IUserRoleRepository UserRoleRepository
        {
            get { return _userRoleRepository ??= new UserRoleRepository(_transaction); }
        }

        public bool Commit()
        {
            try
            {
                _transaction.Commit();
                return true;
            }
            catch (Exception e)
            {
                _transaction.Rollback();
                var error = e.Message;
                return false;
            }
             finally
             {
                 _transaction.Dispose();
                 _transaction = _connection.BeginTransaction();
                 ResetRepositories();
             }

           //return changeAmount > 0;
        }

        private void ResetRepositories()
        {
            _userRepository = null;
            _roleRepository = null;
            _userRoleRepository = null;
        }
        
        // public bool CommitDapper()
        // {
        //     var changeAmount = _context.SaveChanges();
        //
        //     return changeAmount > 0;
        // }
        
        public void Dispose()
        {
            dispose(true);
            GC.SuppressFinalize(this);
        }

        private void dispose(bool disposing)
        {
            if (!_disposed)
            {
                if(disposing)
                {
                    if (_transaction != null)
                    {
                        _transaction.Dispose();
                        _transaction = null;
                    }
                    if(_connection != null)
                    {
                        _connection.Dispose();
                        _connection = null;
                    }
                }
                _disposed = true;
            }
        }

        ~UnitOfWork()
        {
            dispose(false);
        }
    }
}