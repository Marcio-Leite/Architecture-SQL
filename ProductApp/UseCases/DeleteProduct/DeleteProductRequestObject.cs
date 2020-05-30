using System;
using System.Collections.Generic;
using System.Xml;
using ProductApp.UseCasesInterfaces.DeleteProduct;
using Shared;

namespace ProductApp.UseCases.DeleteProduct
{
    public class DeleteProductRequestObject : IDeleteProductRequestObject
    {
        public DeleteProductRequestObject(Guid id)
        {
            Id = id;
            ValidationNotifications = new List<ValidationNotification>();
        }
        public List<ValidationNotification> ValidationNotifications { get; }
        public bool IsValid { get; private set; }
        public void Validate()
        {
            IsValid = false;
            if (IdValidGuid())
                IsValid = true;
        }

        private bool IdValidGuid()
        {
            if (!ValidationHelper.IsValidGuid(Id))
            {
                ValidationNotifications.Add(new ValidationNotification(Messages.ProductIdError));
                return false;
            }

            return true;
        }
        
        public int Skip { get; }
        public int Limit { get; }
        
        public Guid Id { get; private set; }
    }
}