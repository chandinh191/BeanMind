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
    public sealed record DeleteWorksheetAttemptAnswerCommand : IRequest<BaseResponse<GetBriefWorksheetAttemptAnswerResponseModel>>
    {
        [Required]
        public Guid Id { get; init; }
    }

    public class DeleteWorksheetAttemptAnswerCommandHanler : IRequestHandler<DeleteWorksheetAttemptAnswerCommand, BaseResponse<GetBriefWorksheetAttemptAnswerResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DeleteWorksheetAttemptAnswerCommandHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetBriefWorksheetAttemptAnswerResponseModel>> Handle(DeleteWorksheetAttemptAnswerCommand request, CancellationToken cancellationToken)
        {
            var worksheetAttemptAnswer = await _context.WorksheetAttemptAnswers.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (worksheetAttemptAnswer == null)
            {
                return new BaseResponse<GetBriefWorksheetAttemptAnswerResponseModel>
                {
                    Success = false,
                    Message = "Worksheet attempt answer not found",
                };
            }

            worksheetAttemptAnswer.IsDeleted = true;

            var updateWorksheetAttemptAnswerResult = _context.Update(worksheetAttemptAnswer);

            if (updateWorksheetAttemptAnswerResult.Entity == null)
            {
                return new BaseResponse<GetBriefWorksheetAttemptAnswerResponseModel>
                {
                    Success = false,
                    Message = "Delete worksheet attempt answer failed",
                };
            }

            await _context.SaveChangesAsync(cancellationToken);

            var mappedWorksheetAttemptAnswerResult = _mapper.Map<GetBriefWorksheetAttemptAnswerResponseModel>(updateWorksheetAttemptAnswerResult.Entity);

            return new BaseResponse<GetBriefWorksheetAttemptAnswerResponseModel>
            {
                Success = true,
                Message = "Delete worksheet attempt answer successful",
                Data = mappedWorksheetAttemptAnswerResult
            };
        }
    }
}
