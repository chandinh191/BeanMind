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

namespace Application.Parents.Commands
{
    public sealed record DeleteParentCommand : IRequest<BaseResponse<GetBriefParentResponseModel>>
    {
        [Required]
        public Guid Id { get; init; }
    }

    public class DeleteParentCommandHanler : IRequestHandler<DeleteParentCommand, BaseResponse<GetBriefParentResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DeleteParentCommandHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetBriefParentResponseModel>> Handle(DeleteParentCommand request, CancellationToken cancellationToken)
        {
            var parent = await _context.Parents.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (parent == null)
            {
                return new BaseResponse<GetBriefParentResponseModel>
                {
                    Success = false,
                    Message = "Parent not found",
                };
            }

            parent.IsDeleted = true;

            var updateParentResult = _context.Update(parent);

            if (updateParentResult.Entity == null)
            {
                return new BaseResponse<GetBriefParentResponseModel>
                {
                    Success = false,
                    Message = "Delete parent failed",
                };
            }

            await _context.SaveChangesAsync(cancellationToken);

            var mappedParentResult = _mapper.Map<GetBriefParentResponseModel>(updateParentResult.Entity);

            return new BaseResponse<GetBriefParentResponseModel>
            {
                Success = true,
                Message = "Delete parent successful",
                Data = mappedParentResult
            };
        }
    }
}
