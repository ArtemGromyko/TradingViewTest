using TradingViewTest.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;
var configuration = builder.Configuration;

builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddScoped<Aggreagator>();
services.AddScoped<RandomGenerator>();
services.AddScoped<GetAllService>();
services.AddScoped(typeof(Repository<>));

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
