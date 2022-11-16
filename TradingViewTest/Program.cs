using Serilog;
using TradingView.BLL.Services;
using TradingView.DAL.Quartz;
using TradingViewTest.Extensions;

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

services.ConfigureRepositories();
services.ConfigureServices();
services.ConfigureApiServices();
services.ConfigureHttpClient(configuration);
services.ConfigureMongoDBConnection(configuration);

services.ConfigureJobs();

services.AddScoped<GetAllService>();

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

app.StartJobs();

app.Run();
