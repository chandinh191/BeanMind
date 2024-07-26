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

namespace Application.WorksheetQuestions.Queries
{
    public sealed record GetWorksheetQuestionQuery : IRequest<BaseResponse<GetWorksheetQuestionResponseModel>>
    {
        [Required]
        public Guid Id { get; init; }
    }

    public class GetWorksheetQuestionQueryHanler : IRequestHandler<GetWorksheetQuestionQuery, BaseResponse<GetWorksheetQuestionResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetWorksheetQuestionQueryHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetWorksheetQuestionResponseModel>> Handle(GetWorksheetQuestionQuery request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
            {
                return new BaseResponse<GetWorksheetQuestionResponseModel>
                {
                    Success = false,
                    Message = "Get worksheet question failed",
                    Errors = ["Id required"],
                };
            }

            var worksheetQuestion = await _context.WorksheetQuestions
                .Include(x => x.Question)
                .Include(x => x.Worksheet)
                .FirstOrDefaultAsync(x => x.Id.Equals(request.Id), cancellationToken);

            var mappedWorksheetQuestionAnswer = _mapper.Map<GetWorksheetQuestionResponseModel>(worksheetQuestion);

            return new BaseResponse<GetWorksheetQuestionResponseModel>
            {
                Success = true,
                Message = "Get worksheet question successful",
                Data = mappedWorksheetQuestionAnswer
            };
        }
    }
}
