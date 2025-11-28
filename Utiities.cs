public static class Utilities
{
    public static bool IsValidApiKey(string? apiKey)
    {
        return apiKey == AppConfig.ApiKey && !string.IsNullOrEmpty(apiKey);
    }

    public static void SetupDatabase()
    {
        SetupUsers();
    }

    private static void SetupUsers()
    {
        var connection = new Microsoft.Data.Sqlite.SqliteConnection(AppConfig.connectionString);
        connection.Open();
        var cmd = connection.CreateCommand();
        cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS Users (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name VARCHAR(255) NOT NULL,
                Email VARCHAR(255) ,
                Salt VARCHAR(255),
                Password VARCHAR(255),
                TimeStamp DateTime Default CURRENT_TIMESTAMP,
                Banned INT DEFAULT 0,
                BannedReason VARCHAR(255),
                UNIQUE (Email)
            );
        ";
        cmd.ExecuteNonQuery();
        connection.Close(); 
    }
}