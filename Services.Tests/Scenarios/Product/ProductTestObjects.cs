using System;
using Domain.Domains;
using Services.Tests.Fixtures;

namespace Services.Tests.Scenarios
{

    
    public class ProductTestObjects
    {
        internal ProductTestContext _productTestContext;
        internal GetAuthorization auth = new GetAuthorization();
        internal static readonly Guid TEST_GUID = Guid.NewGuid();

        internal Product VALID_PRODUCT = new Product(Guid.NewGuid(), "teste", 10); 
        
        internal object INVALID_BODY = new
        {
            Description = "",
            Price = 0 
        };
        internal object VALID_BODY = new
        {
            Description = "Produto Teste de Integracao " + TEST_GUID,
            Price = 10 
        };
        internal object VALID_EDIT_BODY = new
        {
            Description = "Produto Teste de Integracao editado " + TEST_GUID,
            Price = 10 
        };
        internal object WRONG_BODY = new
        {
            Descriptions = "Produto Teste de Integracao " + TEST_GUID,
            Size = 10 
        };
    }
}