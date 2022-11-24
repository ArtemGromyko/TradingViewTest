using Entites.RealTime.Book;

namespace TradingView.BLL.Abstractions.RealTime;

public interface IBookService
{
    Task<BookItem> GetBookAsync(string symbol);
}
