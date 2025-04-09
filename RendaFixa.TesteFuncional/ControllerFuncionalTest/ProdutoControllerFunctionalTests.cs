using System.Threading.Tasks;
using Xunit;
using System.Collections.Generic;
using FixedIncome.Application.DTO_s;
using FixedIncome.Application.Interfaces;
using Moq;
using Microsoft.AspNetCore.Mvc;
using FixedIncome.API.Controllers;

namespace FixedIncome.TesteFuncional.ControllerFuncionalTest;

public class ProductControllerFunctionalTests 
{

    [Fact]
    public async Task Get_DeveRetornarListaDeProdutos()
    {
        // Arrange
        var productServiceMock = new Mock<IProductService>();
        var produtosMock = new List<ProductDto>
        {
            new ProductDto { Id = 1, BondAsset = "CDB", Index = "IPCA", Tax = 5.0, IssuerName = "Banco Teste", UnitPrice = 1000, Stock = 100 },
            new ProductDto { Id = 2, BondAsset = "LCI", Index = "Pre", Tax = 12.0, IssuerName = "Banco Teste 2", UnitPrice = 2000, Stock = 20 }
        };

        productServiceMock
            .Setup(service => service.GetOrderedProducts())
            .ReturnsAsync(produtosMock);

        var controller = new ProductController(productServiceMock.Object);

        // Act
        var result = await controller.Get();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var produtos = Assert.IsType<List<ProductDto>>(okResult.Value);
        Assert.NotNull(produtos);
        Assert.Equal(2, produtos.Count);
        Assert.Equal("CDB", produtos[0].BondAsset);
        Assert.Equal("LCI", produtos[1].BondAsset);
    }
}
