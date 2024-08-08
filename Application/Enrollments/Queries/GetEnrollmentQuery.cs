using Application.Enrollments;
using Application.Common;
using AutoMapper;
using Infrastructure.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Chapters;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.UserEntities;

namespace Application.Enrollments.Queries
{
    public sealed record GetEnrollmentQuery : IRequest<BaseResponse<GetEnrollmentResponseModel>>
    {
        [Required]
        public Guid Id { get; init; }
    }

    public class GetEnrollmentQueryHanler : IRequestHandler<GetEnrollmentQuery, BaseResponse<GetEnrollmentResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetEnrollmentQueryHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetEnrollmentResponseModel>> Handle(GetEnrollmentQuery request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
            {
                return new BaseResponse<GetEnrollmentResponseModel>
                {
                    Success = false,
                    Message = "Get enrollment failed",
                    Errors = ["Id required"],
                };
            }

            var enrollment = await _context.Enrollments
                .Include(o => o.ApplicationUser).ThenInclude(o => o.Student)
                .Include(o => o.Course)
                .Include(o => o.Participants)
                .Include(o => o.WorksheetAttempts) .ThenInclude(o => o.Worksheet)
                .FirstOrDefaultAsync(x => x.Id.Equals(request.Id), cancellationToken);

            enrollment.PercentTopicCompletion = CactulatePercentTopicCompletion(enrollment.Id, enrollment.CourseId);                
            enrollment.PercentWorksheetCompletion = 1.0;
            

            var mappedEnrollment= _mapper.Map<GetEnrollmentResponseModel>(enrollment);

            return new BaseResponse<GetEnrollmentResponseModel>
            {
                Success = true,
                Message = "Get enrollment successful",
                Data = mappedEnrollment
            };
        }
        public double CactulatePercentTopicCompletion(Guid enrollmentId, Guid courseId)
        {
            var processions = _context.Processions
                .Include(o => o.Participant).ThenInclude(o => o.Enrollment)
                //.Where(o => o.Participant.IsPresent == true && o.Participant.Status == Domain.Enums.ParticipantStatus.Done)
                .Where(o => o.Participant.Enrollment.Id == enrollmentId)
                .ToList();
            var topics = _context.Topics
               .Include(o => o.Chapter).ThenInclude(o => o.Course)
               .Where(o => o.Chapter.Course.Id == courseId)
               .ToList();
            return ((double)processions.Count() / topics.Count()) * 100;
        }
    }
}
