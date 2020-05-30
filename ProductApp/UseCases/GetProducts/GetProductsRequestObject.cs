using System.Collections.Generic;
using Domain.Domains;
using MongoDB.Driver;
using ProductApp.UseCasesInterfaces.GetProducts;
using Shared;

namespace ProductApp.UseCases.GetProducts
{
    public class GetProductsRequestObject : IGetProductsRequestObject
    {
        public int Skip { get; private set; }
        public int Limit { get; private set; }
        
        public string Field { get; private set; }
        public string Search { get; private set; } 


        public GetProductsRequestObject(int skip, int limit, string field = "", string search = "")
        {
            Field = field;
            Search = search; 
            Skip = skip;
            Limit = limit;
            ValidationNotifications = new List<ValidationNotification>();
        }

        public List<ValidationNotification> ValidationNotifications { get; private set; }
        public bool IsValid { get; private set; }

        public void Validate()
        {
            IsValid = true;
            if (Skip < 0)
            {
                IsValid = false;
                ValidationNotifications.Add(new ValidationNotification(Messages.SkipError)); 
            }

            if (Limit > 0) return;
            IsValid = false;
            ValidationNotifications.Add(new ValidationNotification(Messages.LimitError));
        }
    }
}