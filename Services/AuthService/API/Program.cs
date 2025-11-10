using AuthService;
using AuthService.API.Services;
using AuthService.Application.Interfaces;
using AuthService.Application.Services;
using AuthService.Validators;
using Database;
using FluentValidation;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options => { options.ListenAnyIP(5002, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http2;
    });
});

builder.Services.AddGrpc();
builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IAuthService, AuthServiceImpl>();

// Register validators
builder.Services.AddScoped<IValidator<SignInRequest>, SignInRequestValidation>();
builder.Services.AddScoped<IValidator<SignUpRequest>, SignUpRequestValidation>();


var app = builder.Build();

app.MapGrpcService<AuthGrpcService>();
app.MapGet("/", () => "AuthService is running...");

app.Lifetime.ApplicationStarted.Register(() =>
{
    Console.WriteLine("AuthService started successfully on http://localhost:5002");
});

Console.WriteLine($"[BOOT] ContentRoot = {builder.Environment.ContentRootPath}");
Console.WriteLine($"[BOOT] BaseDirectory = {AppContext.BaseDirectory}");
Console.WriteLine($"[BOOT] DefaultConnection = {builder.Configuration.GetConnectionString("DefaultConnection")}");

app.Run();