using System.Collections.Generic;
using ProductApp.UseCasesInterfaces.GetProducts;
using Shared;

namespace ProductApp.UseCases.GetProducts
{
    public class GetProductsRequestObject : IGetProductsRequestObject
    {
        public string Field { get; private set; }
        public string Search { get; private set; } 


        public GetProductsRequestObject(string field = "", string search = "")
        {
            Field = field;
            Search = search;
            ValidationNotifications = new List<ValidationNotification>();
        }

        public List<ValidationNotification> ValidationNotifications { get; private set; }
        public bool IsValid { get; private set; }

        public void Validate()
        {
            IsValid = true;
        }
    }
}