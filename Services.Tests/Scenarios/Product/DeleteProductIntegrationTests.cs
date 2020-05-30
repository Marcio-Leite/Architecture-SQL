using System;
using System.Net;
using System.Threading.Tasks;
using Domain.Domains;
using FluentAssertions;
using Moq;
using Services.Tests.Fixtures;
using Xunit;

namespace Services.Tests.Scenarios
{
    public class DeleteProductIntegrationTests : ProductTestObjects
    {
        public DeleteProductIntegrationTests()
        {
            _productTestContext = new ProductTestContext();
        }
        
        [Fact]
        public async Task DeleteProductWithRightRequest_ReturnsOkResponse()
        {
            // Arrange
            _productTestContext.productRepositoryMock.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(Task.FromResult(VALID_PRODUCT));
            _productTestContext.productRepositoryMock.Setup(x => x.Remove(It.IsAny<Guid>()));
            _productTestContext.unitOfWorkMock.Setup(x => x.Commit()).Returns(Task.FromResult(true));
            
            var request = new
            {
                Url = "/productsService/product/" + Guid.NewGuid(),
            };
            // Act
            var response = await _productTestContext.Client.DeleteAsync(request.Url);

            // Assert
            response.EnsureSuccessStatusCode();
        }
        
        [Fact]
        public async Task DeleteProductWithWrongRequest_ReturnsBadRequestResponse()
        {
            // Arrange
            var request = new
            {
                Url = "/productsService/product/" + "abc",
            };
            // Act
            var response = await _productTestContext.Client.DeleteAsync(request.Url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        
        [Fact]
        public async Task DeleteProductWithWrongRequest_ReturnsNotFoundResponse()
        {
            // Arrange
            _productTestContext.productRepositoryMock.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((Product) null));
            
            var request = new
            {
                Url = "/productsService/product/" + Guid.NewGuid(),
            };
            // Act
            var response = await _productTestContext.Client.DeleteAsync(request.Url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}