using System;
using System.Collections.Generic;
using ProductApp.UseCasesInterfaces.UpdateProduct;
using Shared;

namespace ProductApp.UseCases.UpdateProduct
{
    public class UpdateProductRequestObject : IUpdateProductRequestObject 
    {
        public UpdateProductRequestObject(Guid? id, string description, decimal price)
        {
            ProductId = Guid.TryParse(id.ToString(), out var result)  ? Guid.Parse(id.ToString()) : Guid.Empty;
            Description = description;
            Price = price;
            ValidationNotifications = new List<ValidationNotification>();
        }
        public List<ValidationNotification> ValidationNotifications { get; }
        public bool IsValid { get; private set; }
        
        public int Skip { get; }
        public int Limit { get; }
        public Guid ProductId { get; private set;}
        public string Description { get; private set;}
        public decimal Price { get; private set;}
        
        public void Validate()
        {
            if (IsValidGuid() &
                IsValidDescription() &
                IsValidPrice())
                IsValid = true;
        }

        private bool IsValidGuid()
        {
            if (ValidationHelper.IsValidGuid(ProductId)) return true;
            this.ValidationNotifications.Add(new ValidationNotification(Messages.ProductIdError));
            return false;
        }

        private bool IsValidPrice()
        {
            if (Price > 0) return true;
            this.ValidationNotifications.Add(new ValidationNotification(Messages.ProductPriceError));
            return false;

        }

        private bool IsValidDescription()
        {
            if (string.IsNullOrWhiteSpace(Description))
            {
                this.ValidationNotifications.Add(new ValidationNotification(Messages.ProductDescriptionError));
                return false;
            }

            return true;
        }
    }
}