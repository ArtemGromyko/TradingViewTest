using Serilog;
using TradingView.BLL.Services;
using TradingViewTest.Services;

var builder = WebApplication.CreateBuilder(args);

string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmm");
builder.Host.UseSerilog((context, configuration) =>
{
    configuration
        .WriteTo.Console();
});

var services = builder.Services;
var configuration = builder.Configuration;

builder.Services.AddControllers().AddNewtonsoftJson();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddScoped<Aggreagator>();
services.AddScoped<GetAllService>();

services.AddHttpClient(configuration["HttpClientName"], client =>
{
    client.BaseAddress = new Uri(configuration["IEXCloudUrls:baseUrl"]);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
