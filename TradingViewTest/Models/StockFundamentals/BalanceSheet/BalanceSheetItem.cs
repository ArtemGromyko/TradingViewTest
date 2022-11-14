namespace TradingViewTest.Models.StockFundamentals.BalanceSheet;

public class BalanceSheetItem
{
    public string ReportDate { get; set; }
    public string FilingType { get; set; }
    public string FiscalDate { get; set; }
    public int? FiscalQuarter { get; set; }
    public int? FiscalYear { get; set; }
    public string Currency { get; set; }
    public double? CurrentCash { get; set; }
    public double? ShortTermInvestments { get; set; }
    public double? Receivables { get; set; }
    public double? Inventory { get; set; }
    public double? OtherCurrentAssets { get; set; }
    public double? CurrentAssets { get; set; }
    public double? LongTermInvestments { get; set; }
    public double? PropertyPlantEquipment { get; set; }
    public double? Goodwill { get; set; }
    public double? IntangibleAssets { get; set; }
    public double? OtherAssets { get; set; }
    public double? TotalAssets { get; set; }
    public double? AccountsPayable { get; set; }
    public double? CurrentLongTermDebt { get; set; }
    public double? OtherCurrentLiabilities { get; set; }
    public double? TotalCurrentLiabilities { get; set; }
    public double? LongTermDebt { get; set; }
    public double? OtherLiabilities { get; set; }
    public double? MinorityInterest { get; set; }
    public double? TotalLiabilities { get; set; }
    public double? CommonStock { get; set; }
    public double? RetainedEarnings { get; set; }
    public double? TreasuryStock { get; set; }
    public double? CapitalSurplus { get; set; }
    public double? ShareholderEquity { get; set; }
    public double? NetTangibleAssets { get; set; }
    public string Id { get; set; }
    public string Key { get; set; }
    public string Subkey { get; set; }
    public long? Date { get; set; }
    public long? Updated { get; set; }
}
