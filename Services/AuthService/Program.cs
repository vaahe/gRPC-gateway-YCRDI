using AuthService.Presentation;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5002, listenOptions =>
    {
        listenOptions.UseHttps();
    });
});

// Add gRPC services to the container.
builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<AuthServiceImpl>();

// Configure the HTTP request pipeline.
app.MapGet("/", () => "App is running");

app.Lifetime.ApplicationStarted.Register(() =>
{
    Console.WriteLine("AuthService started successfully on https://localhost:5002");
});

app.Run();
