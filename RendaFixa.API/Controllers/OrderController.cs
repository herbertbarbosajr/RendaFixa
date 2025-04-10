using FixedIncome.Application.DTO_s;
using FixedIncome.Application.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace FixedIncome.API.Controllers
{
    [ApiController]
    [Route("order")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PurchaseRequest request)
        {
            try
            {
                await _orderService.PurchaseAsync(request);
                return Ok(new { message = "Compra realizada com sucesso." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
