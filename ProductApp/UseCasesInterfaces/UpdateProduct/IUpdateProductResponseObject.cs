using ProductApp.UseCases;
using Shared;

namespace ProductApp.UseCasesInterfaces.UpdateProduct
{
    public interface IUpdateProductResponseObject : IPostResponseObject
    {
        ProductResponse ProductResponse { get; }
    }
}