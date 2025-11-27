public static class Utilities
{
    public static bool IsValidApiKey(string? apiKey)
    {
        return apiKey == AppConfig.ApiKey && !string.IsNullOrEmpty(apiKey);
    }
}