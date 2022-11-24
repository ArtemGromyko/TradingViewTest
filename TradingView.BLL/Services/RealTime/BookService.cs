using Entites.Exceptions;
using Entites.RealTime.Book;
using Microsoft.Extensions.Configuration;
using TradingView.BLL.Abstractions.RealTime;
using TradingView.DAL.Abstractions.Repositories.RealTime;

namespace TradingView.BLL.Services.RealTime;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;

    private readonly IConfiguration _configuration;

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly HttpClient _httpClient;

    public BookService(IBookRepository bookRepository, IConfiguration configuration,
        IHttpClientFactory httpClientFactory)
    {
        _bookRepository = bookRepository;
        _configuration = configuration;

        _httpClientFactory = httpClientFactory;
        _httpClient = _httpClientFactory.CreateClient(_configuration["HttpClientName"]);
    }

    public async Task<BookItem> GetBookAsync(string symbol)
    {
        var book = await _bookRepository.GetAsync((b) => b.Symbol!.ToUpper() == symbol.ToUpper());
        if (book is null)
        {
            var url = $"{_configuration["IEXCloudUrls:version"]}" +
                $"{string.Format(_configuration["IEXCloudUrls:bookUrl"], symbol)}" +
                $"?token={Environment.GetEnvironmentVariable("PUBLISHABLE_TOKEN")}";

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                throw new IexCloudException(response);
            }

            var res = await response.Content.ReadAsAsync<BookItem>();

            var newBook = new Book { BookItem = res, Symbol = symbol };
            await _bookRepository.AddAsync(newBook);
            book = newBook;
        }

        return book.BookItem!;
    }
}
