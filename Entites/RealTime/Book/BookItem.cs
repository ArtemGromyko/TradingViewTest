namespace Entites.RealTime.Book;

public class BookItem
{
    public Quote Quote { get; set; }
    public List<Bid> Bids { get; set; }
    public List<Ask> Asks { get; set; }
    public SystemEventInfo SystemEvent { get; set; }
}
