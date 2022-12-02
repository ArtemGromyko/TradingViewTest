using Entites;
using TradingView.BLL.Abstractions;
using TradingView.BLL.Abstractions.RealTime;
using TradingView.BLL.Contracts;
using TradingView.BLL.Services;
using TradingView.BLL.Services.RealTime;
using TradingView.DAL.Abstractions.ApiServices;
using TradingView.DAL.Abstractions.Repositories;
using TradingView.DAL.Abstractions.Repositories.RealTime;
using TradingView.DAL.ApiServices;
using TradingView.DAL.Quartz;
using TradingView.DAL.Quartz.Jobs;
using TradingView.DAL.Quartz.Jobs.RealTime;
using TradingView.DAL.Quartz.Schedulers;
using TradingView.DAL.Quartz.Schedulers.RealTime;
using TradingView.DAL.Repositories;
using TradingView.DAL.Repositories.RealTime;
using TradingView.DAL.Settings;

namespace TradingViewTest.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureCors(this IServiceCollection services) =>
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });

    public static void ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<ISymbolService, SymbolService>();
        services.AddScoped<IStockProfileService, StockProfileService>();
        services.AddScoped<IStockFundamentalsService, StockFundamentalsService>();
    }

    public static void ConfigureRepositories(this IServiceCollection services)
    {
        services.AddTransient<ISymbolRepository, SymbolRepository>();
        services.AddTransient<IStockProfileRepository, StockProfileRepository>();
        services.AddTransient<IStockFundamentalsRepository, StockFundamentalsRepository>();
        services.AddTransient<IFinancialsAsReportedRepository, FinancialsAsReportedRepository>();

        services.AddScoped<IHistoricalPricesRepository, HistoricalPricesRepository>();
        services.AddScoped<IQuotesRepository, QuotesRepository>();
        services.AddScoped<IIntradayPricesRepository, IntradayPricesRepository>();
        services.AddScoped<ILargestTradesRepository, LargestTradesRepository>();
        services.AddScoped<IOHLCRepository, OHLCRepository>();
        services.AddScoped<IPreviousDayPriceRepository, PreviousDayPriceRepository>();
        services.AddScoped<IVolumeByVenueRepository, VolumeByVenueRepository>();
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IDelayedQuoteRepository, DelayedQuoteRepository>();
        services.AddScoped<IPriceOnlyRepository, PriceOnlyRepository>();
        services.AddScoped<IDividendsRepository, DividendsRepository>();
        services.AddScoped<IExchangeRepository, ExchangeRepository>();
    }

    public static void ConfigureHttpClient(this IServiceCollection services, IConfiguration configuration) =>
        services.AddHttpClient(configuration["HttpClientName"], client =>
        {
            client.BaseAddress = new Uri(configuration["IEXCloudUrls:baseUrl"]);
        });

    public static void ConfigureMongoDBConnection(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DatabaseSettings>(configuration
            .GetSection("DatabaseSettings"));
    }

    public static void ConfigureJobs(this IServiceCollection services, IConfiguration configuration) =>
        services.Configure<JobConfiguration>(configuration
            .GetSection("JobConfiguration"));
        

    public static void ConfigureApiServices(this IServiceCollection services)
    {
        services.AddTransient<ISymbolApiService, SymbolApiService>();
        services.AddTransient<IStockProfileApiService, StockProfileApiService>();
        services.AddTransient<IStockFundamentalsApiService, StockFundamentalsApiService>();
        services.AddTransient<IFinancialsAsReportedApiService, FinancialsAsReportedApiService>();

        services.AddScoped<IHistoricalPricesService, HistoricalPricesService>();
        services.AddScoped<IQuotesService, QuotesService>();
        services.AddScoped<IIntradayPricesService, IntradayPricesService>();
        services.AddScoped<ILargestTradesService, LargestTradesService>();
        services.AddScoped<IOHLCService, OHLCService>();
        services.AddScoped<IPreviousDayPriceService, PreviousDayPriceService>();
        services.AddScoped<IPriceOnlyService, PriceOnlyService>();
        services.AddScoped<IVolumeByVenueService, VolumeByVenueService>();
        services.AddScoped<IBookService, BookService>();
        services.AddScoped<IDelayedQuoteService, DelayedQuoteService>();
        services.AddScoped<IExchangeService, ExchangeService>();
    }

    public static void ConfigureJobs(this IServiceCollection services)
    {
        services.AddTransient<JobFactory>();

        services.AddScoped<StockDataJob>();
        services.AddScoped<FinancialsAsReportedJob>();

        services.AddScoped<BookJob>();
        services.AddScoped<DelayedQuoteJob>();
        services.AddScoped<IntradayPricesJob>();
        services.AddScoped<LargestTradesJob>();
        services.AddScoped<OHLCJob>();
        services.AddScoped<PreviousDayPriceJob>();
        services.AddScoped<PriceOnlyJob>();
        services.AddScoped<QuotesJob>();
        services.AddScoped<VolumeByVenueJob>();
    }

    public static void StartJobs(this WebApplication host)
    {
        using var scope = host.Services.CreateScope();

        var serviceProvider = scope.ServiceProvider;
        try
        {
            //StockDataScheduler.Start(serviceProvider);
            //FinancialsAsReportedScheduler.Start(serviceProvider);

            BookScheduler.Start(serviceProvider);
            DelayedQuoteScheduler.Start(serviceProvider);
            LargestTradesScheduler.Start(serviceProvider);
            OHLCScheduler.Start(serviceProvider);
            QuotesScheduler.Start(serviceProvider);
            VolumeByVenueScheduler.Start(serviceProvider);
            IntradayPricesScheduler.Start(serviceProvider);
            PriceOnlyScheduler.Start(serviceProvider);
            PreviousDayPriceScheduler.Start(serviceProvider);
        }
        catch (Exception)
        {
            throw;
        }
    }
}
