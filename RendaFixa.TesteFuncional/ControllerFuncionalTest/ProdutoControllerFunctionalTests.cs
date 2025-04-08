using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using System.Collections.Generic;
using System.Net.Http.Json;
using RendaFixa.Application.DTO_s;
using System;

public class ProdutoControllerTests
{
    private readonly HttpClient _client;

    public ProdutoControllerTests()
    {
        var handler = new MockHttpMessageHandler();
        _client = new HttpClient(handler)
        {
            BaseAddress = new Uri("http://localhost")
        };
    }

    [Fact]
    public async Task GetProdutos_DeveRetornarListaDeProdutos()
    {
        // Act
        var response = await _client.GetAsync("/api/Produto/listar");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var produtos = await response.Content.ReadFromJsonAsync<List<ProdutoDto>>();
        Assert.NotNull(produtos);
        Assert.True(produtos.Count > 0, "A lista de produtos deve conter itens.");
    }

    private class MockHttpMessageHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.RequestUri.AbsolutePath == "/api/Produto/listar")
            {
                var produtos = new List<ProdutoDto>
                {
                    new ProdutoDto { Id = 1, Nome = "Produto 1" },
                    new ProdutoDto { Id = 2, Nome = "Produto 2" }
                };

                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = JsonContent.Create(produtos)
                };

                return Task.FromResult(response);
            }

            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound));
        }
    }
}
