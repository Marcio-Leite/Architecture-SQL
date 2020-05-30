using System.Threading.Tasks;
using ProductApp.UseCases.AddProduct;

namespace ProductApp.UseCasesInterfaces.AddProduct
{
    public interface IAddProduct
    {
        Task<IAddProductResponseObject> Handle(IAddProductRequestObject requestObject);
    }
}