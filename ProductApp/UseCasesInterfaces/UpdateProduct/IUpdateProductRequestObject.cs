using System;
using Shared;

namespace ProductApp.UseCasesInterfaces.UpdateProduct
{
    public interface IUpdateProductRequestObject : IValidator
    {
        Guid ProductId { get; }
        string Description { get; }
        decimal Price { get; }
        
    }
}