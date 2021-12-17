var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("/config/appsettings.json", true);

builder.Services
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services
    .AddHttpLogging(options =>
    {
        builder.Configuration.GetSection("HttpLogging").Bind(options);
    });

var app = builder.Build();

app.UseHttpLogging();
app.MapReverseProxy();

app.Run();
