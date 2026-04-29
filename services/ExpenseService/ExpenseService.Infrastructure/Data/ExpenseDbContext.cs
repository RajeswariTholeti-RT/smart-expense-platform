using Microsoft.EntityFrameworkCore;
using ExpenseService.Domain.Entities;

namespace ExpenseService.Infrastructure.Data;

public class ExpenseDbContext : DbContext
{
    public ExpenseDbContext(DbContextOptions<ExpenseDbContext> options)
        : base(options)
    {
    }

    public DbSet<Expense> Expenses => Set<Expense>();
}