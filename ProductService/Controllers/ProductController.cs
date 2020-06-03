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
    [Authorize(Roles="Admin,Products")]
    public class ProductController : ControllerBase
    {
        private IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
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
        public IActionResult GetProduct([FromQuery] string field = "", [FromQuery] string search = "")
        {
            var result = _productService.Handle( new GetProductsRequestObject(field,search));

            return StatusCode(result.Result.StatusCode, result.Result);
        }
    }
}