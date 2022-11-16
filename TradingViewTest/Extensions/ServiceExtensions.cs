﻿using TradingView.BLL.Abstractions;
using TradingView.BLL.Services;
using TradingView.DAL.Abstractions.ApiServices;
using TradingView.DAL.Abstractions.Repositories;
using TradingView.DAL.ApiServices;
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
    }
}
