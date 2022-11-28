namespace TradingView.DAL.Settings;

public class DatabaseSettings
{
    public string ConnectionString { get; set; } = Environment.GetEnvironmentVariable("CONNECTION_STRING");

    public string DatabaseName { get; set; } = null!;
}
