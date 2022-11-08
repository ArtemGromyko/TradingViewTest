using Microsoft.AspNetCore.Mvc;
using TradingViewTest.Services;

namespace TradingViewTest.Controllers
{
    [Route("api/get-all")]
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
            throw new NotImplementedException();
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
