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
    public sealed record DeleteWorksheetQuestionCommand : IRequest<BaseResponse<GetBriefWorksheetQuestionResponseModel>>
    {
        [Required]
        public Guid Id { get; init; }
    }

    public class DeleteWorksheetQuestionCommandHanler : IRequestHandler<DeleteWorksheetQuestionCommand, BaseResponse<GetBriefWorksheetQuestionResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DeleteWorksheetQuestionCommandHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetBriefWorksheetQuestionResponseModel>> Handle(DeleteWorksheetQuestionCommand request, CancellationToken cancellationToken)
        {
            var worksheetQuestion = await _context.WorksheetQuestions.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (worksheetQuestion == null)
            {
                return new BaseResponse<GetBriefWorksheetQuestionResponseModel>
                {
                    Success = false,
                    Message = "Worksheet question not found",
                };
            }

            worksheetQuestion.IsDeleted = true;

            var updateWorksheetQuestionResult = _context.Update(worksheetQuestion);

            if (updateWorksheetQuestionResult.Entity == null)
            {
                return new BaseResponse<GetBriefWorksheetQuestionResponseModel>
                {
                    Success = false,
                    Message = "Delete worksheet question failed",
                };
            }

            await _context.SaveChangesAsync(cancellationToken);

            var mappedWorksheetQuestionResult = _mapper.Map<GetBriefWorksheetQuestionResponseModel>(updateWorksheetQuestionResult.Entity);

            return new BaseResponse<GetBriefWorksheetQuestionResponseModel>
            {
                Success = true,
                Message = "Delete worksheet question successful",
                Data = mappedWorksheetQuestionResult
            };
        }
    }
}
