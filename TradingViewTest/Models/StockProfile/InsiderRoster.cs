namespace TradingViewTest.Models.StockProfile;

public class InsiderRoster : EntityBase
{
    public string? EntityName { get; set; }
    public double? Position { get; set; }
    public long ReportDate { get; set; }
}
