using Application.Common;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Infrastructure.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TeachingSlots.Queries
{
    public sealed record GetTeachingSlotQuery : IRequest<BaseResponse<GetTeachingSlotResponseModel>>
    {
        [Required]
        public Guid Id { get; init; }
    }

    public class GetTeachingSlotQueryHanler : IRequestHandler<GetTeachingSlotQuery, BaseResponse<GetTeachingSlotResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetTeachingSlotQueryHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetTeachingSlotResponseModel>> Handle(GetTeachingSlotQuery request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
            {
                return new BaseResponse<GetTeachingSlotResponseModel>
                {
                    Success = false,
                    Message = "Get teaching slot failed",
                    Errors = ["Id required"],
                };
            }

            var teachingSlot = await _context.TeachingSlots
                .Include(x => x.Course)
                .Include(x => x.Sessions)
                .FirstOrDefaultAsync(x => x.Id.Equals(request.Id), cancellationToken);
            var mappedTeachingSlot = _mapper.Map<GetTeachingSlotResponseModel>(teachingSlot);

            return new BaseResponse<GetTeachingSlotResponseModel>
            {
                Success = true,
                Message = "Get teaching slot successful",
                Data = mappedTeachingSlot
            };
        }
    }
}
