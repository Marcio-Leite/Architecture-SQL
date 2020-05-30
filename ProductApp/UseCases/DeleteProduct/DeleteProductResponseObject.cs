using System.Collections.Generic;
using ProductApp.UseCasesInterfaces.DeleteProduct;
using Shared;

namespace ProductApp.UseCases.DeleteProduct
{
    public class DeleteProductResponseObject : IDeleteProductResponseObject
    {
        public DeleteProductResponseObject(int statusCode, IEnumerable<ValidationNotification> validationNotifications)
        {
            ValidationNotifications = validationNotifications;
            StatusCode = statusCode;
        }

        public DeleteProductResponseObject(int statusCode, ValidationNotification validationNotifications)
        {
            ValidationNotifications = new List<ValidationNotification>{validationNotifications};
            StatusCode = statusCode;
        }
        
        public DeleteProductResponseObject(int statusCode)
        {
            StatusCode = statusCode;
        }
        public IEnumerable<ValidationNotification> ValidationNotifications { get; }
        public int StatusCode { get; }
    }
}