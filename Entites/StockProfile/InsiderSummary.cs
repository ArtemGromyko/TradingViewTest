using MongoDB.Bson.Serialization.Attributes;

namespace Entites.StockProfile;

public class InsiderSummary
{
    public string FullName { get; set; }
    public int? NetTransacted { get; set; }
    public string ReportedTitle { get; set; }
    public string Symbol { get; set; }
    public int? TotalBought { get; set; }
    public int? TotalSold { get; set; }
    [BsonElement("InsiderSummaryId")]
    public string Id { get; set; }
    public string Key { get; set; }
    public string Subkey { get; set; }
    public long? Updated { get; set; }
}
