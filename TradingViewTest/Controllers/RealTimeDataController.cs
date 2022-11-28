using Entites.RealTime;
using Microsoft.AspNetCore.Mvc;
using TradingView.BLL.Abstractions.RealTime;
using TradingView.BLL.Contracts;

namespace TradingViewTest.Controllers;

[Route("api")]
[ApiController]
public class RealTimeDataController : ControllerBase
{
    private readonly IHistoricalPricesService _historicalPricesService;
    private readonly IQuotesService _quotesService;
    private readonly IIntradayPricesService _intradayPricesService;
    private readonly ILargestTradesService _largestTradesService;
    private readonly IOHLCService _ohlcService;
    private readonly IPreviousDayPriceService _previousDayPriceService;
    private readonly IPriceOnlyService _priceOnlyService;
    private readonly IVolumeByVenueService _volumeByVenueService;
    private readonly IBookService _bookService;
    private readonly IDelayedQuoteService _delayedQuoteService;
    private readonly IExchangeService _exchangeService;

    public RealTimeDataController(IHistoricalPricesService historicalPricesService, IQuotesService quotesService,
        IIntradayPricesService intradayPricesService, ILargestTradesService largestTradesService, IOHLCService ohlcService,
        IPreviousDayPriceService previousDayPriceService, IPriceOnlyService priceOnlyService, IVolumeByVenueService volumeByVenueService,
        IBookService bookService, IDelayedQuoteService delayedQuoteService, IExchangeService exchangeService)
    {
        _historicalPricesService = historicalPricesService;
        _quotesService = quotesService;
        _intradayPricesService = intradayPricesService;
        _largestTradesService = largestTradesService;
        _ohlcService = ohlcService;
        _previousDayPriceService = previousDayPriceService;
        _priceOnlyService = priceOnlyService;
        _volumeByVenueService = volumeByVenueService;
        _bookService = bookService;
        _delayedQuoteService = delayedQuoteService;
        _exchangeService = exchangeService;
    }

    [HttpGet("{symbol}/historical-prices")]
    public async Task<IActionResult> GetHistoricalPricesListAsync(string symbol)
    {
        var historicalPrices = await _historicalPricesService.GetHistoricalPricesListAsync(symbol);

        return Ok(historicalPrices);
    }

    [HttpGet("{symbol}/quote")]
    public async Task<IActionResult> GetQuoteync(string symbol)
    {
        var quote = await _quotesService.GetQuoteAsync(symbol);

        return Ok(quote);
    }

    [HttpGet("{symbol}/intraday-prices")]
    public async Task<IActionResult> GetIntradayPricesAsync(string symbol)
    {
        var intradayPrices = await _intradayPricesService.GetIntradayPricesListAsync(symbol);

        return Ok(intradayPrices);
    }

    [HttpGet("{symbol}/largest-trades")]
    public async Task<IActionResult> GetLargestTradesAsync(string symbol)
    {
        var largestTrades = await _largestTradesService.GetLargestTradesListAsync(symbol);

        return Ok(largestTrades);
    }

    [HttpGet("{symbol}/ohlc")]
    public async Task<IActionResult> GetOHLCAsync(string symbol)
    {
        var ohlc = await _ohlcService.GetOHLCAsync(symbol);

        return Ok(ohlc);
    }

    [HttpGet("{symbol}/previous-day-price")]
    public async Task<IActionResult> GetPreviousDayPriceAsync(string symbol)
    {
        var previousDayPrice = await _previousDayPriceService.GetPreviousDayPriceAsync(symbol);

        return Ok(previousDayPrice);
    }

    [HttpGet("{symbol}/price")]
    public async Task<IActionResult> GetPriceOnlyAsync(string symbol)
    {
        var priceOnly = await _priceOnlyService.GetPriceOnlyAsync(symbol);

        return Ok(priceOnly);
    }

    [HttpGet("{symbol}/volume-by-venue")]
    public async Task<IActionResult> GetVolumeByVenueAsync(string symbol)
    {
        var volumesByVenue = await _volumeByVenueService.GetVolumesByVenueAsync(symbol);

        return Ok(volumesByVenue);
    }

    [HttpGet("{symbol}/book")]
    public async Task<IActionResult> GetBookAsync(string symbol)
    {
        var book = await _bookService.GetBookAsync(symbol);

        return Ok(book);
    }

    [HttpGet("{symbol}/delayed-quote")]
    public async Task<IActionResult> GetDelayedQuoteAsync(string symbol)
    {
        var delayedQuote = await _delayedQuoteService.GetDelayedQuoteAsync(symbol);

        return Ok(delayedQuote);
    }

    [HttpGet("exchanges")]
    public async Task<IActionResult> GetAllExchanges()
    {
        var exchanges = await _exchangeService.GetExchangesAsync();

        return Ok(exchanges);
    }
}
