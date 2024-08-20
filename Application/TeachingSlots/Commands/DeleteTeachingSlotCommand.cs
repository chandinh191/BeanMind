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

namespace Application.TeachingSlots.Commands
{
    public sealed record DeleteTeachingSlotCommand : IRequest<BaseResponse<GetBriefTeachingSlotResponseModel>>
    {
        [Required]
        public Guid Id { get; init; }
    }

    public class DeleteTeachingSlotCommandHanler : IRequestHandler<DeleteTeachingSlotCommand, BaseResponse<GetBriefTeachingSlotResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DeleteTeachingSlotCommandHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetBriefTeachingSlotResponseModel>> Handle(DeleteTeachingSlotCommand request, CancellationToken cancellationToken)
        {
            var teachingSlot = await _context.TeachingSlots.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (teachingSlot == null)
            {
                return new BaseResponse<GetBriefTeachingSlotResponseModel>
                {
                    Success = false,
                    Message = "Teaching slot  not found",
                };
            }

            teachingSlot.IsDeleted = true;

            var updateTeachingSlotResult = _context.Update(teachingSlot);

            if (updateTeachingSlotResult.Entity == null)
            {
                return new BaseResponse<GetBriefTeachingSlotResponseModel>
                {
                    Success = false,
                    Message = "Delete teaching slot failed",
                };
            }

            await _context.SaveChangesAsync(cancellationToken);

            var mappedTeachingSlotResult = _mapper.Map<GetBriefTeachingSlotResponseModel>(updateTeachingSlotResult.Entity);

            return new BaseResponse<GetBriefTeachingSlotResponseModel>
            {
                Success = true,
                Message = "Delete teaching slot successful",
                Data = mappedTeachingSlotResult
            };
        }
    }
}
