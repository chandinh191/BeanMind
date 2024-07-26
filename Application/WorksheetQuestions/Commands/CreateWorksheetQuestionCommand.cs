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

namespace Application.WorksheetQuestions.Commands
{
    [AutoMap(typeof(Domain.Entities.WorksheetQuestion), ReverseMap = true)]
    public sealed record CreateWorksheetQuestionCommand : IRequest<BaseResponse<GetBriefWorksheetQuestionResponseModel>>
    {
        [Required]
        public Guid QuestionId { get; set; }
        [Required]
        public Guid WorksheetId { get; set; }
    }

    public class CreateWorksheetQuestionCommandHanler : IRequestHandler<CreateWorksheetQuestionCommand, BaseResponse<GetBriefWorksheetQuestionResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateWorksheetQuestionCommandHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetBriefWorksheetQuestionResponseModel>> Handle(CreateWorksheetQuestionCommand request, CancellationToken cancellationToken)
        {
            var question = await _context.Questions.FirstOrDefaultAsync(x => x.Id == request.QuestionId);
            if (question == null)
            {
                return new BaseResponse<GetBriefWorksheetQuestionResponseModel>
                {
                    Success = false,
                    Message = "Question not found",
                };
            }

            var worksheet = await _context.Worksheets.FirstOrDefaultAsync(x => x.Id == request.WorksheetId);
            if (worksheet == null)
            {
                return new BaseResponse<GetBriefWorksheetQuestionResponseModel>
                {
                    Success = false,
                    Message = "Worksheet not found",
                };
            }

            var worksheetQuestion = _mapper.Map<Domain.Entities.WorksheetQuestion>(request);
            var createWorksheetQuestionResult = await _context.AddAsync(worksheetQuestion, cancellationToken);

            if (createWorksheetQuestionResult.Entity == null)
            {
                return new BaseResponse<GetBriefWorksheetQuestionResponseModel>
                {
                    Success = false,
                    Message = "Create worksheet question failed",
                };
            }

            await _context.SaveChangesAsync(cancellationToken);

            var mappedWorksheetQuestionResult = _mapper.Map<GetBriefWorksheetQuestionResponseModel>(createWorksheetQuestionResult.Entity);

            return new BaseResponse<GetBriefWorksheetQuestionResponseModel>
            {
                Success = true,
                Message = "Create worksheet question successful",
                Data = mappedWorksheetQuestionResult
            };
        }
    }
}
