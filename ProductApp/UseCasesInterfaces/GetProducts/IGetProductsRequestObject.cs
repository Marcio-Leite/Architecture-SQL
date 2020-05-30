using Domain.Domains;
using MongoDB.Driver;
using Shared;

namespace ProductApp.UseCasesInterfaces.GetProducts
{
    public interface IGetProductsRequestObject : IValidator
    {
        public string Field { get; }
        public string Search { get; } 
    }
}