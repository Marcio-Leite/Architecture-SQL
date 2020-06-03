using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Repository.Interfaces;
using Repository.Map;

namespace Repository.UnitOfWork
    {
    public class UnitOfWorkDapper: UnitOfWorkDapperMapper, IUnitOfWorkDapper
    {
        private IDbConnection _connection;
        private bool _disposed;

        public UnitOfWorkDapper()
        {
            _connection = new SqlConnection( "Server=.\\SQLEXPRESS;Database=ArchitectureSQL;Trusted_Connection=True;User ID=marcioleite;Password=turambar;MultipleActiveResultSets=True"); 
            _connection.Open();
            _transaction = _connection.BeginTransaction();
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
        }

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

        ~UnitOfWorkDapper()
        {
            dispose(false);
        }
    }
}