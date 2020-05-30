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
using MongoDB.Driver;
using ProductApp.UseCasesInterfaces.GetProducts;
using ServiceStack.Script;
using Shared;

namespace ProductApp.UseCases.GetProducts
{
    public class GetProducts : IGetProducts
    {
        private readonly IProductRepository _productRepository;
        private readonly IDistributedCache _distributedCache;

        public GetProducts(IProductRepository productRepository, IDistributedCache distributedCache = null)
        {
            _productRepository = productRepository;
            _distributedCache = distributedCache;
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
                    totalProducts = await _productRepository.Count();
                    products = await _productRepository.GetAll(requestObject.Skip, requestObject.Limit, "Description");
                }
                else
                {
                    var fieldProperties = typeof(Product).GetProperties().FirstOrDefault(prop => prop.Name == requestObject.Field);
                    
                    if (fieldProperties == null)
                        return new GetProductsResponseObject((int)HttpStatusCode.BadRequest, new ValidationNotification(Messages.ProductSearchFieldError, requestObject.Field));

                    products = 
                        await _productRepository.GetByFilterLike(fieldProperties.Name,requestObject.Search,requestObject.Skip, requestObject.Limit, fieldProperties.Name);

                    totalProducts =
                        await _productRepository.CountByFilterLike(fieldProperties.Name,requestObject.Search);
                }

                return new GetProductsResponseObject(products, requestObject.Skip, requestObject.Limit, totalProducts);
            }
            catch (Exception e)
            {
                return new GetProductsResponseObject((int)HttpStatusCode.InternalServerError, new ValidationNotification(Messages.ServerError, e.Message));
            }
        }
    }
}