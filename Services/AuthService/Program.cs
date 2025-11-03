using AuthService;
using AuthService.Interfaces;
using AuthService.Presentation;
using AuthService.Services;
using AuthService.Validators;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5002);
});

// Add gRPC services to the container.
builder.Services.AddGrpc();

// Register business logic services
builder.Services.AddScoped<IAuthService, AuthServiceLogic>();

// Register validators
builder.Services.AddScoped<IValidator<SignInRequest>, SignInRequestValidation>();
builder.Services.AddScoped<IValidator<SignUpRequest>, SignUpRequestValidation>();

var app = builder.Build();

app.MapGrpcService<AuthServiceImpl>();

// Configure the HTTP request pipeline.
app.MapGet("/", () => "App is running");

app.Lifetime.ApplicationStarted.Register(() =>
{
    Console.WriteLine("AuthService started successfully on https://localhost:5002");
});

app.Run();
