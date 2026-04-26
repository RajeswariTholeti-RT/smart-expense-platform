namespace ExpenseService.Application.DTOs;

public class ExpenseRequest
{
    public string Title { get; set; } = string.Empty;
    public decimal Amount {get; set;}
}