using System.Collections.Generic;
using System.Linq;

namespace Shared
{
    public static class Messages
    {
        //generic and server error
        public static Dictionary<string, string> DatabaseError = new Dictionary<string, string>
            {{"","Não foi possível gravar no banco de dados."}};
        
        public static Dictionary<string, string> ServerError = new Dictionary<string, string>
            {{"","Ocorreu um erro no servidor: "}};
        
        public static Dictionary<string, string> SkipError = new Dictionary<string, string> 
            {{"skip","Skip deve ser maior ou igual a zero."}};
        
        public static Dictionary<string, string> LimitError = new Dictionary<string, string>
            {{"limit", "Limit deve ser maior que zero."}};
        
        //product error
        public static Dictionary<string, string> ProductDescriptionError = new Dictionary<string, string>
            {{"description", "Descrição de produto inválida."}};
        
        public static Dictionary<string, string> ProductDescriptionExistError = new Dictionary<string, string>
            {{"description","Descrição de produto informada já existe."}};
        
        public static Dictionary<string, string> ProductPriceError = new Dictionary<string, string>
            {{"price","Preço de produto precisa ser maior que zero."}};

        public static Dictionary<string, string> ProductIdError = new Dictionary<string, string> 
            {{"id","Id de Produto informado é inválido."}};
        
        public static Dictionary<string, string> ProductSearchFieldError = new Dictionary<string, string> 
            {{"field","O campo informado para busca não existe:"}};
        
        public static string GetField(this IDictionary<string, string> error)
        {
            return error.First().Key;
        }
        
        public static string GetMessage(this IDictionary<string, string> error)
        {
            return error.First().Value;
        }
        
        public static (string,string) GetMessages(this IDictionary<string, string> error)
        {
            return (error.First().Key , error.First().Value);
        }
    }
}