namespace Entites.StockProfile;

public class CEOCompensation
{
    public string Symbol { get; set; }
    public string Name { get; set; }
    public string CompanyName { get; set; }
    public string Location { get; set; }
    public long? Salary { get; set; }
    public long? Bonus { get; set; }
    public long? StockAwards { get; set; }
    public long? OptionAwards { get; set; }
    public long? NonEquityIncentives { get; set; }
    public long? PensionAndDeferred { get; set; }
    public long? OtherComp { get; set; }
    public long? Total { get; set; }
    public string Year { get; set; }
}
