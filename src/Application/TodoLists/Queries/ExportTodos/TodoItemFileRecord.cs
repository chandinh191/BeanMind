using BeanMind.Application.Common.Mappings;
using BeanMind.Domain.Entities;

namespace BeanMind.Application.TodoLists.Queries.ExportTodos;

public class TodoItemRecord : IMapFrom<TodoItem>
{
    public string? Title { get; init; }

    public bool Done { get; init; }
}
