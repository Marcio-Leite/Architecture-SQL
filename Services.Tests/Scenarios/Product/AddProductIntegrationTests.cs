using System.Net;
using System.Threading.Tasks;
using Domain.Domains;
using FluentAssertions;
using Moq;
using Services.Tests.Fixtures;
using Xunit;

namespace Services.Tests.Scenarios
{
    public class AddProductIntegrationTests : ProductTestObjects
    {
        public AddProductIntegrationTests()
        {
            _productTestContext = new ProductTestContext();
        }
        
        [Fact]
        public async Task AddProductWithInvalidRequest_ReturnsBadRequestResponse()
        {
            // Arrange

            var request = new
            {
                Url = "/productsService/product",
                Body = INVALID_BODY
            };
            
            // Act
            var response = await _productTestContext.Client.PostAsync(request.Url,  ContentHelper.GetStringContent(request.Body));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        
        [Fact]
        public async Task AddProductWithWrongRequest_ReturnsBadRequestResponse()
        {
            // Arrange

            var request = new
            {
                Url = "/productsService/product",
                Body = WRONG_BODY
            };
            // Act
            var response = await _productTestContext.Client.PostAsync(request.Url,  ContentHelper.GetStringContent(request.Body));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        
        [Fact]
        public async Task AddProductWithRightRequest_ReturnsOkResponse()
        {
            // Arrange
            _productTestContext.productRepositoryMock.Setup(x => x.Add(It.IsAny<Product>()));
            _productTestContext.unitOfWorkMock.Setup(x => x.Commit()).Returns(Task.FromResult(true));
            
            var request = new
            {
                Url = "/productsService/product",
                Body = VALID_BODY
            };
            // Act
            var response = await _productTestContext.Client.PostAsync(request.Url,  ContentHelper.GetStringContent(request.Body));

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}