using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Infra.Data.RepositoryInterfaces;
using Microsoft.AspNetCore.Http;
using Repository.Interfaces;
using ProductApp.UseCasesInterfaces.DeleteProduct;
using ServiceStack.NativeTypes.Java;
using Shared;

namespace ProductApp.UseCases.DeleteProduct
{
    public class DeleteProduct : IDeleteProduct
    {
        private IProductRepository _productRepository;
        private IUnitOfWork _unitOfWork;
        
        public DeleteProduct(IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }
        
        public async Task<IDeleteProductResponseObject> Handle(IDeleteProductRequestObject deleteProductRequestObject)
        {
            try
            {
                deleteProductRequestObject.Validate();

                if (!deleteProductRequestObject.IsValid)
                    return new DeleteProductResponseObject((int) HttpStatusCode.BadRequest,
                        deleteProductRequestObject.ValidationNotifications);
                
                if (await _productRepository.GetById(deleteProductRequestObject.Id) == null)
                    return new DeleteProductResponseObject((int) HttpStatusCode.NotFound,
                        new ValidationNotification(Messages.ProductIdError));

                _productRepository.Remove(deleteProductRequestObject.Id);

                if (!await _unitOfWork.Commit())
                    return new DeleteProductResponseObject((int) HttpStatusCode.InternalServerError,
                        new ValidationNotification(Messages.DatabaseError));

                return new DeleteProductResponseObject((int) HttpStatusCode.OK);
            }catch (Exception e)
            {
                return new DeleteProductResponseObject((int) HttpStatusCode.InternalServerError, new ValidationNotification(Messages.ServerError, e.Message)); 
            }
        }
    }
}