using Application.Common;
using Application.Participants;
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

namespace Application.Parents.Queries
{
    public sealed record GetParentQuery : IRequest<BaseResponse<GetParentResponseModel>>
    {
        [Required]
        public Guid Id { get; init; }
    }

    public class GetParentQueryHanler : IRequestHandler<GetParentQuery, BaseResponse<GetParentResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetParentQueryHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetParentResponseModel>> Handle(GetParentQuery request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
            {
                return new BaseResponse<GetParentResponseModel>
                {
                    Success = false,
                    Message = "Get parent failed",
                    Errors = ["Id required"],
                };
            }

            var parent = await _context.Parents
                .Include(o => o.ApplicationUser)
                .Include(o => o.Students)
                .FirstOrDefaultAsync(x => x.Id.Equals(request.Id), cancellationToken);

            var mappedParent = _mapper.Map<GetParentResponseModel>(parent);

            return new BaseResponse<GetParentResponseModel>
            {
                Success = true,
                Message = "Get parent successful",
                Data = mappedParent
            };
        }
    }
}
