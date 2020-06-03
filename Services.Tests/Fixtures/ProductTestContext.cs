using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Repository.Interfaces;
using WebMotions.Fake.Authentication.JwtBearer;

namespace Services.Tests.Fixtures
{
    public class ProductTestContext
    {
        public HttpClient Client { get; set; }
        public HttpClient IdentityClient { get; set; }
        private TestServer _server;
        public Mock<IUnitOfWorkDapper> unitOfWorkMock;
        public ProductTestContext()
        {
            SetupClient();
        }

        private void SetupClient()
        {
            unitOfWorkMock = new Mock<IUnitOfWorkDapper>();
           
            _server = new TestServer(new WebHostBuilder()
                .UseStartup<ProductService.Startup>()
                .ConfigureTestServices(services =>
                {
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