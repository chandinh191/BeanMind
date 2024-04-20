using BeanMind.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BeanMind.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<TodoList> TodoLists { get; }

    DbSet<TodoItem> TodoItems { get; }
    DbSet<BeanMind.Domain.Entities.Subject> Subjects { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
