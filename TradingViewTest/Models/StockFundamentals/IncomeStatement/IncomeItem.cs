namespace TradingViewTest.Models.StockFundamentals.IncomeStatement;

public class IncomeItem
{
    public DateTime? ReportDate { get; set; }
    public string FilingType { get; set; }
    public DateTime? FiscalDate { get; set; }
    public int? FiscalQuarter { get; set; }
    public int? FiscalYear { get; set; }
    public string Currency { get; set; }
    public double? TotalRevenue { get; set; }
    public double? CostOfRevenue { get; set; }
    public double? GrossProfit { get; set; }
    public double? ResearchAndDevelopment { get; set; }
    public double? SellingGeneralAndAdmin { get; set; }
    public double? OperatingExpense { get; set; }
    public double? OperatingIncome { get; set; }
    public double? OtherIncomeExpenseNet { get; set; }
    public double? Ebit { get; set; }
    public double? InterestIncome { get; set; }
    public double? PretaxIncome { get; set; }
    public double? IncomeTax { get; set; }
    public double? MinorityInterest { get; set; }
    public double? NetIncome { get; set; }
    public double? NetIncomeBasic { get; set; }
}
