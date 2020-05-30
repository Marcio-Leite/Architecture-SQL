using System.Net;
using System.Net.Http;
using Infra.Data.RepositoryInterfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using MongoCoreDbRepository.Interfaces;
using Moq;
using WebMotions.Fake.Authentication.JwtBearer;

namespace Services.Tests.Fixtures
{
    public class ProductTestContext
    {
        public HttpClient Client { get; set; }
        public HttpClient IdentityClient { get; set; }
        private TestServer _server;
        public Mock<IProductRepository> productRepositoryMock;
        public Mock<IUnitOfWork> unitOfWorkMock;
        public ProductTestContext()
        {
            SetupClient();
        }

        private void SetupClient()
        {
            productRepositoryMock = new Mock<IProductRepository>();
            unitOfWorkMock = new Mock<IUnitOfWork>();
           
            _server = new TestServer(new WebHostBuilder()
                .UseStartup<ProductService.Startup>()
                .ConfigureTestServices(services =>
                {
                    services.AddSingleton(productRepositoryMock.Object);
                    services.AddSingleton(unitOfWorkMock.Object);
                    
                    //fake bearer token
                    services.AddAuthentication(options =>
                    {
                        options.DefaultScheme = FakeJwtBearerDefaults.AuthenticationScheme;
                        options.DefaultAuthenticateScheme = FakeJwtBearerDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = FakeJwtBearerDefaults.AuthenticationScheme;
                    }).AddFakeJwtBearer();
                }));

            Client = _server.CreateClient();
            Client.SetFakeBearerToken("admin", new[] { "Admin", "Product" });

        }
    }
}