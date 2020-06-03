using System.Data;
using Repository.Interfaces;
using Repository.Repository;

namespace Repository.Map
{
    //incluir aqui todo novo repositório criado! é importante para que todos utilizem a mesma transação.
    
    public abstract class UnitOfWorkDapperMapper
    {
        private IProductRepository _productRepository;
        internal IDbTransaction _transaction;
        
        //incluir repositório para resetar após o commit
        internal void ResetRepositories()
        {
            _productRepository = null;
        }
        
        //incluir cada repositório para instanciar e construir
        public IProductRepository ProductRepository
        {
            get { return _productRepository ?? (_productRepository = new ProductRepository(_transaction)); }
        }
    }
}