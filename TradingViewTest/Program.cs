using Serilog;
using TradingViewTest.Extensions;
using TradingViewTest.Middlewares;

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

services.ConfigureCors();
services.ConfigureRepositories();
services.ConfigureServices();
services.ConfigureApiServices();
services.ConfigureHttpClient(configuration);
services.ConfigureMongoDBConnection(configuration);
services.ConfigureJobs(configuration);

services.ConfigureJobs();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseCors("CorsPolicy");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.StartJobs();

app.Run();
