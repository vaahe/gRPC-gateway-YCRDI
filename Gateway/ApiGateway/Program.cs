using Database;
using Microsoft.EntityFrameworkCore;
using static AuthService.AuthService;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5001);
});

builder.Services.AddGrpcClient<AuthServiceClient>(o =>
{
    o.Address = new Uri("http://authservice:5002");
});

builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlite("Data Source=app.db"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.MapControllers();

app.Lifetime.ApplicationStarted.Register(() =>
{
    Console.WriteLine("ApiGateway started successfully on http://localhost:5001");
});

app.Run();