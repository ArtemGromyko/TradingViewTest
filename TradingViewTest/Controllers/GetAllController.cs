﻿using Microsoft.AspNetCore.Mvc;
using TradingView.BLL.Services;

namespace TradingViewTest.Controllers
{
    [Route("api/stock-data")]
    [ApiController]
    public class GetAllController : ControllerBase
    {
        private readonly GetAllService _service;

        public GetAllController(GetAllService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var dtos = await _service.GetAllAsync();

            return Ok(dtos);
        }

        [HttpGet("symbols")]
        public async Task<IActionResult> GetSymbolsAsync()
        {
            var symbols = await _service.GetSymbolsAsync();

            return Ok(symbols);
        }

        [HttpGet("{symbol}/stock-profile")]
        public async Task<IActionResult> GetStockProfileAsync(string symbol)
        {
            var stockProfile = await _service.GetStockProfileAsync(symbol);

            return Ok(stockProfile);
        }

        [HttpGet("{symbol}/stock-fundamentals")]
        public async Task<IActionResult> GetStockFundamentalsAsync(string symbol)
        {
            var stockFundamentals = await _service.GetStockFundamentalsAsync(symbol);

            return Ok(stockFundamentals);
        }
    }
}
