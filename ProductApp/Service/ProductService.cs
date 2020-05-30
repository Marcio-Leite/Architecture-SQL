using System.Threading.Tasks;
using ProductApp.UseCases.AddProduct;
using ProductApp.UseCasesInterfaces.AddProduct;
using ProductApp.UseCasesInterfaces.DeleteProduct;
using ProductApp.UseCasesInterfaces.GetProducts;
using ProductApp.UseCasesInterfaces.UpdateProduct;

namespace ProductApp.Service
{
    public class ProductService : IProductService
    {
        private readonly IAddProduct _addProduct;
        private readonly IUpdateProduct _updateProduct;
        private readonly IGetProducts _getProducts;
        private readonly IDeleteProduct _deleteProduct;

        public ProductService(IAddProduct addProduct = null, IGetProducts getProducts = null, IUpdateProduct updateProduct = null, IDeleteProduct deleteProduct = null)
        {
            _addProduct = addProduct;
            _getProducts = getProducts;
            _updateProduct = updateProduct;
            _deleteProduct = deleteProduct;
        }
        
        public async Task<IAddProductResponseObject> Handle(IAddProductRequestObject addProductRequestObject)
        {
            return await _addProduct.Handle(addProductRequestObject);
        }

        public async Task<IUpdateProductResponseObject> Handle(IUpdateProductRequestObject updateProductRequestObject)
        {
            return await _updateProduct.Handle(updateProductRequestObject);
        }

        public async Task<IGetProductsResponseObject> Handle(IGetProductsRequestObject getProductRequestObject)
        {
            return await _getProducts.Handle(getProductRequestObject);
        }

        public async Task<IDeleteProductResponseObject> Handle(IDeleteProductRequestObject deleteProductRequestObject)
        {
            return await _deleteProduct.Handle(deleteProductRequestObject);
        }
    }
}