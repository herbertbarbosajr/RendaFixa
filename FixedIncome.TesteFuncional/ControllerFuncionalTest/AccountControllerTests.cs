using Xunit;
using Moq;
using FixedIncome.Domain.Interfaces;
using FixedIncome.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using FixedIncome.Domain.Entities;

namespace FixedIncome.TesteFuncional.ControllerFuncionalTest;
public class AccountControllerTests
{
    [Fact]
    public async Task Get_DeveRetornarConta_QuandoIdExistir()
    {
        // Arrange
        var accountMock = new CustomerAccount(1, "Cliente123", 1000.50m);
        var repoMock = new Mock<IAccountRepository>();
        repoMock
            .Setup(repo => repo.GetByIdAsync(1))
            .ReturnsAsync(accountMock);

        var controller = new AccountController(repoMock.Object);

        // Act
        var result = await controller.Get(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var account = Assert.IsType<CustomerAccount>(okResult.Value);
        Assert.NotNull(account);
        Assert.Equal(1, account.Account);
        Assert.Equal("Cliente123", account.ClientId);
        Assert.Equal(1000.50m, account.Balance);
    }

    [Fact]
    public async Task Get_DeveRetornarNotFound_QuandoIdNaoExistir()
    {
        // Arrange
        var repoMock = new Mock<IAccountRepository>();
        repoMock
            .Setup(repo => repo.GetByIdAsync(99))
            .ReturnsAsync((CustomerAccount?)null);

        var controller = new AccountController(repoMock.Object);

        // Act
        var result = await controller.Get(99);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
