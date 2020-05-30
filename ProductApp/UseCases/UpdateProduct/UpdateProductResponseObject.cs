using System.Collections.Generic;
using System.Net;
using Domain.Domains;
using ProductApp.UseCasesInterfaces.UpdateProduct;
using ServiceStack;
using Shared;

namespace ProductApp.UseCases.UpdateProduct
{
    public class UpdateProductResponseObject : IUpdateProductResponseObject
    {
        public ProductResponse ProductResponse { get; }

        public UpdateProductResponseObject(Product product)
        {
            StatusCode = (int) HttpStatusCode.OK;
            ProductResponse = new ProductResponse(product.Id, product.Description, product.Price);
            ValidationNotifications = new List<ValidationNotification>();
        }
        
        public UpdateProductResponseObject(int statusCode, IEnumerable<ValidationNotification> validationNotifications)
        {
            StatusCode = statusCode;
            ValidationNotifications = validationNotifications;
        }
        
        public UpdateProductResponseObject(int statusCode, ValidationNotification validationNotification)
        {
            this.StatusCode = statusCode;
            this.ValidationNotifications = new List<ValidationNotification> { validationNotification };
        }

        public IEnumerable<ValidationNotification> ValidationNotifications { get; }
        public int StatusCode { get; }
    }
}