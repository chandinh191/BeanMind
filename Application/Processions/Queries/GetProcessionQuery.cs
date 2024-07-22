using Application.Common;
using Application.Processions;
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

namespace Application.Processions.Queries
{
    public sealed record GetProcessionQuery : IRequest<BaseResponse<GetProcessionResponseModel>>
    {
        [Required]
        public Guid Id { get; init; }
    }

    public class GetProcessionQueryHanler : IRequestHandler<GetProcessionQuery, BaseResponse<GetProcessionResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetProcessionQueryHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetProcessionResponseModel>> Handle(GetProcessionQuery request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
            {
                return new BaseResponse<GetProcessionResponseModel>
                {
                    Success = false,
                    Message = "Get procession failed",
                    Errors = ["Id required"],
                };
            }

            var procession = await _context.Processions
                .Include(o => o.Participant)
                .Include(o => o.Topic)
                .FirstOrDefaultAsync(x => x.Id.Equals(request.Id), cancellationToken);

            var mappedProcession = _mapper.Map<GetProcessionResponseModel>(procession);

            return new BaseResponse<GetProcessionResponseModel>
            {
                Success = true,
                Message = "Get procession successful",
                Data = mappedProcession
            };
        }
    }
}
