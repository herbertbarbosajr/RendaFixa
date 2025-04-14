﻿using Microsoft.AspNetCore.Mvc;
using FixedIncome.Application.Interfaces;

namespace FixedIncome.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var products = await _productService.GetOrderedProducts();
            return Ok(products);
        }
    }
}

