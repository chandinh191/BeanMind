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

namespace Application.Teachers.Queries
{
    public sealed record GetTeacherQuery : IRequest<BaseResponse<GetTeacherResponseModel>>
    {
        [Required]
        public Guid Id { get; init; }
    }

    public class GetTeacherQueryHanler : IRequestHandler<GetTeacherQuery, BaseResponse<GetTeacherResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetTeacherQueryHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetTeacherResponseModel>> Handle(GetTeacherQuery request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
            {
                return new BaseResponse<GetTeacherResponseModel>
                {
                    Success = false,
                    Message = "Get teacher failed",
                    Errors = ["Id required"],
                };
            }

            var teacher = await _context.Teachers
                .Include(x => x.ApplicationUser)
                .FirstOrDefaultAsync(x => x.Id.Equals(request.Id), cancellationToken);
            var mappedTeacher = _mapper.Map<GetTeacherResponseModel>(teacher);

            return new BaseResponse<GetTeacherResponseModel>
            {
                Success = true,
                Message = "Get teacher successful",
                Data = mappedTeacher
            };
        }
    }
}
