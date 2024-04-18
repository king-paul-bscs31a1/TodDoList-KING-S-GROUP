using TodoList.Domain;
using TodoList.Domain.Interface;
using Todolist.Models;

namespace Todo.Service;

public class TodoService(ITodoRepository repository) : ITodoService
{
    public async Task<bool> Delete(int id)
    {
        var todo = await repository.GetByIdAsync(id);
        todo.IsActive = false;
        repository.Update(todo);
        await repository.SaveChangesAsync(default);
        return true;
    }

    public async Task<bool> MarkAsDone(int id)
    {
        var todo = await repository.GetByIdAsync(id);
        todo.Status = Status.Done;
        repository.Update(todo);
        await repository.SaveChangesAsync(default);

        return true;
    }

    public IEnumerable<TodoModel> GetAll()
    {
        return repository.GetAll().Select(c => new TodoModel()
        {
            Category = c.Category,
            IsActive = c.IsActive,
            Description = c.Description,
            DueDate = c.DueDate,
            Status = c.Status,
            Id = c.Id
        }).ToArray();
    }

    public async Task<int> Add(AddTodoModel model)
    {
        repository.Add(new ToDo()
        {
            Category = model.Category,
            Description = model.Description,
            DueDate = model.DueDate,
            IsActive = true,
            Status = Status.New,
        });

        return await repository.SaveChangesAsync();
    }

    public async Task<int> UpdateDescription(int id, string description)
    {
        var todo = await repository.GetByIdAsync(id);
        todo.Description = description;

        repository.Update(todo);
        await repository.SaveChangesAsync(default);

        return 0;
    }
}