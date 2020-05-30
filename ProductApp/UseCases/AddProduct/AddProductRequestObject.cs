using System;
using System.Collections.Generic;
using ProductApp.UseCasesInterfaces.AddProduct;
using Shared;

namespace ProductApp.UseCases.AddProduct
{
    public class AddProductRequestObject : IAddProductRequestObject
    {
      
        public AddProductRequestObject(string description, decimal price)
        {
            Description = description;
            Price = price;
            ValidationNotifications = new List<ValidationNotification>();
        }
        
        public Guid Id { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; private set; }

        public List<ValidationNotification> ValidationNotifications { get; private set; }
        public bool IsValid { get; private set; }

        public void Validate()
        {
            if (IsValidDescription() &
                IsValidPrice())
                IsValid = true;
        }

        public int Skip { get; }
        public int Limit { get; }

        private bool IsValidPrice()
        {
            if (Price <= 0)
            {
                ValidationNotifications.Add(new ValidationNotification(Messages.ProductPriceError));
                return false;
            }
            return true;
        }

        private bool IsValidDescription()
        {
            if (String.IsNullOrWhiteSpace(Description))
            {
                ValidationNotifications.Add(new ValidationNotification(Messages.ProductDescriptionError));
                return false;
            }
            return true;
        }
    }
}