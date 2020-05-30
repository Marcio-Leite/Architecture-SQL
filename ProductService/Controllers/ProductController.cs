using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using ProductApp.Service;
using ProductApp.UseCases.AddProduct;
using ProductApp.UseCases.DeleteProduct;
using ProductApp.UseCases.GetProducts;
using ProductApp.UseCases.UpdateProduct;
using ProductService.ViewModel;

namespace ProductService.Controllers
{
    [Route("productsService")]
    [ApiController]
    [Authorize(Roles="Admin,Product")]
    public class ProductController : ControllerBase
    {
        private IProductService _productService;
        private readonly IDistributedCache _distributedCache;
        
        public ProductController(IProductService productService, IDistributedCache distributedCache)
        {
            _productService = productService;
            _distributedCache = distributedCache;
        }
        
        [HttpPost, Route("product")]
        public IActionResult AddProduct([FromBody] ProductRequestDTO request)
        {
            var result = _productService.Handle( new AddProductRequestObject(request.Description,request.Price));

            return StatusCode(result.Result.StatusCode, result.Result);
        }
        
        [HttpPut, Route("product/{id}")]
        public IActionResult UpdateProduct([FromBody] ProductRequestDTO request, [FromRoute] Guid id)
        {
            var result = _productService.Handle( new UpdateProductRequestObject(id, request.Description,request.Price));

            return StatusCode(result.Result.StatusCode, result.Result);
        }
        
        [HttpDelete, Route("product/{id}")]
        public IActionResult UpdateProduct([FromRoute] Guid id)
        {
            var result = _productService.Handle( new DeleteProductRequestObject(id));

            return StatusCode(result.Result.StatusCode, result.Result);
        }
        
        [HttpGet, Route("products")]
        public IActionResult GetProduct([FromQuery]int skip, [FromQuery]int limit, [FromQuery] string field, [FromQuery] string search)
        {
            var result = _productService.Handle( new GetProductsRequestObject(skip,limit,field,search));

            return StatusCode(result.Result.StatusCode, result.Result);
        }
        
        
        /////
        
        [AllowAnonymous]
        [HttpGet, Route("product/teste")]
        public async Task<string> Get()
        {
            var cacheKey = "TheTime";
            var existingTime = _distributedCache.GetString(cacheKey);
            if (!string.IsNullOrEmpty(existingTime))
            {
                return "Fetched from cache : " + existingTime;
            }
            else
            {
                existingTime = DateTime.UtcNow.ToString();
                _distributedCache.SetString(cacheKey, existingTime);
                return "Added to cache : " + existingTime;
            }
        }
    }
}