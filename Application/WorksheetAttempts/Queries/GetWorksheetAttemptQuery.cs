using Application.Common;
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

namespace Application.WorksheetAttempts.Queries
{
    public sealed record GetWorksheetAttemptQuery : IRequest<BaseResponse<GetWorksheetAttemptResponseModel>>
    {
        [Required]
        public Guid Id { get; init; }
    }

    public class GetWorksheetAttemptQueryHanler : IRequestHandler<GetWorksheetAttemptQuery, BaseResponse<GetWorksheetAttemptResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetWorksheetAttemptQueryHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetWorksheetAttemptResponseModel>> Handle(GetWorksheetAttemptQuery request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
            {
                return new BaseResponse<GetWorksheetAttemptResponseModel>
                {
                    Success = false,
                    Message = "Get worksheet attempt failed",
                    Errors = ["Id required"],
                };
            }

            var worksheetAttempt = await _context.WorksheetAttempts
                .Include(x => x.Enrollment).ThenInclude(x => x.ApplicationUser)
                .Include(x => x.Worksheet)
                .Include(x => x.WorksheetAttemptAnswers).ThenInclude(o => o.QuestionAnswer).ThenInclude(o => o.Question)
                .FirstOrDefaultAsync(x => x.Id.Equals(request.Id), cancellationToken);

            var mappedWorksheetAttemptAnswer = _mapper.Map<GetWorksheetAttemptResponseModel>(worksheetAttempt);

            return new BaseResponse<GetWorksheetAttemptResponseModel>
            {
                Success = true,
                Message = "Get worksheet attempt successful",
                Data = mappedWorksheetAttemptAnswer
            };
        }
    }
}
