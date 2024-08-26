
using Application.Orders;
using AutoMapper;
using Infrastructure.Data;
using MediatR;
using Application.Orders.Commands;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;


namespace Application.BackgroundServices
{
    public class CheckingExpiredOrder : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMapper _mapper;
        private readonly ISender _sender;
        private readonly ILogger<CheckingExpiredOrder> _logger;

        public CheckingExpiredOrder(IServiceProvider serviceProvider, IMapper mapper, ISender sender, ILogger<CheckingExpiredOrder> logger)
        {
            _serviceProvider = serviceProvider;
            _mapper = mapper;
            _sender = sender;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Running background service Checking Expired Order");

                using (var scope = _serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    var orders = await context.Orders
                        .Where(o => o.Created.AddMinutes(15) < DateTime.UtcNow.AddHours(14) && o.Status == Domain.Enums.OrderStatus.Pending)
                        .ToListAsync(stoppingToken);

                    foreach (var order in orders)
                    {
                        var result = await _sender.Send(new UpdateOrderCommand
                        {
                            Id = order.Id,
                            Status = Domain.Enums.OrderStatus.Cancelled,
                        }, stoppingToken);
                    }
                }

                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }
    }

}
