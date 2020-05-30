using System.Collections.Generic;
using ProductApp.UseCases;
using Shared;

namespace ProductApp.UseCasesInterfaces.GetProducts
{
    public interface IGetProductsResponseObject : IGetResponseObject
    {
        List<ProductResponse> ProductsResponse { get; }
    }
}