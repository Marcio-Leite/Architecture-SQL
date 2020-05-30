using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Services.Tests.Fixtures;
using Xunit;

namespace Services.Tests.Scenarios
{
    public class GetProductIntegrationTests : ProductTestObjects
    {
        public GetProductIntegrationTests()
        {
            _productTestContext = new ProductTestContext();
        }
        
        [Fact]
        public async Task GetProductsWithWrongRequest_ReturnsBadRequestResponse()
        {
            // Arrange
            var request = new
            {
                Url = "/productsService/products?skip=0&limit=0"
            };
            // Act
            var response = await _productTestContext.Client.GetAsync(request.Url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        
        [Fact]
        public async Task GetProductWithRightRequest_ReturnsOkResponse()
        {
            // Arrange
            var request = new
            {
                Url = "/productsService/products?skip=0&limit=10"
            };
            // Act
            var response = await _productTestContext.Client.GetAsync(request.Url);

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}