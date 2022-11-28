using Entites.Exceptions;
using Entites.StockFundamentals;
using TradingView.BLL.Abstractions;
using TradingView.DAL.Abstractions.ApiServices;
using TradingView.DAL.Abstractions.Repositories;

namespace TradingView.BLL.Services;

public class StockFundamentalsService : IStockFundamentalsService
{
    private readonly IStockFundamentalsRepository _stockFundamentalsRepository;
    private readonly IFinancialsAsReportedRepository _financialsAsReportedRepository;

    public StockFundamentalsService(IStockFundamentalsRepository stockFundamentalsRepository, IFinancialsAsReportedRepository financialsAsReportedRepository)
    {
        _stockFundamentalsRepository = stockFundamentalsRepository;
        _financialsAsReportedRepository = financialsAsReportedRepository;
    }

    public async Task<StockFundamentals> GetStockFundamentalsAsync(string symbol)
    {
        var stockFundamentals = await _stockFundamentalsRepository.GetAsync(
            (s) => s.SymbolName.ToUpper() == symbol.ToUpper());

        if (stockFundamentals is null)
        {
            throw new NotFoundException("Symbol not found");
        }

        var financialsAsReported = await _financialsAsReportedRepository.GetAsync(
            (s) => s.Symbol.ToUpper() == symbol.ToUpper());

        stockFundamentals.StockFundamentalsItem.FinancialsAsReported = financialsAsReported?.FinancialsAsReportedItems;

        return stockFundamentals;
    }
}
