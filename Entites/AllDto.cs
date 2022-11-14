using Entites.StockFundamentals;
using Entites.StockProfile;

namespace Entites;

public class AllDto
{
    public string SymbolName { get; set; }
    public StockProfileItem StockProfile { get; set; }
    public StockFundamentalsItem StockFundamentals { get; set; }

}
