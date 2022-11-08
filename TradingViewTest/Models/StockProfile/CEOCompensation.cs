using Newtonsoft.Json;

namespace TradingViewTest.Models.StockProfile;

public class CEOCompensation : EntityBase
{
    public string? Symbol { get; set; }
    public string? Name { get; set; }
    public string? CompanyName { get; set; }
    public string? Location { get; set; }
    public double Salary { get; set; }
    public double StockAwards { get; set; }
    public double OptionAwards { get; set; }
    public double NonEquityIncentives { get; set; }
    public double PensionAndDeferred { get; set; }
    public double OtherComp { get; set; }
    public double Total { get; set; }
    public string? Year { get; set; }
}
