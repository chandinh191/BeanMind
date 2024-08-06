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
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.WorksheetAttempts.Commands
{
    public class CreateWorkSheetAttemptAnswerModel
    {
        public Guid QuestionAnswerId { get; set; }
    }
    [AutoMap(typeof(Domain.Entities.WorksheetAttempt), ReverseMap = true)]
    public sealed record CreateWorksheetAttemptCommand : IRequest<BaseResponse<GetBriefWorksheetAttemptResponseModel>>
    {
        [Required]
        public Guid EnrollmentId { get; set; }
        [Required]
        public Guid WorksheetId { get; set; }
        public DateTime? CompletionDate { get; set; }
        public WorksheetAttemptStatus Status { get; set; }
        public int? Score { get; set; }
        public List<CreateWorkSheetAttemptAnswerModel>? WorkSheetAttemptAnswers {  get; set; }
    }

    public class CreateWorksheetAttemptCommandHanler : IRequestHandler<CreateWorksheetAttemptCommand, BaseResponse<GetBriefWorksheetAttemptResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateWorksheetAttemptCommandHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetBriefWorksheetAttemptResponseModel>> Handle(CreateWorksheetAttemptCommand request, CancellationToken cancellationToken)
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

            var worksheet = await _context.Worksheets.FirstOrDefaultAsync(x => x.Id == request.WorksheetId);
            if (worksheet == null)
            {
                return new BaseResponse<GetBriefWorksheetAttemptResponseModel>
                {
                    Success = false,
                    Message = "Worksheet not found",
                };
            }

            var worksheetAttempt = new WorksheetAttempt()
            {
                EnrollmentId = request.EnrollmentId,
                WorksheetId = request.WorksheetId,
                CompletionDate = request.CompletionDate,
                Status = request.Status,
                Score = request.Score
            };
            var createWorksheetAttemptResult = await _context.AddAsync(worksheetAttempt, cancellationToken);

            if (createWorksheetAttemptResult.Entity == null)
            {
                return new BaseResponse<GetBriefWorksheetAttemptResponseModel>
                {
                    Success = false,
                    Message = "Create worksheet attempt failed",
                };
            }

            await _context.SaveChangesAsync(cancellationToken);

            if(request.WorkSheetAttemptAnswers != null && request.WorkSheetAttemptAnswers.Count > 0)
            {
                foreach(var answer in request.WorkSheetAttemptAnswers)
                {
                    var worksheetAttemptAnswer = new WorksheetAttemptAnswer()
                    {
                        WorksheetAttemptId = worksheetAttempt.Id,
                        QuestionAnswerId = answer.QuestionAnswerId,
                    };
                    var createWorksheetAttemptAnswerResult = await _context.AddAsync(worksheetAttemptAnswer, cancellationToken);
                }
            }

            await _context.SaveChangesAsync(cancellationToken);

            var mappedWorksheetAttemptResult = _mapper.Map<GetBriefWorksheetAttemptResponseModel>(createWorksheetAttemptResult.Entity);

            return new BaseResponse<GetBriefWorksheetAttemptResponseModel>
            {
                Success = true,
                Message = "Create worksheet attempt successful",
                Data = mappedWorksheetAttemptResult
            };
        }
    }
}
