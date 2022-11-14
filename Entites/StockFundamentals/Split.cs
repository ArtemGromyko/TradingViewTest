namespace Entites.StockFundamentals;

public class Split
{
    public DateTime? DeclaredDate { get; set; }
    public string Description { get; set; }
    public DateTime? ExDate { get; set; }
    public int? FromFactor { get; set; }
    public double? Ratio { get; set; }
    public long? Refid { get; set; }
    public string Symbol { get; set; }
    public int? ToFactor { get; set; }
    public string Id { get; set; }
    public string Key { get; set; }
    public string Subkey { get; set; }
    public long? Updated { get; set; }
}