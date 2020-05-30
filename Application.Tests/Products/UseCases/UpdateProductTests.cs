using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using Domain.Domains;
using Infra.Data.RepositoryInterfaces;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using MongoCoreDbRepository.Interfaces;
using Moq;
using NUnit.Framework;
using ProductApp.Service;
using ProductApp.UseCases.AddProduct;
using ProductApp.UseCases.UpdateProduct;
using ProductApp.UseCasesInterfaces.UpdateProduct;
using Shared;

namespace Application.Tests.Products.UseCases
{
    [TestFixture]
    public class UpdateProductTests
    {


        private string INVALID_DESCRIPTION = "";
        private string VALID_DESCRIPTION = "Café Pilão 500g";
        private decimal VALID_PRICE = Convert.ToDecimal(10.50);
        private decimal INVALID_PRICE = 0;
        private Guid VALID_GUID = Guid.NewGuid();
        private Guid INVALID_GUID = Guid.Empty;
        private IProductService GetUseCase(IProductRepository productRepository = null, IUnitOfWork unitOfWork = null)
        {
            return new ProductService(null, null, new UpdateProduct(productRepository,unitOfWork));
        } 
        
        private IUpdateProductRequestObject GetUpdateProductRequestObject(Guid id, string description, decimal price)
        {
            return new UpdateProductRequestObject(id, description, price);
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
        [TestCase(TestName = "Update product with invalid description", Category = "Product App", Description = "Update product", TestOf = typeof(UpdateProduct))]
        public async Task WhenGivenAnInvalidDescription_ShouldReturnBadRequest()
        {
            // Arrange
            var useCase = GetUseCase();
            var requestObject = GetUpdateProductRequestObject(VALID_GUID, INVALID_DESCRIPTION, VALID_PRICE);

            // Act
            var response = await useCase.Handle(requestObject);

            // Assert
            Assert.AreEqual((int)HttpStatusCode.BadRequest, response.StatusCode);
            CollectionAssert.Contains(response.ValidationNotifications, new ValidationNotification(Messages.ProductDescriptionError));
        }
        
        [Test]
        [TestCase(TestName = "Update product with invalid price", Category = "Product App", Description = "Update product", TestOf = typeof(UpdateProduct))]
        public async Task WhenGivenAnInvalidPrice_ShouldReturnBadRequest()
        {
            // Arrange
            var useCase = GetUseCase();
            var requestObject = GetUpdateProductRequestObject(VALID_GUID, VALID_DESCRIPTION, INVALID_PRICE);
            
            // Act
            var response = await useCase.Handle(requestObject);

            // Assert
            Assert.AreEqual((int)HttpStatusCode.BadRequest, response.StatusCode);
            CollectionAssert.Contains(response.ValidationNotifications, new ValidationNotification(Messages.ProductPriceError));
        }
        
        [Test]
        [TestCase(TestName = "Update product with existing description", Category = "Product App", Description = "Update product", TestOf = typeof(UpdateProduct))]
        public async Task WhenGivenAnExistingDescription_ShouldReturnBadRequest()
        {
            // Arrange
            var repositoryMock = GetProductRepositoryMock();
            
            repositoryMock
                .Setup(r => r.GetById(It.IsAny<Guid>()))
                .Returns(Task.FromResult(new Product(VALID_GUID,VALID_DESCRIPTION,VALID_PRICE)));
            
            repositoryMock
                .Setup(r => r.CheckIfProductExistsByDescription(It.IsAny<String>()))
                .Returns(Task.FromResult(true));
            
            var useCase = GetUseCase(repositoryMock.Object);
            var requestObject = GetUpdateProductRequestObject(VALID_GUID, VALID_DESCRIPTION, VALID_PRICE);

            // Act
            var response = await useCase.Handle(requestObject);

            // Assert
            Assert.AreEqual((int)HttpStatusCode.BadRequest, response.StatusCode);
            CollectionAssert.Contains(response.ValidationNotifications, new ValidationNotification(Messages.ProductDescriptionExistError));
        }
        
        [Test]
        [TestCase(TestName = "Update product that don't exist", Category = "Product App", Description = "Update product", TestOf = typeof(UpdateProduct))]
        public async Task WhenGivenAnNotExistProduct_ShouldReturnNotFound()
        {
            // Arrange
            var repositoryMock = GetProductRepositoryMock();
            repositoryMock
                .Setup(r => r.CheckIfProductExistsByDescription(It.IsAny<String>()))
                .Returns(Task.FromResult(false));
            
            repositoryMock
                .Setup(r => r.GetById(It.IsAny<Guid>()))
                .Returns(Task.FromResult((Product) null));

            var useCase = GetUseCase(repositoryMock.Object);
            var requestObject = GetUpdateProductRequestObject(VALID_GUID, VALID_DESCRIPTION, VALID_PRICE);

            // Act
            var response = await useCase.Handle(requestObject);

            // Assert
            Assert.AreEqual((int)HttpStatusCode.NotFound, response.StatusCode);
            CollectionAssert.Contains(response.ValidationNotifications, new ValidationNotification(Messages.ProductIdError));
        }
        
        [Test]
        [TestCase(TestName = "Sucessfull update product", Category = "Product App", Description = "Update product", TestOf = typeof(UpdateProduct))]
        public async Task WhenGivenAnValidProduct_ShouldReturnOk()
        {
            // Arrange
            var repositoryMock = GetProductRepositoryMock();
            repositoryMock
                .Setup(r => r.CheckIfProductExistsByDescription(It.IsAny<String>()))
                .Returns(Task.FromResult(false));
            
            repositoryMock
                .Setup(r => r.GetById(It.IsAny<Guid>()))
                .Returns(Task.FromResult(new Product(VALID_GUID,VALID_DESCRIPTION,VALID_PRICE)));
            
            var uowMock = GetUowMock();
            uowMock.Setup(x => x.Commit()).Returns(Task.FromResult(true));
            
            var useCase = GetUseCase(repositoryMock.Object, uowMock.Object);
            var requestObject = GetUpdateProductRequestObject(VALID_GUID, VALID_DESCRIPTION, VALID_PRICE);

            // Act
            var response = await useCase.Handle(requestObject);

            // Assert
            Assert.AreEqual((int)HttpStatusCode.OK, response.StatusCode);
        }
    }
}