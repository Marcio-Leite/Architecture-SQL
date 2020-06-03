using System;
using System.Net;
using System.Threading.Tasks;
using Domain.Domains;
using Infra.Data.RepositoryInterfaces;
using Repository.Interfaces;
using ProductApp.UseCases.AddProduct;
using ProductApp.UseCasesInterfaces.UpdateProduct;
using Shared;
using IProductRepository = Infra.Data.RepositoryInterfaces.IProductRepository;

namespace ProductApp.UseCases.UpdateProduct
{
    public class UpdateProduct : IUpdateProduct
    {
        private readonly IUnitOfWorkDapper _uow;

        public UpdateProduct(IUnitOfWorkDapper uow)
        {
            _uow = uow;
        }
        
        public async Task<IUpdateProductResponseObject> Handle(IUpdateProductRequestObject requestObject)
        {
            try
            {
                requestObject.Validate();

                if (!requestObject.IsValid)
                    return new UpdateProductResponseObject((int) HttpStatusCode.BadRequest,
                        requestObject.ValidationNotifications);

                var productExists = await CheckIfProductExists(requestObject.ProductId);
                
                if (!productExists)
                    return new UpdateProductResponseObject((int) HttpStatusCode.NotFound, new ValidationNotification(Messages.ProductIdError));
                
                if (await DescriptionExists(requestObject.Description))
                    return new UpdateProductResponseObject((int) HttpStatusCode.BadRequest,
                        new ValidationNotification(Messages.ProductDescriptionExistError));
                
                Product product = new Product(requestObject.ProductId, requestObject.Description, requestObject.Price); 
                
                _uow.ProductRepository.Update(product);

                if (!_uow.Commit())
                    return new UpdateProductResponseObject((int) HttpStatusCode.InternalServerError, new ValidationNotification(Messages.DatabaseError)); 
                
                return new UpdateProductResponseObject(product);
            }
            catch (Exception e)
            {
                return new UpdateProductResponseObject((int) HttpStatusCode.InternalServerError, new ValidationNotification(Messages.ServerError, e.Message)); 
            }
        }

        private async Task<bool> CheckIfProductExists(Guid requestObjectProductId)
        {
            var product = await _uow.ProductRepository.GetById(requestObjectProductId);
            return product != null;
        }
        
        private async Task<bool> DescriptionExists(string description)
        {
            return await _uow.ProductRepository.CheckIfProductExistsByDescription(description);
        }
    }
}