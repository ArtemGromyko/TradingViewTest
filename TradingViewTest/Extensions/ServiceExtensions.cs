using TradingView.BLL.Abstractions;
using TradingView.BLL.Services;
using TradingView.DAL.Abstractions.ApiServices;
using TradingView.DAL.Abstractions.Repositories;
using TradingView.DAL.ApiServices;
using TradingView.DAL.Quartz;
using TradingView.DAL.Quartz.Jobs;
using TradingView.DAL.Quartz.Schedulers;
using TradingView.DAL.Repositories;
using TradingView.DAL.Settings;

namespace TradingViewTest.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<ISymbolService, SymbolService>();
        services.AddScoped<IStockProfileService, StockProfileService>();
        services.AddScoped<IStockFundamentalsService, StockFundamentalsService>();
    }

    public static void ConfigureRepositories(this IServiceCollection services)
    {
        services.AddScoped<ISymbolRepository, SymbolRepository>();
        services.AddScoped<IStockProfileRepository, StockProfileRepository>();
        services.AddScoped<IStockFundamentalsRepository, StockFundamentalsRepository>();
        services.AddScoped<IFinancialsAsReportedRepository, FinancialsAsReportedRepository>();
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

    public static void ConfigureApiServices(this IServiceCollection services)
    {
        services.AddScoped<ISymbolApiService, SymbolApiService>();
        services.AddScoped<IStockProfileApiService, StockProfileApiService>();
        services.AddScoped<IStockFundamentalsApiService, StockFundamentalsApiService>();
        services.AddScoped<IFinancialsAsReportedApiService, FinancialsAsReportedApiService>();
    }

    public static void ConfigureJobs(this IServiceCollection services)
    {
        services.AddTransient<JobFactory>();

        services.AddScoped<StockDataJob>();
    }

    public static void StartJobs(this WebApplication host)
    {
        using var scope = host.Services.CreateScope();

        var serviceProvider = scope.ServiceProvider;
        try
        {
            StockDataScheduler.Start(serviceProvider);
        }
        catch (Exception)
        {
            throw;
        }
    }
}
