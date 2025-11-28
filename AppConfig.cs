public static class AppConfig
{
    public static readonly string? ApiKey = Environment.GetEnvironmentVariable("apikey");
    public const string databaseFile = "Leaderboard.db";
    public static string connectionString => $"Data Source={databaseFile}";

}