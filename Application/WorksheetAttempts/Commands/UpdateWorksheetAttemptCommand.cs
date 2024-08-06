using Application.Common;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.WorksheetAttempts.Commands
{
    public class UpdateWorkSheetAttemptAnswerModel
    {
        public Guid QuestionAnswerId { get; set; }
    }
    [AutoMap(typeof(Domain.Entities.WorksheetAttempt), ReverseMap = true)]
    public sealed record UpdateWorksheetAttemptCommand : IRequest<BaseResponse<GetBriefWorksheetAttemptResponseModel>>
    {
        [Required]
        public Guid Id { get; init; }
        public Guid? EnrollmentId { get; set; }
        public Guid? WorksheetId { get; set; }
        public DateTime? CompletionDate { get; set; }
        public WorksheetAttemptStatus Status { get; set; }
        public int? Score { get; set; }
        public List<CreateWorkSheetAttemptAnswerModel>? WorkSheetAttemptAnswers { get; set; }
    }

    public class UpdateWorksheetAttemptCommandHanler : IRequestHandler<UpdateWorksheetAttemptCommand, BaseResponse<GetBriefWorksheetAttemptResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateWorksheetAttemptCommandHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetBriefWorksheetAttemptResponseModel>> Handle(UpdateWorksheetAttemptCommand request, CancellationToken cancellationToken)
        {
            if (request.EnrollmentId != null)
            {
                var enrollment = await _context.Enrollments.FirstOrDefaultAsync(x => x.Id == request.EnrollmentId);

                if (enrollment == null)
                {
                    return new BaseResponse<GetBriefWorksheetAttemptResponseModel>
                    {
                        Success = false,
                        Message = "Enrollment not found",
                    };
                }
            }
            if (request.WorksheetId != null)
            {
                var worksheet = await _context.Worksheets.FirstOrDefaultAsync(x => x.Id == request.WorksheetId);

                if (worksheet == null)
                {
                    return new BaseResponse<GetBriefWorksheetAttemptResponseModel>
                    {
                        Success = false,
                        Message = "Worksheet not found",
                    };
                }
            }

            var worksheetAttempt = await _context.WorksheetAttempts.FirstOrDefaultAsync(x => x.Id == request.Id);

            if (worksheetAttempt== null)
            {
                return new BaseResponse<GetBriefWorksheetAttemptResponseModel>
                {
                    Success = false,
                    Message = "Worksheet attempt is not found",
                    Errors = ["Worksheet attempt is not found"]
                };
            }

            // Use reflection to update non-null properties
            foreach (var property in request.GetType().GetProperties())
            {
                var requestValue = property.GetValue(request);
                if (requestValue != null)
                {
                    var targetProperty = worksheetAttempt.GetType().GetProperty(property.Name);
                    if (targetProperty != null && targetProperty.Name != "WorkSheetAttemptAnswers")
                    {
                        targetProperty.SetValue(worksheetAttempt, requestValue);
                    }
                }
            }

            var updateWorksheetAttemptResult = _context.Update(worksheetAttempt);

            if (updateWorksheetAttemptResult.Entity == null)
            {
                return new BaseResponse<GetBriefWorksheetAttemptResponseModel>
                {
                    Success = false,
                    Message = "Update worksheet attempt failed",
                };
            }

            await _context.SaveChangesAsync(cancellationToken);

            if (request.WorkSheetAttemptAnswers != null && request.WorkSheetAttemptAnswers.Count > 0)
            {
                var workSheetAttemptAnswers = _context.WorksheetAttemptAnswers.Where(x => x.WorksheetAttemptId == worksheetAttempt.Id).AsQueryable();
                foreach (var record in workSheetAttemptAnswers)
                {
                    _context.Remove(record);
                }
                await _context.SaveChangesAsync(cancellationToken);
                foreach (var record in request.WorkSheetAttemptAnswers)
                {
                    var existedWorkSheetAttemptAnswers = await _context.WorksheetAttemptAnswers
                        .FirstOrDefaultAsync(x => x.QuestionAnswerId == record.QuestionAnswerId && x.WorksheetAttemptId == worksheetAttempt.Id);
                    if (existedWorkSheetAttemptAnswers != null)
                    {
                        existedWorkSheetAttemptAnswers.IsDeleted = false;
                    }
                    else
                    {
                        var worksheetAttemptAnswer = new WorksheetAttemptAnswer()
                        {
                            WorksheetAttemptId = worksheetAttempt.Id,
                            QuestionAnswerId = record.QuestionAnswerId, 
                        };
                        var createWorksheetAttemptAnswerResult = await _context.AddAsync(worksheetAttemptAnswer, cancellationToken);
                    }
                }
            }
            await _context.SaveChangesAsync(cancellationToken);

            var mappedWorksheetAttemptResult = _mapper.Map<GetBriefWorksheetAttemptResponseModel>(updateWorksheetAttemptResult.Entity);

            return new BaseResponse<GetBriefWorksheetAttemptResponseModel>
            {
                Success = true,
                Message = "Update worksheet attempt successful",
                Data = mappedWorksheetAttemptResult
            };
        }
    }
}
