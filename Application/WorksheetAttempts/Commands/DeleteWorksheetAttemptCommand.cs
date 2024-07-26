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

namespace Application.WorksheetAttempts.Commands
{
    public sealed record DeleteWorksheetAttemptCommand : IRequest<BaseResponse<GetBriefWorksheetAttemptResponseModel>>
    {
        [Required]
        public Guid Id { get; init; }
    }

    public class DeleteWorksheetAttemptCommandHanler : IRequestHandler<DeleteWorksheetAttemptCommand, BaseResponse<GetBriefWorksheetAttemptResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DeleteWorksheetAttemptCommandHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetBriefWorksheetAttemptResponseModel>> Handle(DeleteWorksheetAttemptCommand request, CancellationToken cancellationToken)
        {
            var worksheetAttempt = await _context.WorksheetAttemptAnswers.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (worksheetAttempt == null)
            {
                return new BaseResponse<GetBriefWorksheetAttemptResponseModel>
                {
                    Success = false,
                    Message = "Worksheet attempt not found",
                };
            }

            worksheetAttempt.IsDeleted = true;

            var updateWorksheetAttemptResult = _context.Update(worksheetAttempt);

            if (updateWorksheetAttemptResult.Entity == null)
            {
                return new BaseResponse<GetBriefWorksheetAttemptResponseModel>
                {
                    Success = false,
                    Message = "Delete worksheet attempt failed",
                };
            }

            await _context.SaveChangesAsync(cancellationToken);

            var mappedWorksheetAttemptResult = _mapper.Map<GetBriefWorksheetAttemptResponseModel>(updateWorksheetAttemptResult.Entity);

            return new BaseResponse<GetBriefWorksheetAttemptResponseModel>
            {
                Success = true,
                Message = "Delete worksheet attempt successful",
                Data = mappedWorksheetAttemptResult
            };
        }
    }
}
