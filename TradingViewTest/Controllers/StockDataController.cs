using Microsoft.AspNetCore.Mvc;
using TradingView.BLL.Abstractions;
using TradingView.BLL.Services;
using TradingView.DAL.Abstractions.ApiServices;
using TradingView.DAL.Abstractions.Repositories;

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

        private readonly ISymbolRepository _symbolRepository;
        private readonly IStockProfileRepository _stockProfileRepository;
        private readonly IStockFundamentalsRepository _stockFundamentalsRepository;
        private readonly IFinancialsAsReportedRepository _financialsAsReportedRepository;

        private readonly IFinancialsAsReportedApiService _financialsAsReportedApiService;

        public StockDataController(GetAllService service, ISymbolService symbolService,
            IStockFundamentalsService stockFundamentalsProfile, IStockProfileService stockProfileService, ISymbolRepository symbolRepository, IStockProfileRepository stockProfileRepository, IStockFundamentalsRepository stockFundamentalsRepository, IFinancialsAsReportedRepository financialsAsReportedRepository, IFinancialsAsReportedApiService financialsAsReportedApiService)
        {
            _service = service;
            _symbolService = symbolService;
            _stockFundamentalsProfile = stockFundamentalsProfile;
            _stockProfileService = stockProfileService;
            _symbolRepository = symbolRepository;
            _stockProfileRepository = stockProfileRepository;
            _stockFundamentalsRepository = stockFundamentalsRepository;
            _financialsAsReportedRepository = financialsAsReportedRepository;
            _financialsAsReportedApiService = financialsAsReportedApiService;
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

        [HttpGet("{symbol}/financials")]
        public async Task<IActionResult> GetFinancialsAsync(string symbol)
        {
            var financials = await _financialsAsReportedApiService.FetchFinancialsAsReportedAsync(symbol);

            return Ok(financials);
        }

        [HttpGet("delete")]
        public async Task<IActionResult> DeleteAsync()
        {
            await _stockProfileRepository.DeleteAllAsync();
            await _stockFundamentalsRepository.DeleteAllAsync();
            await _symbolRepository.DeleteAllAsync();
            await _financialsAsReportedRepository.DeleteAllAsync();

            return Ok();
        }
    }
}
