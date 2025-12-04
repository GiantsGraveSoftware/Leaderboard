public static class UserLogin
{
    public static void WebUserLogin(this WebApplication app)
    {
        app.MapPost("/userlogin", (User user) =>
        {
            if (Utilities.IsValidApiKey(user.ApiKey) == false)
                return Results.Unauthorized();
            else if ((user.Email is null && user.HashEmail is null) || (user.Password is null && user.HashPassword is null))
                return Results.BadRequest(new { error = "Email, and Password are required to login." });
            else
            {
                string hashEmail = user.HashEmail ?? Utilities.HashString(user.Email) ?? "";
                int? userId = Utilities.GetID(hashEmail) ?? -1;
                string? salt = Utilities.GetSalt(userId);
                string hashPassword = user.HashPassword ?? Utilities.HashString(hashEmail.ToLower() + salt + user.Password) ?? "";

                using var connection = new Microsoft.Data.Sqlite.SqliteConnection(AppConfig.connectionString);
                connection.Open();
                using var cmd = connection.CreateCommand();
                cmd.CommandText = @"
                    SELECT  Id, Name, IFNULL(Banned,0), IFNULL(Bannedreason,'') 
                    FROM    Users 
                    WHERE   ID = $id AND Password = $password LIMIT 1;";

                cmd.Parameters.AddWithValue("$id", userId);
                cmd.Parameters.AddWithValue("$password", Utilities.DbVal(hashPassword));

                using var reader = cmd.ExecuteReader();                
                if (reader.Read())
                    return Results.Ok(new
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Banned = reader.GetInt32(2),
                        Bannedreason = reader.GetString(3)
                    });
                else
                    return Results.Unauthorized();
            }
        });
    }
}