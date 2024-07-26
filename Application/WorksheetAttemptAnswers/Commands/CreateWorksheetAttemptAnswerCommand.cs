using Application.Common;
using AutoMapper;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.WorksheetAttemptAnswers.Commands
{

    [AutoMap(typeof(Domain.Entities.WorksheetAttemptAnswer), ReverseMap = true)]
    public sealed record CreateWorksheetAttemptAnswerCommand : IRequest<BaseResponse<GetBriefWorksheetAttemptAnswerResponseModel>>
    {
        [Required]
        public Guid WorksheetAttemptId { get; set; }
        [Required]
        public Guid QuestionAnswerId { get; set; }
    }

    public class CreateWorksheetAttemptAnswerCommandHanler : IRequestHandler<CreateWorksheetAttemptAnswerCommand, BaseResponse<GetBriefWorksheetAttemptAnswerResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateWorksheetAttemptAnswerCommandHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetBriefWorksheetAttemptAnswerResponseModel>> Handle(CreateWorksheetAttemptAnswerCommand request, CancellationToken cancellationToken)
        {
            var worksheetAttempt = await _context.WorksheetAttempts.FirstOrDefaultAsync(x => x.Id == request.WorksheetAttemptId);
            if (worksheetAttempt == null)
            {
                return new BaseResponse<GetBriefWorksheetAttemptAnswerResponseModel>
                {
                    Success = false,
                    Message = "Worksheet attempt not found",
                };
            }

            var questionAnswer = await _context.QuestionAnswers.FirstOrDefaultAsync(x => x.Id == request.QuestionAnswerId);
            if (questionAnswer == null)
            {
                return new BaseResponse<GetBriefWorksheetAttemptAnswerResponseModel>
                {
                    Success = false,
                    Message = "Question answer not found",
                };
            }

            var worksheetAttemptAnswer = _mapper.Map<Domain.Entities.WorksheetAttemptAnswer>(request);
            var createWorksheetAttemptAnswerResult = await _context.AddAsync(worksheetAttemptAnswer, cancellationToken);

            if (createWorksheetAttemptAnswerResult.Entity == null)
            {
                return new BaseResponse<GetBriefWorksheetAttemptAnswerResponseModel>
                {
                    Success = false,
                    Message = "Create worksheet attempt answer failed",
                };
            }

            await _context.SaveChangesAsync(cancellationToken);

            var mappedWorksheetAttemptAnswerResult = _mapper.Map<GetBriefWorksheetAttemptAnswerResponseModel>(createWorksheetAttemptAnswerResult.Entity);

            return new BaseResponse<GetBriefWorksheetAttemptAnswerResponseModel>
            {
                Success = true,
                Message = "Create worksheet attempt answer successful",
                Data = mappedWorksheetAttemptAnswerResult
            };
        }
    }
}
