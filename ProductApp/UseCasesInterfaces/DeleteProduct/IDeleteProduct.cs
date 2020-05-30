using System.Threading.Tasks;

namespace ProductApp.UseCasesInterfaces.DeleteProduct
{
    public interface IDeleteProduct
    {
        Task<IDeleteProductResponseObject> Handle(IDeleteProductRequestObject deleteProductRequestObject);
    }
}