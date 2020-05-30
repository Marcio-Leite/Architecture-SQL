using System;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using IdentityServerMongo.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Services.Tests.Fixtures
{
    public class GetAuthorization
    {
        
        public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
        {
            public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, 
                ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
                : base(options, logger, encoder, clock)
            {
            }

            protected override Task<AuthenticateResult> HandleAuthenticateAsync()
            {
                var claims = new[] { new Claim(ClaimTypes.Name, "Test user") };
                var identity = new ClaimsIdentity(claims, "Test");
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, "Test");

                var result = AuthenticateResult.Success(ticket);

                return Task.FromResult(result);
            }
        }
        
        
        
        private readonly ProductTestContext _productTestContext;

        private static readonly Guid TEST_GUID = Guid.NewGuid();
        
        public GetAuthorization()
        {
            _productTestContext = new ProductTestContext();
        }

        
        private object AUTHORIZATION_BODY = new
        {
            Email = "marcio.leite@shiftlabs.com.br",
            Password = "teste@123" 
        };
        
        public async Task<string> Request()
        {
            // Arrange
            string token="";
            
            var request = new
            {
                Url = "user/login",
                Body = AUTHORIZATION_BODY
            };
            // Act
            var response = await _productTestContext.IdentityClient.PostAsync(request.Url,  ContentHelper.GetStringContent(request.Body));
            if (response.IsSuccessStatusCode)
            {
                var serializedResponse = await response.Content.ReadAsStringAsync();
                var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(serializedResponse);
                token = loginResponse.Token;
            }

            return token;
        }
    }
}