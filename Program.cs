using System.Diagnostics;
DotNetEnv.Env.Load();

Utilities.SetupDatabase();

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5000, listenOptions =>
    {
        listenOptions.UseHttps();
    });
});

var app = builder.Build();

app.WebLeaderboard();
app.WebUserCreate();
app.MapUserLogin();

app.Run();
