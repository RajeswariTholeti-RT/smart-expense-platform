using ExpenseService.Application;
using ExpenseService.Application.DTOs;
using ExpenseService.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ======================
// 🔧 DEPENDENCY INJECTION
// ======================
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<IExpenseService, ExpenseService.Infrastructure.Services.ExpenseService>();

// ======================
// 🔧 JWT AUTHENTICATION
// ======================
var jwtSettings = builder.Configuration.GetSection("Jwt");
var keyString = jwtSettings["Key"] ?? throw new Exception("JWT Key missing");
var key = Encoding.UTF8.GetBytes(keyString);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddAuthorization();

// ======================
// 🔧 SWAGGER + JWT SUPPORT
// ======================
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

var app = builder.Build();

// ======================
// 🔧 MIDDLEWARE
// ======================
// app.UseSwagger();
// app.UseSwaggerUI();

// Configure the HTTP request pipeline.     
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// ======================
// 🔧 ENDPOINTS
// ======================

app.MapGet("/expenses", async (IExpenseService service) =>
{
    return await service.GetAllAsync();
}).RequireAuthorization();

app.MapPost("/expenses", async (IExpenseService service, ExpenseRequest request) =>
{
    return await service.CreateAsync(request);
}).RequireAuthorization();

app.Run();