
using Application.Enrollments;
using Application.Enrollments.Commands;
using AutoMapper;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.BackgroundServices
{
    public class CheckingCompeleteEnrollment : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMapper _mapper;
        private readonly ISender _sender;
        private readonly ILogger<CheckingCompeleteEnrollment> _logger;

        public CheckingCompeleteEnrollment(IServiceProvider serviceProvider, IMapper mapper, ISender sender, ILogger<CheckingCompeleteEnrollment> logger)
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
                _logger.LogInformation("Running background service Checking Complete Enrollment");

                using (var scope = _serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    var enrollments = await context.Enrollments.ToListAsync(stoppingToken);
                    var mappedEnrollments = _mapper.Map<List<GetBriefEnrollmentResponseModelVer2>>(enrollments);

                    foreach (var mappedEnrollment in mappedEnrollments)
                    {
                        var participant = await context.Participants
                            .FirstOrDefaultAsync(x => x.EnrollmentId == mappedEnrollment.Id && x.Status == Domain.Enums.ParticipantStatus.NotYet);

                        if (participant == null)
                        {
                            mappedEnrollment.PercentTopicCompletion = await CalculatePercentTopicCompletionAsync(context, mappedEnrollment.Id, mappedEnrollment.CourseId, stoppingToken);

                            if (mappedEnrollment.PercentTopicCompletion >= 100)
                            {
                                var result = await _sender.Send(new UpdateEnrollmentCommand
                                {
                                    Id = mappedEnrollment.Id,
                                    Status = Domain.Enums.EnrollmentStatus.Complete,
                                }, stoppingToken);
                            }
                        }
                    }
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

        private async Task<double> CalculatePercentTopicCompletionAsync(ApplicationDbContext context, Guid enrollmentId, Guid courseId, CancellationToken stoppingToken)
        {
            var completedTopicsCount = await context.Processions
                .Include(o => o.Participant)
                .ThenInclude(o => o.Enrollment)
                .Where(o => o.Participant.Enrollment.Id == enrollmentId)
                .CountAsync(stoppingToken);

            var totalTopicsCount = await context.Topics
                .Include(o => o.Chapter)
                .ThenInclude(o => o.Course)
                .Where(o => o.Chapter.Course.Id == courseId)
                .CountAsync(stoppingToken);

            return totalTopicsCount == 0 ? 0 : ((double)completedTopicsCount / totalTopicsCount) * 100;
        }
    }


}
