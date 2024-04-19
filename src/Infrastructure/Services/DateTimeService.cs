using BeanMind.Application.Common.Interfaces;

namespace BeanMind.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}
