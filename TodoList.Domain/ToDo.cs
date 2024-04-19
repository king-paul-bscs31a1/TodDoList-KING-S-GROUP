using Todolist.Models;

namespace TodoList.Domain;

public class ToDo
{
    public bool Overdue
    {
        get
        {
            return DueDate.HasValue && DueDate.Value < DateTime.Today;
        }
    }

    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime? DueDate { get; set; }
    public Category Category { get; set; } = Category.Adventure; // Correct enum usage
    public bool IsActive { get; set; } = true;
    public Status Status { get; set; } = Status.New; // Correct enum usage
    public string StatusId { get; set; }

}