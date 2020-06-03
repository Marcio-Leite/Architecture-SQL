using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.ComTypes;
using Domain.Domains;
using ProductApp.UseCasesInterfaces.GetProducts;
using Shared;

namespace ProductApp.UseCases.GetProducts
{
    public class GetProductsResponseObject : IGetProductsResponseObject
    {
        public GetProductsResponseObject(int statusCode, IEnumerable<ValidationNotification> validationNotifications)
        {
            StatusCode = statusCode;
            ValidationNotifications = validationNotifications;
        }
        public GetProductsResponseObject(int statusCode, ValidationNotification validationNotification)
        {
            this.StatusCode = statusCode;
            this.ValidationNotifications = new List<ValidationNotification> { validationNotification };
        }
        public GetProductsResponseObject(IEnumerable<Product> products, long count)
        {
            StatusCode = (int) HttpStatusCode.OK;
            ProductsResponse = products.Select(c => new ProductResponse(c.ProductId, c.Description, c.Price)).ToList();
            Count = count;
        }
        
        public long Count { get; private set;}
        public IEnumerable<ValidationNotification> ValidationNotifications { get; }
        public int StatusCode { get; }
        public List<ProductResponse> ProductsResponse { get; private set;}
    }
}