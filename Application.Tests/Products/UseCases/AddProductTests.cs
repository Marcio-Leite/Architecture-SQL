using System;
using System.Net;
using System.Threading.Tasks;
using Infra.Data.RepositoryInterfaces;
using MongoCoreDbRepository.Interfaces;
using Moq;
using NUnit.Framework;
using ProductApp.Service;
using ProductApp.UseCases.AddProduct;
using ProductApp.UseCasesInterfaces.AddProduct;
using Shared;

namespace Application.Tests.Products.UseCases
{
    [TestFixture]
    public class AddProductTests
    {
        private string INVALID_DESCRIPTION = "";
        private string VALID_DESCRIPTION = "Café Pilão 500g";
        private decimal VALID_PRICE = Convert.ToDecimal(10.50);
        private decimal INVALID_PRICE = 0;
        private IProductService GetUseCase(IProductRepository productRepository = null, IUnitOfWork unitOfWork = null)
        {
            return new ProductService(new AddProduct(productRepository,unitOfWork));
        } 
        
        private IAddProductRequestObject GetInvalidDescriptionRequestObject()
        {
            return new AddProductRequestObject(INVALID_DESCRIPTION, VALID_PRICE);
        }

        private IAddProductRequestObject GetValidRequestObject()
        {
            return new AddProductRequestObject(VALID_DESCRIPTION, VALID_PRICE);
        }
        
        private IAddProductRequestObject GetInvalidPriceRequestObject()
        {
            return new AddProductRequestObject(VALID_DESCRIPTION, INVALID_PRICE);
        }

        private Mock<IProductRepository> GetProductRepositoryMock()
        {
            return new Mock<IProductRepository>();
        }

        private Mock<IUnitOfWork> GetUowMock()
        {
            return new Mock<IUnitOfWork>();
        }
        
        [Test]
        [TestCase(TestName = "Add product with invalid description", Category = "Product App", Description = "Add new product", TestOf = typeof(AddProduct))]
        public async Task WhenGivenAnInvalidDescription_ShouldReturnBadRequest()
        {
            // Arrange
            var useCase = GetUseCase();
            var requestObject = GetInvalidDescriptionRequestObject();

            // Act
            var response = await useCase.Handle(requestObject);

            // Assert
            Assert.AreEqual((int)HttpStatusCode.BadRequest, response.StatusCode);
            CollectionAssert.Contains(response.ValidationNotifications, new ValidationNotification(Messages.ProductDescriptionError));
        }
        
        [Test]
        [TestCase(TestName = "Add product with invalid price", Category = "Product App", Description = "Add new product", TestOf = typeof(AddProduct))]
        public async Task WhenGivenAnInvalidPrice_ShouldReturnBadRequest()
        {
            // Arrange
            var useCase = GetUseCase();
            var requestObject = GetInvalidPriceRequestObject();

            // Act
            var response = await useCase.Handle(requestObject);

            // Assert
            Assert.AreEqual((int)HttpStatusCode.BadRequest, response.StatusCode);
            CollectionAssert.Contains(response.ValidationNotifications, new ValidationNotification(Messages.ProductPriceError));
        }
        
        [Test]
        [TestCase(TestName = "Add product with existing description", Category = "Product App", Description = "Add new product", TestOf = typeof(AddProduct))]
        public async Task WhenGivenAnExistingDescription_ShouldReturnBadRequest()
        {
            // Arrange
            var repositoryMock = GetProductRepositoryMock();
            repositoryMock
                .Setup(r => r.CheckIfProductExistsByDescription(It.IsAny<String>()))
                .Returns(Task.FromResult(true));
            var useCase = GetUseCase(repositoryMock.Object);
            var requestObject = GetValidRequestObject();

            // Act
            var response = await useCase.Handle(requestObject);

            // Assert
            Assert.AreEqual((int)HttpStatusCode.BadRequest, response.StatusCode);
            CollectionAssert.Contains(response.ValidationNotifications, new ValidationNotification(Messages.ProductDescriptionExistError));
        }
        
        [Test]
        [TestCase(TestName = "Sucessfull Add product", Category = "Product App", Description = "Add new product", TestOf = typeof(AddProduct))]
        public async Task WhenGivenAnValidProduct_ShouldReturnOk()
        {
            // Arrange
            var repositoryMock = GetProductRepositoryMock();
            repositoryMock
                .Setup(r => r.CheckIfProductExistsByDescription(It.IsAny<String>()))
                .Returns(Task.FromResult(false));
            
            var uowMock = GetUowMock();
            uowMock.Setup(x => x.Commit()).Returns(Task.FromResult(true));
            
            var useCase = GetUseCase(repositoryMock.Object, uowMock.Object);
            var requestObject = GetValidRequestObject();

            // Act
            var response = await useCase.Handle(requestObject);

            // Assert
            Assert.AreEqual((int)HttpStatusCode.OK, response.StatusCode);
        }
    }
}