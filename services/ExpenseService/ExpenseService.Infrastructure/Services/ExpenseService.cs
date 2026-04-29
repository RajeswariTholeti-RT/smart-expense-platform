using ExpenseService.Application;
using ExpenseService.Application.DTOs;
using ExpenseService.Domain.Entities;
using ExpenseService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpenseService.Infrastructure.Services;
public class ExpenseService : IExpenseService
{   
    private readonly ExpenseDbContext _db;

    public ExpenseService(ExpenseDbContext db)
    {
        _db = db;
    }
    
    public async Task<List<Expense>> GetAllAsync()
    {
        return await _db.Expenses.ToListAsync();
    }

    public async Task<Expense> CreateAsync(ExpenseRequest request)
    {
        var expense = new Expense
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Amount = request.Amount
         };

        _db.Expenses.Add(expense);
        await _db.SaveChangesAsync();

        return expense;
    }
}