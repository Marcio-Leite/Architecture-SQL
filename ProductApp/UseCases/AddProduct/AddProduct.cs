using System;
using System.Net;
using System.Threading.Tasks;
using Domain.Domains;
using Infra.Data.RepositoryInterfaces;
using Repository.Interfaces;
using ProductApp.UseCasesInterfaces.AddProduct;
using Shared;

namespace ProductApp.UseCases.AddProduct
{
    public class AddProduct : IAddProduct
    {
        private readonly IUnitOfWorkDapper _uow;

        public AddProduct( IUnitOfWorkDapper uow)
        {
            _uow = uow;
        }

        public async Task<IAddProductResponseObject> Handle(IAddProductRequestObject requestObject)
        {
            try
            {
                requestObject.Validate();

                if (!requestObject.IsValid)
                    return new AddProductResponseObject((int) HttpStatusCode.BadRequest,
                        requestObject.ValidationNotifications);

                if (await DescriptionExists(requestObject.Description))
                    return new AddProductResponseObject((int) HttpStatusCode.BadRequest,
                        new ValidationNotification(Messages.ProductDescriptionExistError));

                var product = new Product(Guid.NewGuid(), requestObject.Description, requestObject.Price);

                _uow.ProductRepository.Add(product);

                if (! _uow.Commit())
                    return new AddProductResponseObject((int) HttpStatusCode.InternalServerError,
                        new ValidationNotification(Messages.DatabaseError));

                return new AddProductResponseObject(product);
            }
            catch (Exception e)
            {
                return new AddProductResponseObject((int)HttpStatusCode.InternalServerError,
                    new ValidationNotification(Messages.ServerError, e.Message));
            }
        }

        private async Task<bool> DescriptionExists(string description)
        {
            return await _uow.ProductRepository.CheckIfProductExistsByDescription(description);
        }
        
    }
}