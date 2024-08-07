using Application.Common;
using Application.Participants;
using AutoMapper;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Parents.Queries
{
    public sealed record GetParentQuery : IRequest<BaseResponse<GetParentResponseModel>>
    {
        [Required]
        public Guid Id { get; init; }
    }

    public class GetParentQueryHanler : IRequestHandler<GetParentQuery, BaseResponse<GetParentResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetParentQueryHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetParentResponseModel>> Handle(GetParentQuery request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
            {
                return new BaseResponse<GetParentResponseModel>
                {
                    Success = false,
                    Message = "Get parent failed",
                    Errors = ["Id required"],
                };
            }

            var parent = await _context.Parents
                .Include(o => o.ApplicationUser)
                .Include(o => o.Students).ThenInclude(o => o.ApplicationUser).ThenInclude(o => o.Enrollments).ThenInclude(o => o.Course)
                .FirstOrDefaultAsync(x => x.Id.Equals(request.Id), cancellationToken);

            var mappedParent = _mapper.Map<GetParentResponseModel>(parent);
            foreach (var student in mappedParent.Students)
            {
                foreach (var enrollment in student.ApplicationUser.Enrollments)
                {
                    enrollment.PercentTopicCompletion = CactulatePercentTopicCompletion(enrollment.Id,enrollment.CourseId);
                    //enrollment.PercentWorksheetCompletion = CactulatePercentWorksheetCompletion(enrollment.Id, enrollment.CourseId);
                    enrollment.PercentWorksheetCompletion = 0.0;
                }
            }

            return new BaseResponse<GetParentResponseModel>
            {
                Success = true,
                Message = "Get parent successful",
                Data = mappedParent
            };
        }
        public double CactulatePercentTopicCompletion(Guid enrollmentId, Guid courseId) {
            var processions = _context.Processions
                .Include(o => o.Participant).ThenInclude(o => o.Enrollment)
                .Where(o => o.Participant.IsPresent == true && o.Participant.Status == Domain.Enums.ParticipantStatus.Done)
                .Where(o => o.Participant.Enrollment.Id == enrollmentId)                
                .AsQueryable();
            var topics = _context.Topics
               .Include(o => o.Chapter).ThenInclude(o => o.Course)
               .Where(o => o.Chapter.Course.Id == courseId)
               .AsQueryable();

            return (double)(processions.Count()/topics.Count());
        }
/*        public double CactulatePercentWorksheetCompletion(Guid enrollmentId, Guid courseId)
        {
            var chapters = _context.Chapters
               .Where(o => o.CourseId == courseId)
               .AsQueryable();
            var topics = _context.Topics
               .Where(o => o.CourseId == courseId)
               .AsQueryable();

            return (double)(processions.Count() / topics.Count());
        }*/
    }
}
