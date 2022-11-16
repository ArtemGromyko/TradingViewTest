using Microsoft.AspNetCore.Mvc;
using TradingView.BLL.Abstractions;
using TradingView.BLL.Services;

namespace TradingViewTest.Controllers
{
    [Route("api/stock-data")]
    [ApiController]
    public class StockDataController : ControllerBase
    {
        private readonly GetAllService _service;

        private readonly ISymbolService _symbolService;
        private readonly IStockProfileService _stockProfileService;
        private readonly IStockFundamentalsService _stockFundamentalsProfile;

        public StockDataController(GetAllService service, ISymbolService symbolService,
            IStockFundamentalsService stockFundamentalsProfile, IStockProfileService stockProfileService)
        {
            _service = service;
            _symbolService = symbolService;
            _stockFundamentalsProfile = stockFundamentalsProfile;
            _stockProfileService = stockProfileService;
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
            var symbols = await _symbolService.GetSymbolsAsync();

            return Ok(symbols);
        }

        [HttpGet("{symbol}/stock-profile")]
        public async Task<IActionResult> GetStockProfileAsync(string symbol)
        {
            var stockProfile = await _stockProfileService.GetStockProfileAsync(symbol);

            return Ok(stockProfile);
        }

        [HttpGet("{symbol}/stock-fundamentals")]
        public async Task<IActionResult> GetStockFundamentalsAsync(string symbol)
        {
            var stockFundamentals = await _stockFundamentalsProfile.GetStockFundamentalsAsync(symbol);

            return Ok(stockFundamentals);
        }
    }
}
