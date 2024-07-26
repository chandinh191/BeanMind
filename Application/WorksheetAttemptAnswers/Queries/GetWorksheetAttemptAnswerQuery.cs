using Application.Common;
using Application.Topics;
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

namespace Application.WorksheetAttemptAnswers.Queries
{
    public sealed record GetWorksheetAttemptAnswerQuery : IRequest<BaseResponse<GetWorksheetAttemptAnswerResponseModel>>
    {
        [Required]
        public Guid Id { get; init; }
    }

    public class GetWorksheetAttemptAnswerQueryHanler : IRequestHandler<GetWorksheetAttemptAnswerQuery, BaseResponse<GetWorksheetAttemptAnswerResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetWorksheetAttemptAnswerQueryHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetWorksheetAttemptAnswerResponseModel>> Handle(GetWorksheetAttemptAnswerQuery request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
            {
                return new BaseResponse<GetWorksheetAttemptAnswerResponseModel>
                {
                    Success = false,
                    Message = "Get worksheet attempt answer failed",
                    Errors = ["Id required"],
                };
            }

            var worksheetAttemptAnswer = await _context.WorksheetAttemptAnswers
                .Include(x => x.WorksheetAttempt)
                .Include(x => x.QuestionAnswer)
                .FirstOrDefaultAsync(x => x.Id.Equals(request.Id), cancellationToken);

            var mappedWorksheetAttemptAnswer = _mapper.Map<GetWorksheetAttemptAnswerResponseModel>(worksheetAttemptAnswer);

            return new BaseResponse<GetWorksheetAttemptAnswerResponseModel>
            {
                Success = true,
                Message = "Get worksheet attempt answer successful",
                Data = mappedWorksheetAttemptAnswer
            };
        }
    }
}
