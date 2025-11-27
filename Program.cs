using System.Diagnostics;
DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5000, listenOptions =>
    {
        listenOptions.UseHttps();
    });
});

Debug.Print(Utilities.IsValidApiKey("wrong key").ToString());
Debug.Print(Utilities.IsValidApiKey("My New Api Key").ToString());

var app = builder.Build();

app.WebLeaderboard();

app.Run();
