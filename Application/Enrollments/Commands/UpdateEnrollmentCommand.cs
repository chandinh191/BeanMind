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
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Enums;

namespace Application.Enrollments.Commands
{
    [AutoMap(typeof(Domain.Entities.Enrollment), ReverseMap = true)]
    public sealed record UpdateEnrollmentCommand : IRequest<BaseResponse<GetBriefEnrollmentResponseModel>>
    {
        [Required]
        public Guid Id { get; set; }
        public string? ApplicationUserId { get; set; }
        public Guid? CourseId { get; set; }
        public EnrollmentStatus? Status { get; set; }
        public bool? IsDeleted { get; set; }
    }

    public class UpdateEnrollmentCommandHanler : IRequestHandler<UpdateEnrollmentCommand, BaseResponse<GetBriefEnrollmentResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateEnrollmentCommandHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetBriefEnrollmentResponseModel>> Handle(UpdateEnrollmentCommand request, CancellationToken cancellationToken)
        {
            if (request.CourseId != null)
            {
                var course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == request.CourseId);
                if (course == null)
                {
                    return new BaseResponse<GetBriefEnrollmentResponseModel>
                    {
                        Success = false,
                        Message = "Course not found",
                    };
                }
            }

            if (request.ApplicationUserId != null)
            {
                var applicationUser = await _context.ApplicationUsers.FirstOrDefaultAsync(x => x.Id.Equals(request.ApplicationUserId));
                if (applicationUser == null)
                {
                    return new BaseResponse<GetBriefEnrollmentResponseModel>
                    {
                        Success = false,
                        Message = "User not found",
                    };
                }
            }

            var enrollment = await _context.Enrollments.FirstOrDefaultAsync(x => x.Id == request.Id);

            if (enrollment == null)
            {
                return new BaseResponse<GetBriefEnrollmentResponseModel>
                {
                    Success = false,
                    Message = "Enrollment is not found",
                    Errors = ["Enrollment is not found"]
                };
            }

            if (request.Status != null && request.Status == EnrollmentStatus.Complete)
            {
                double percentTopicCompletion = CactulatePercentTopicCompletion(enrollment.Id, enrollment.CourseId);
                if (percentTopicCompletion < 100) {
                    return new BaseResponse<GetBriefEnrollmentResponseModel>
                    {
                        Success = false,
                        Message = "Student have not completed 100% of the topic",
                    };
                }
            }

            //_mapper.Map(request, enrollment);
            // Use reflection to update non-null properties
            foreach (var property in request.GetType().GetProperties())
            {
                var requestValue = property.GetValue(request);
                if (requestValue != null)
                {
                    var targetProperty = enrollment.GetType().GetProperty(property.Name);
                    if (targetProperty != null)
                    {
                        targetProperty.SetValue(enrollment, requestValue);
                    }
                }
            }

            var updateEnrollmentResult = _context.Update(enrollment);

            if (updateEnrollmentResult.Entity == null)
            {
                return new BaseResponse<GetBriefEnrollmentResponseModel>
                {
                    Success = false,
                    Message = "Update enrollment failed",
                };
            }

            await _context.SaveChangesAsync(cancellationToken);

            var mappedEnrollmentResult = _mapper.Map<GetBriefEnrollmentResponseModel>(updateEnrollmentResult.Entity);

            return new BaseResponse<GetBriefEnrollmentResponseModel>
            {
                Success = true,
                Message = "Update enrollment successful",
                Data = mappedEnrollmentResult
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
