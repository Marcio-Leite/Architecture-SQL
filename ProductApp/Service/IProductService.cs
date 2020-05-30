using System.Threading.Tasks;
using ProductApp.UseCases.AddProduct;
using ProductApp.UseCasesInterfaces.AddProduct;
using ProductApp.UseCasesInterfaces.DeleteProduct;
using ProductApp.UseCasesInterfaces.GetProducts;
using ProductApp.UseCasesInterfaces.UpdateProduct;

namespace ProductApp.Service
{
    public interface IProductService
    {
        Task<IAddProductResponseObject> Handle(IAddProductRequestObject addProductRequestObject);
        Task<IUpdateProductResponseObject> Handle(IUpdateProductRequestObject updateProductRequestObject);
        Task<IGetProductsResponseObject> Handle(IGetProductsRequestObject getProductRequestObject);
        Task<IDeleteProductResponseObject> Handle(IDeleteProductRequestObject deleteProductRequestObject);
    }
}