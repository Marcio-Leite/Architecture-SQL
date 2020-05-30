using System;
using System.Collections.Generic;
using System.Net;
using Domain.Domains;
using ProductApp.UseCasesInterfaces.AddProduct;
using Shared;

namespace ProductApp.UseCases.AddProduct
{
    public class AddProductResponseObject : IAddProductResponseObject
    {
        public AddProductResponseObject(int statusCode, IEnumerable<ValidationNotification> validationNotifications)
        {
            StatusCode = statusCode;
            ValidationNotifications = validationNotifications;
        }
        
        public AddProductResponseObject(int statusCode, ValidationNotification validationNotification)
        {
            this.StatusCode = statusCode;
            this.ValidationNotifications = new List<ValidationNotification> { validationNotification };
        }

        public AddProductResponseObject(Product product)
        {
            StatusCode = (int)HttpStatusCode.OK;
            ProductResponse = new ProductResponse(product.Id, product.Description, product.Price);
        }
        public ProductResponse ProductResponse { get; private set; }
        public IEnumerable<ValidationNotification> ValidationNotifications { get; private set; }
        public int StatusCode { get;  private set;}
    }
}