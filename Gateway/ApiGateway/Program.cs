using static AuthService.AuthService;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5001);
});

builder.Services.AddGrpcClient<AuthServiceClient>(o =>
{
    o.Address = new Uri("http://localhost:5002");
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.MapControllers();

app.Lifetime.ApplicationStarted.Register(() =>
{
    Console.WriteLine("ApiGateway started successfully on http://localhost:5001");
});

app.Run();