using System;
using Shared;

namespace ProductApp.UseCasesInterfaces.AddProduct
{
    public interface IAddProductRequestObject : IValidator
    {
        Guid Id { get; }
        string Description { get; }
        decimal Price { get; }
    }
}