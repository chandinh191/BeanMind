using Application.Common;
using Microsoft.EntityFrameworkCore;
using Application.Subjects;
using AutoMapper;
using Infrastructure.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Teachables.Queries
{
    public sealed record GetTeachableQuery : IRequest<BaseResponse<GetTeachableResponseModel>>
    {
        [Required]
        public Guid Id { get; init; }
    }

    public class GetTeachableQueryHanler : IRequestHandler<GetTeachableQuery, BaseResponse<GetTeachableResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetTeachableQueryHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetTeachableResponseModel>> Handle(GetTeachableQuery request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
            {
                return new BaseResponse<GetTeachableResponseModel>
                {
                    Success = false,
                    Message = "Get teachable failed",
                    Errors = ["Id required"],
                };
            }

            var teachable = await _context.Teachables
                .Include(x => x.Course)
                .Include(x => x.ApplicationUser)
                .FirstOrDefaultAsync(x => x.Id.Equals(request.Id), cancellationToken);
            var mappedTeachable = _mapper.Map<GetTeachableResponseModel>(teachable);

            return new BaseResponse<GetTeachableResponseModel>
            {
                Success = true,
                Message = "Get teachable successful",
                Data = mappedTeachable
            };
        }
    }

}
