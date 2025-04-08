using Microsoft.AspNetCore.Mvc;
using Moq;
using FixedIncome.API.Controllers;
using FixedIncome.Application.DTO_s;
using FixedIncome.Application.Interfaces;

namespace FixedIncome.Tests
{
    public class ProductControllerTests
    {
        private readonly ProductController _controller;
        private readonly Mock<IProductService> _mockProductService;

        public ProductControllerTests()
        {
            _mockProductService = new Mock<IProductService>();
            _controller = new ProductController(_mockProductService.Object);
        }

        [Fact]
        public async Task Get_ReturnsOkResult_WithListOfProducts()
        {
            // Arrange
            var products = new List<ProductDto>
            {
                new ProductDto { Id = 1, BondAsset = "Product 1", Index = "IPCA", UnitPrice = 100.0m, Tax = 5.0m, Stock = 50 },
                new ProductDto { Id = 2, BondAsset = "Product 2", Index = "CDI", UnitPrice = 200.0m, Tax = 6.0m, Stock = 30 }
            };
            _mockProductService.Setup(service => service.GetOrderedProducts()).ReturnsAsync(products);

            // Act
            var result = await _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<ProductDto>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetBalance_ReturnsOkResult_WithBalance()
        {
            // Arrange
            var Balance = 1000.0m;
            _mockProductService.Setup(service => service.GetBalance(It.IsAny<int>())).ReturnsAsync(Balance);

            // Act
            var result = await _controller.GetBalance(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<decimal>(okResult.Value.GetType().GetProperty("Balance").GetValue(okResult.Value));
            Assert.Equal(Balance, returnValue);
        }

        [Fact]
        public async Task Comprar_ReturnsOkResult_WithSuccessMessage()
        {
            // Arrange
            var request = new ProductController.CompraRequest { ProductId = 1, Quantidade = 10 };
            _mockProductService.Setup(service => service.MakePurchase(request.ProductId, request.Quantidade)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Comprar(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<string>(okResult.Value.GetType().GetProperty("Mensagem").GetValue(okResult.Value));
            Assert.Equal("Success purchase realized.", returnValue);
        }
    }
}
