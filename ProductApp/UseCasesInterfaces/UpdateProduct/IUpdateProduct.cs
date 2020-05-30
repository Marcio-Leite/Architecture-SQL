using System.Threading.Tasks;
using ProductApp.UseCasesInterfaces.AddProduct;

namespace ProductApp.UseCasesInterfaces.UpdateProduct
{
    public interface IUpdateProduct
    {
        Task<IUpdateProductResponseObject> Handle(IUpdateProductRequestObject requestObject);
    }
}