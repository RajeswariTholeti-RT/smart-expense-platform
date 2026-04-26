using ExpenseService.Application.DTOs;
using ExpenseService.Domain.Entities;

namespace ExpenseService.Application;

public interface IExpenseService
{
    Task<List<Expense>> GetAllAsync();
    Task<Expense> CreateAsync(ExpenseRequest request);
}