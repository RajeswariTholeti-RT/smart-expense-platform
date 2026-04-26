using ExpenseService.Application;
using ExpenseService.Application.DTOs;
using ExpenseService.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<IExpenseService, ExpenseService.Infrastructure.Services.ExpenseService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ======================
// 🔧 MIDDLEWARE
// ======================
app.UseSwagger();
app.UseSwaggerUI();

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
});

app.MapPost("/expenses", async (IExpenseService service, ExpenseRequest request) =>
{
    return await service.CreateAsync(request);
});

app.Run();