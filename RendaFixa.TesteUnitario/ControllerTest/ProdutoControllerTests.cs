using Microsoft.AspNetCore.Mvc;
using Moq;
using RendaFixa.API.Controllers;
using RendaFixa.Application.DTO_s;
using RendaFixa.Application.Interfaces;

namespace RendaFixa.Tests
{
    public class ProdutoControllerTests
    {
        private readonly ProdutoController _controller;
        private readonly Mock<IProdutoService> _mockProdutoService;

        public ProdutoControllerTests()
        {
            _mockProdutoService = new Mock<IProdutoService>();
            _controller = new ProdutoController(_mockProdutoService.Object);
        }

        [Fact]
        public async Task Get_ReturnsOkResult_WithListOfProdutos()
        {
            // Arrange
            var produtos = new List<ProdutoDto>
            {
                new ProdutoDto { Id = 1, Nome = "Produto 1", Indexador = "IPCA", PrecoUnitario = 100.0m, Taxa = 5.0m, Estoque = 50 },
                new ProdutoDto { Id = 2, Nome = "Produto 2", Indexador = "CDI", PrecoUnitario = 200.0m, Taxa = 6.0m, Estoque = 30 }
            };
            _mockProdutoService.Setup(service => service.ObterProdutosOrdenados()).ReturnsAsync(produtos);

            // Act
            var result = await _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<ProdutoDto>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetSaldo_ReturnsOkResult_WithSaldo()
        {
            // Arrange
            var saldo = 1000.0m;
            _mockProdutoService.Setup(service => service.ObterSaldo(It.IsAny<int>())).ReturnsAsync(saldo);

            // Act
            var result = await _controller.GetSaldo(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<decimal>(okResult.Value.GetType().GetProperty("Saldo").GetValue(okResult.Value));
            Assert.Equal(saldo, returnValue);
        }

        [Fact]
        public async Task Comprar_ReturnsOkResult_WithSuccessMessage()
        {
            // Arrange
            var request = new ProdutoController.CompraRequest { ProdutoId = 1, Quantidade = 10 };
            _mockProdutoService.Setup(service => service.RealizarCompra(request.ProdutoId, request.Quantidade)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Comprar(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<string>(okResult.Value.GetType().GetProperty("Mensagem").GetValue(okResult.Value));
            Assert.Equal("Compra realizada com sucesso.", returnValue);
        }
    }
}
