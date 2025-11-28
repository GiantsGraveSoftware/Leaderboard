using Microsoft.Data.Sqlite;
public static class UserCreate
{
    public static void WebUserCreate(this WebApplication app)
    {
        app.MapPost("/adduser", (User user) =>
        {
            if (Utilities.IsValidApiKey(user.ApiKey) == false)
                return Results.Unauthorized();
            else if (user.Name is null || (user.Email is null && user.HashEmail is null) || 
                    (user.Password is null && user.HashPassword is null))
                return Results.BadRequest(new { error = "Name, Email, and Password are required to add a user." });
            else
            {
                string salt = Utilities.GenerateSalt(32);
                string? hashEmail = user.HashEmail;
                string? hashPassword = user.HashPassword;

                if (hashEmail is null && user.Email is not null)
                    hashEmail = Utilities.HashString(user.Email);

                if (hashPassword is null && hashEmail is not null && salt is not null && user.Password is not null)
                    hashPassword = Utilities.HashString( hashEmail.ToLower() + salt + user.Password);                       
               
                var connection = new SqliteConnection(AppConfig.connectionString);
                connection.Open();
                try
                {
                    var cmd = connection.CreateCommand();
                    cmd.CommandText = "INSERT INTO Users (Name, Email, Salt, Password) VALUES ($name, $email, $salt, $password);";
                    cmd.Parameters.AddWithValue("$name", user.Name);
                    cmd.Parameters.AddWithValue("$email", hashEmail);
                    cmd.Parameters.AddWithValue("$salt", salt);
                    cmd.Parameters.AddWithValue("$password", hashPassword);
                    cmd.ExecuteNonQuery();
                    connection.Close();

                    return Results.Ok($"User '{user.Name}' added successfully!");
                }
                catch (SqliteException ex) when (ex.SqliteErrorCode == 19) 
                {
                    connection.Close();
                    return Results.Conflict(new { error = $"User with email '{user.Email}' already exists." });
                }
            }
        });
    }
}