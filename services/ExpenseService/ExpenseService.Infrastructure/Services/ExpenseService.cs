using ExpenseService.Application;
using ExpenseService.Application.DTOs;
using ExpenseService.Domain.Entities;

namespace ExpenseService.Infrastructure.Services;
public class ExpenseService : IExpenseService
{   
    private static List<Expense> _expenses = new();
    
    public Task<List<Expense>> GetAllAsync()
    {
        return Task.FromResult(_expenses);
    }

    public Task<Expense> CreateAsync(ExpenseRequest request)
    {
        var expense = new Expense
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Amount = request.Amount
         };

         _expenses.Add(expense);

         return Task.FromResult(expense);
    }
}