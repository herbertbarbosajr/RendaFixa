using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using System.Collections.Generic;
using System.Net.Http.Json;
using FixedIncome.Application.DTO_s;
using System;

public class ProductControllerTests
{
    private readonly HttpClient _client;

    public ProductControllerTests()
    {
        var handler = new MockHttpMessageHandler();
        _client = new HttpClient(handler)
        {
            BaseAddress = new Uri("http://localhost")
        };
    }

    [Fact]
    public async Task GetProducts_DeveRetornarListaDeProducts()
    {
        // Act
        var response = await _client.GetAsync("/api/Product/listar");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var Products = await response.Content.ReadFromJsonAsync<List<ProductDto>>();
        Assert.NotNull(Products);
        Assert.True(Products.Count > 0, "A lista de Products deve conter itens.");
    }

    private class MockHttpMessageHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.RequestUri.AbsolutePath == "/api/Product/listar")
            {
                var Products = new List<ProductDto>
                {
                    new ProductDto { Id = 1, Nome = "Product 1" },
                    new ProductDto { Id = 2, Nome = "Product 2" }
                };

                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = JsonContent.Create(Products)
                };

                return Task.FromResult(response);
            }

            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound));
        }
    }
}
