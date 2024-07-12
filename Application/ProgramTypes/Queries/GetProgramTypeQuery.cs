using Application.Common;
using Application.ProgramTypes;
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

namespace Application.ProgramTypes.Queries
{
    public sealed record GetProgramTypeQuery : IRequest<BaseResponse<GetProgramTypeResponseModel>>
    {
        [Required]
        public Guid Id { get; init; }
    }

    public class GetProgramTypeQueryHanler : IRequestHandler<GetProgramTypeQuery, BaseResponse<GetProgramTypeResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetProgramTypeQueryHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetProgramTypeResponseModel>> Handle(GetProgramTypeQuery request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
            {
                return new BaseResponse<GetProgramTypeResponseModel>
                {
                    Success = false,
                    Message = "Get program type failed",
                    Errors = ["Id required"],
                };
            }

            var programTypes = await _context.ProgramTypes
                .Include(o => o.Courses)
                .Include(o => o.Teachables)
                .FirstOrDefaultAsync(x => x.Id.Equals(request.Id), cancellationToken);

            var mappedProgramTypes = _mapper.Map<GetProgramTypeResponseModel>(programTypes);

            return new BaseResponse<GetProgramTypeResponseModel>
            {
                Success = true,
                Message = "Get program type successful",
                Data = mappedProgramTypes
            };
        }
    }
}
