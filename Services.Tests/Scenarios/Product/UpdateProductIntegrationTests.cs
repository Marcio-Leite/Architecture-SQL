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
    public class UpdateProductIntegrationTests : ProductTestObjects
    {
        public UpdateProductIntegrationTests()
        {
            _productTestContext = new ProductTestContext();
        }
        
        [Fact]
        public async Task UpdateProductWithRightRequest_ReturnsOkResponse()
        {
            // Arrange

            _productTestContext.productRepositoryMock.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(Task.FromResult(VALID_PRODUCT));
            _productTestContext.productRepositoryMock.Setup(x => x.Update(It.IsAny<Product>()));
            _productTestContext.unitOfWorkMock.Setup(x => x.Commit()).Returns(Task.FromResult(true));

            var request = new
            {
                Url = "/productsService/product/" + Guid.NewGuid(),
                Body = VALID_EDIT_BODY
            };
            // Act
            var response = await _productTestContext.Client.PutAsync(request.Url,  ContentHelper.GetStringContent(request.Body));

            // Assert
            response.EnsureSuccessStatusCode();
        }
        
        [Fact]
        public async Task UpdateProductWithWrongRequest_ReturnsBadRequestResponse()
        {
            // Arrange
            var request = new
            {
                Url = "/productsService/product/" + Guid.NewGuid(),
                Body = INVALID_BODY
            };
            // Act
            var response = await _productTestContext.Client.PutAsync(request.Url,  ContentHelper.GetStringContent(request.Body));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        
        [Fact]
        public async Task UpdateProductWithWrongRequest_ReturnsNotFoundResponse()
        {
            // Arrange
            var request = new
            {
                Url = "/productsService/product/" + Guid.NewGuid(),
                Body = VALID_BODY
            };
            // Act
            var response = await _productTestContext.Client.PutAsync(request.Url,  ContentHelper.GetStringContent(request.Body));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}