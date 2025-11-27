public static class Main
{
    public static void WebLeaderboard(this WebApplication app)
    {
        app.MapGet("/", () =>
        {
            return "Welcome to the Web Leaderboard!";
        }); 
    }
}