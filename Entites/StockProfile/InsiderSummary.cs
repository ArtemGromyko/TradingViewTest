﻿using MongoDB.Bson.Serialization.Attributes;

namespace Entites.StockProfile;

public class InsiderSummary
{
    public string Date { get; set; }
    public string FullName { get; set; }
    public string IssuerCik { get; set; }
    public double? NetTransacted { get; set; }
    public string ReportedTitle { get; set; }
    public string Symbol { get; set; }
    public double? TotalBought { get; set; }
    public double? TotalSold { get; set; }
    [BsonElement("InsiderSummaryId")]
    public string Id { get; set; }
    public string Key { get; set; }
    public string Subkey { get; set; }
    public double? Updated { get; set; }
}
