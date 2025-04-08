using Microsoft.AspNetCore.Mvc;
using RendaFixa.Application.Interfaces;

namespace RendaFixa.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private readonly IProdutoService _produtoService;

        public ProdutoController(IProdutoService produtoService)
        {
            _produtoService = produtoService;
        }

        [HttpGet("listar")]
        public async Task<IActionResult> Get() => Ok(await _produtoService.ObterProdutosOrdenados());

        [HttpGet("saldo")]
        public async Task<IActionResult> GetSaldo(int id) => Ok(new { Saldo = await _produtoService.ObterSaldo(id) });

        [HttpPost("comprar")]
        public async Task<IActionResult> Comprar([FromBody] CompraRequest request)
        {
            await _produtoService.RealizarCompra(request.ProdutoId, request.Quantidade);
            return Ok(new { Mensagem = "Compra realizada com sucesso." });
        }

        public class CompraRequest
        {
            public int ProdutoId { get; set; }
            public int Quantidade { get; set; }
        }
    }
}

