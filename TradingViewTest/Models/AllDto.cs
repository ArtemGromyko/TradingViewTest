using TradingViewTest.Models.StockFundamentals;
using TradingViewTest.Models.StockProfile;

namespace TradingViewTest.Models;

public class AllDto
{
    public string SymbolName { get; set; }
    public StockProfileItem StockProfile { get; set; }
    public StockFundamentalsItem StockFundamentals { get; set; }

}
