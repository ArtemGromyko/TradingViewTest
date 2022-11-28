using Entites;
using Microsoft.AspNetCore.Mvc;
using TradingView.BLL.Abstractions;
 
namespace TradingViewTest.Controllers
{
    [Route("api/stock-data")]
    [ApiController]
    public class StockDataController : ControllerBase
    {
        private readonly ISymbolService _symbolService;
        private readonly IStockProfileService _stockProfileService;
        private readonly IStockFundamentalsService _stockFundamentalsService;

        public StockDataController(ISymbolService symbolService, IStockProfileService stockProfileService, IStockFundamentalsService stockFundamentalsService)
        {
            _symbolService = symbolService;
            _stockProfileService = stockProfileService;
            _stockFundamentalsService = stockFundamentalsService;
        }

        [HttpGet("symbols")]
        public async Task<IActionResult> GetSymbolsAsync()
        {
            var symbols = await _symbolService.GetSymbolsAsync();

            return Ok(symbols);
        }

        [HttpGet("{symbol}")]
        public async Task<IActionResult> GetStockDataAsync(string symbol)
        {
            var stockProfile = await _stockProfileService.GetStockProfileAsync(symbol);
            var stockFundamentals = await _stockFundamentalsService.GetStockFundamentalsAsync(symbol);

            var stockDataDto = new StockDataDto { SymbolName = symbol, StockProfile = stockProfile.StockProfileItem, StockFundamentals = stockFundamentals.StockFundamentalsItem };

            return Ok(stockDataDto); 
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
            var stockFundamentals = await _stockFundamentalsService.GetStockFundamentalsAsync(symbol);

            return Ok(stockFundamentals);
        }
    }
}
