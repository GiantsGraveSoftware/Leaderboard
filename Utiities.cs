public static class Utilities
{
    public static bool IsValidApiKey(string? apiKey)
    {
        return apiKey == AppConfig.ApiKey && !string.IsNullOrEmpty(apiKey);
    }
    public static string GenerateSalt(int size = 32)
    {
        byte[] saltBytes = new byte[size];
        using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
        rng.GetBytes(saltBytes);

        return Convert.ToBase64String(saltBytes);
    }
    public static string? HashString(string? ToHash)
    {
        if (ToHash is null)
            return null;

        using var sha256 = System.Security.Cryptography.SHA256.Create();
        string ToHashAndKey = ToHash + AppConfig.ApiKey;
        byte[] bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(ToHash));

        var builder = new System.Text.StringBuilder();
        foreach (var b in bytes)
            builder.Append(b.ToString("x2"));

        string hash = builder.ToString();
        return hash;
    }
    public static int? GetID(string hashEmail)
    {
        var connection = new Microsoft.Data.Sqlite.SqliteConnection(AppConfig.connectionString);
        connection.Open();
        var cmd = connection.CreateCommand();
        cmd.CommandText = "SELECT ID FROM Users WHERE email = $hashEmail LIMIT 1";
        cmd.Parameters.AddWithValue("$hashEmail",hashEmail);

        using var reader = cmd.ExecuteReader();
        if (reader.Read())
            return reader.GetInt32(0);
        else
            return null;
    }
    public static string? GetSalt(int? userId)
    {
        if (userId is null)
            return null;

        using var connection = new Microsoft.Data.Sqlite.SqliteConnection(AppConfig.connectionString);
        connection.Open();
        
        using var cmd = connection.CreateCommand();
        cmd.CommandText = "SELECT Salt FROM Users WHERE ID = $id LIMIT 1";
        cmd.Parameters.AddWithValue("$id", userId);

        using var reader = cmd.ExecuteReader();
        if (reader.Read())
            return reader.GetString(0);
        else
            return null;
    }
    public static object DbVal(object? v) => v is null ? DBNull.Value : v!;
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