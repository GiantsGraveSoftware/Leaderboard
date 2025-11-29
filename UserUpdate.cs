using System.Diagnostics;
public static class UserUpdate
{
    public static void WebUserUpdate(this WebApplication app)
    {
        app.MapPost("/userupdate", (User user) =>
        {
            if (Utilities.IsValidApiKey(user.ApiKey) == false)
                return Results.Unauthorized();
            else if (user.ID is null)
                return Results.BadRequest(new { error = "User IDs are required to update." });
            else
            {
                int rowsAffected;

                using var connection = new Microsoft.Data.Sqlite.SqliteConnection(AppConfig.connectionString);
                connection.Open();

                using var cmd = connection.CreateCommand();
                cmd.Parameters.AddWithValue("$id", user.ID);

                Debug.Print("Updating user details...");
                cmd.CommandText = @"
                    UPDATE Users 
                    SET Name = IFNULL($name,Name)
                    ,   Banned = IFNULL($banned,banned)
                    ,   BannedReason = IFNULL($bannedreason,BannedReason)
                    WHERE ID = $id;";

                cmd.Parameters.AddWithValue("$name", Utilities.DbVal(user.Name));
                cmd.Parameters.AddWithValue("$banned", user.Banned.HasValue ? (object)user.Banned.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("$bannedreason", Utilities.DbVal(user.BannedReason));
                rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected == 0)
                    return Results.NotFound(new { error = "User not found." });
                else
                    return Results.Ok($"User '{user.Name}' updated successfully!");
            }
        });
    }
}