public record User(string ApiKey, int? ID, string? Name, string? Email, string? HashEmail, string? Password,
                   string? HashPassword, string? OldPassword, string? OldHashPassword, int? Banned, 
                   string? BannedReason);
