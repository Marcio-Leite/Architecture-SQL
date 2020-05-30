using System.Threading.Tasks;

namespace ProductApp.UseCasesInterfaces.GetProducts
{
    public interface IGetProducts
    {
        Task<IGetProductsResponseObject> Handle(IGetProductsRequestObject requestObject);
    }
}