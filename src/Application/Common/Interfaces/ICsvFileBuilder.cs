using BeanMind.Application.TodoLists.Queries.ExportTodos;

namespace BeanMind.Application.Common.Interfaces;

public interface ICsvFileBuilder
{
    byte[] BuildTodoItemsFile(IEnumerable<TodoItemRecord> records);
}
