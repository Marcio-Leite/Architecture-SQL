using System;
using System.Net;
using System.Threading.Tasks;
using Infra.Data.RepositoryInterfaces;
using MongoCoreDbRepository.Interfaces;
using Moq;
using NUnit.Framework;
using ProductApp.Service;
using ProductApp.UseCases.AddProduct;
using ProductApp.UseCases.GetProducts;
using ProductApp.UseCasesInterfaces.GetProducts;
using Shared;

namespace Application.Tests.Products.UseCases
{
    [TestFixture]
    public class GetProductsTests
    {
        private int VALID_LIMIT = 10;
        private int INVALID_LIMIT = 0;
        private int VALID_SKIP = 5;
        private int INVALID_SKIP = -5;
        
        private IProductService GetUseCase(IProductRepository productRepository = null)
        {
            return new ProductService(null, new GetProducts(productRepository));
        }

        private IGetProductsRequestObject GetRequestObject(int skip, int limit)
        {
            return new GetProductsRequestObject(skip, limit);
        }
        
        private Mock<IProductRepository> GetProductRepositoryMock()
        {
            return new Mock<IProductRepository>();
        }

        [Test]
        [TestCase(TestName = "Get products with invalid skip", Category = "Product App", Description = "Get products", TestOf = typeof(GetProducts))]
        public async Task WhenGivenAnInvalidSkip_ShouldReturnBadRequest()
        {
            // Arrange
            var useCase = GetUseCase();
            var requestObject = GetRequestObject(INVALID_SKIP, VALID_LIMIT);

            // Act
            var response = await useCase.Handle(requestObject);

            // Assert
            Assert.AreEqual((int)HttpStatusCode.BadRequest, response.StatusCode);
            CollectionAssert.Contains(response.ValidationNotifications, new ValidationNotification(Messages.SkipError));
        }
        
        [Test]
        [TestCase(TestName = "Get products with invalid limit", Category = "Product App", Description = "Get products", TestOf = typeof(GetProducts))]
        public async Task WhenGivenAnInvalidLimit_ShouldReturnBadRequest()
        {
            // Arrange
            var useCase = GetUseCase();
            var requestObject = GetRequestObject(VALID_SKIP, INVALID_LIMIT);

            // Act
            var response = await useCase.Handle(requestObject);

            // Assert
            Assert.AreEqual((int)HttpStatusCode.BadRequest, response.StatusCode);
            CollectionAssert.Contains(response.ValidationNotifications, new ValidationNotification(Messages.LimitError));
        }
        
        [Test]
        [TestCase(TestName = "Sucessfull Get product", Category = "Product App", Description = "Get products", TestOf = typeof(GetProducts))]
        public async Task WhenGivenAnValidGetProduct_ShouldReturnOk()
        {
            // Arrange
            var repositoryMock = GetProductRepositoryMock();
            repositoryMock
                .Setup(r => r.CheckIfProductExistsByDescription(It.IsAny<String>()))
                .Returns(Task.FromResult(false));

            var useCase = GetUseCase(repositoryMock.Object);
            var requestObject = GetRequestObject(VALID_SKIP, VALID_LIMIT);

            // Act
            var response = await useCase.Handle(requestObject);

            // Assert
            Assert.AreEqual((int)HttpStatusCode.OK, response.StatusCode);
        }
        
    }
}