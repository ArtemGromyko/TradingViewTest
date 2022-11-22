namespace Entites.StockFundamentals.FinancialsAsReported
{
    public class FinancialsAsReported : EntityBase
    {
        public string Symbol { get; set; }
        public List<FinancialsAsReportedItem> FinancialsAsReportedItems { get; set; }
    }
}
