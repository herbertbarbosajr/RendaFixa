using Xunit;
using Moq;
using FixedIncome.Application.Interfaces;
using FixedIncome.Application.DTO_s;
using FixedIncome.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

namespace RendaFixa.TesteFuncional.ControllerFuncionalTest;
public class OrderControllerTests
{
    [Fact]
    public async Task Post_DeveRetornarSucesso_QuandoCompraForValida()
    {
        // Arrange
        var orderServiceMock = new Mock<IOrderService>();
        var request = new PurchaseRequest { ProductId = 1, Quantity = 10 };

        orderServiceMock.Setup(service => 
                                 service
                                 .PurchaseAsync(request))
                                 .ReturnsAsync(true);


        var controller = new OrderController(orderServiceMock.Object);

        // Act
        var result = await controller.Post(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = okResult.Value;
        Assert.Equal("Compra realizada com sucesso.", response.GetType().GetProperty("message")?.GetValue(response)?.ToString());

    }

    [Fact]
    public async Task Post_DeveRetornarBadRequest_QuandoCompraForInvalida()
    {
        // Arrange
        var orderServiceMock = new Mock<IOrderService>();
        var request = new PurchaseRequest { ProductId = 1, Quantity = 0 };

        orderServiceMock
            .Setup(service => service.PurchaseAsync(request))
            .ThrowsAsync(new InvalidOperationException("Quantidade inválida."));

        var controller = new OrderController(orderServiceMock.Object);

        // Act
        var result = await controller.Post(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var response = badRequestResult.Value;
        Assert.Equal("Quantidade inválida.", response.GetType().GetProperty("error")?.GetValue(response)?.ToString());

    }
}
