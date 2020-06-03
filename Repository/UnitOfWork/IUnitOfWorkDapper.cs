using System;

namespace Repository.Interfaces
{
    public interface IUnitOfWorkDapper : IDisposable
    {
        //Acrescentar todos os repositórios aqui
        IProductRepository ProductRepository { get; }
        bool Commit();
    }
}