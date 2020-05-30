using System;
using System.Net;
using System.Threading.Tasks;
using Domain.Domains;
using Infra.Data.RepositoryInterfaces;
using MongoCoreDbRepository.Interfaces;
using Moq;
using NUnit.Framework;
using ProductApp.Service;
using ProductApp.UseCases.AddProduct;
using ProductApp.UseCases.DeleteProduct;
using ProductApp.UseCases.UpdateProduct;
using ProductApp.UseCasesInterfaces.DeleteProduct;
using Shared;

namespace Application.Tests.Products.UseCases
{
    public class DeleteProductsTests
    {
        private Guid INVALID_GUID = Guid.Empty;
        private Guid VALID_GUID = Guid.NewGuid();
        private string VALID_DESCRIPTION = "Café Pilão 500g";
        private decimal VALID_PRICE = Convert.ToDecimal(10.50);
        private IProductService GetUseCase(IProductRepository productRepository = null, IUnitOfWork unitOfWork = null)
        {
            return new ProductService(null, null, null, new DeleteProduct(productRepository,unitOfWork));
        } 

        private IDeleteProductRequestObject GetDeleteProductRequestObject(Guid id)
        {
            return new DeleteProductRequestObject(id);
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
        [TestCase(TestName = "Delete product with invalid id", Category = "Product App", Description = "Delete product", TestOf = typeof(DeleteProduct))]
        public async Task WhenGivenAnInvalidId_ShouldReturnBadRequest()
        {
            // Arrange
            var useCase = GetUseCase();
            var requestObject = GetDeleteProductRequestObject(INVALID_GUID);

            // Act
            var response = await useCase.Handle(requestObject);

            // Assert
            Assert.AreEqual((int)HttpStatusCode.BadRequest, response.StatusCode);
            CollectionAssert.Contains(response.ValidationNotifications, new ValidationNotification(Messages.ProductIdError));
        }
        
        [Test]
        [TestCase(TestName = "Delete product that don't exist", Category = "Product App", Description = "Delete product", TestOf = typeof(DeleteProduct))]
        public async Task WhenGivenAnNotExistProduct_ShouldReturnNotFound()
        {
            // Arrange
            var repositoryMock = GetProductRepositoryMock();
            repositoryMock
                .Setup(r => r.GetById(It.IsAny<Guid>()))
                .Returns(Task.FromResult((Product)null));
            
            var useCase = GetUseCase(repositoryMock.Object);
            var requestObject = GetDeleteProductRequestObject(VALID_GUID);

            // Act
            var response = await useCase.Handle(requestObject);

            // Assert
            Assert.AreEqual((int)HttpStatusCode.NotFound, response.StatusCode);
            CollectionAssert.Contains(response.ValidationNotifications, new ValidationNotification(Messages.ProductIdError));
        }
        
        [Test]
        [TestCase(TestName = "Sucessfull delete product", Category = "Product App", Description = "Delete product", TestOf = typeof(DeleteProduct))]
        public async Task WhenGivenAnValidProductId_ShouldReturnOk()
        {
            // Arrange
            var repositoryMock = GetProductRepositoryMock();
            
            repositoryMock
                .Setup(r => r.GetById(It.IsAny<Guid>()))
                .Returns(Task.FromResult(new Product(VALID_GUID,VALID_DESCRIPTION,VALID_PRICE)));
            
            var uowMock = GetUowMock();
            uowMock.Setup(x => x.Commit()).Returns(Task.FromResult(true));
            
            var useCase = GetUseCase(repositoryMock.Object, uowMock.Object);
            var requestObject = GetDeleteProductRequestObject(VALID_GUID);

            // Act
            var response = await useCase.Handle(requestObject);

            // Assert
            Assert.AreEqual((int)HttpStatusCode.OK, response.StatusCode);
        }
    }
}