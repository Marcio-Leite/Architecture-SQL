using ProductApp.UseCases;
using Shared;

namespace ProductApp.UseCasesInterfaces.AddProduct
{
    public interface IAddProductResponseObject : IPostResponseObject
    {
        ProductResponse ProductResponse { get; }
    }
}