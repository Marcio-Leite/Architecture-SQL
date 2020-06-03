using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Domain.Domains;
using Infra.Data.RepositoryInterfaces;
using Microsoft.Extensions.Caching.Distributed;
using Repository.Interfaces;
using ProductApp.UseCasesInterfaces.GetProducts;
using ServiceStack.Script;
using Shared;

namespace ProductApp.UseCases.GetProducts
{
    public class GetProducts : IGetProducts
    {
        private readonly IUnitOfWorkDapper _uow;

        public GetProducts(IUnitOfWorkDapper uow)
        {
            _uow = uow;
        }
        
        public async Task<IGetProductsResponseObject> Handle(IGetProductsRequestObject requestObject)
        {
            IEnumerable<Product> products = null;
            long totalProducts = 0;
            try
            {
                requestObject.Validate();

                if (!requestObject.IsValid)
                    return new GetProductsResponseObject((int) HttpStatusCode.BadRequest,
                        requestObject.ValidationNotifications);


                if (String.IsNullOrWhiteSpace(requestObject.Field))
                {
                    products = await _uow.ProductRepository.Query();
                    totalProducts = products.Count();
                }
                else
                {
                    var fieldProperties = typeof(Product).GetProperties().FirstOrDefault(prop => prop.Name == requestObject.Field);
                    
                    if (fieldProperties == null)
                        return new GetProductsResponseObject((int)HttpStatusCode.BadRequest, new ValidationNotification(Messages.ProductSearchFieldError, requestObject.Field));

                    if (Decimal.TryParse(requestObject.Search, out _))
                    {
                        products = 
                            await _uow.ProductRepository.Query(fieldProperties.Name + " = @queryValue" ,new {queryValue = requestObject.Search});
                    }
                    else
                    {
                        products = 
                            await _uow.ProductRepository.Query(fieldProperties.Name + " like @queryValue" ,new {queryValue = "%" + requestObject.Search + "%"});
                    }
                       

                    totalProducts = products.Count();
                }

                return new GetProductsResponseObject(products, totalProducts);
            }
            catch (Exception e)
            {
                return new GetProductsResponseObject((int)HttpStatusCode.InternalServerError, new ValidationNotification(Messages.ServerError, e.Message));
            }
        }
    }
}