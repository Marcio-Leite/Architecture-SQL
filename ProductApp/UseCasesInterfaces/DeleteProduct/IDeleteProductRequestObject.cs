using System;
using Shared;

namespace ProductApp.UseCasesInterfaces.DeleteProduct
{
    public interface IDeleteProductRequestObject : IValidator
    {
        Guid Id { get; }
    }
}