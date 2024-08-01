using Application.Common;
using Application.Sessions;
using AutoMapper;
using Infrastructure.Data;
using MediatR;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Students.Queries
{

    public sealed record GetStudentQuery : IRequest<BaseResponse<GetStudentResponseModel>>
    {
        [Required]
        public Guid Id { get; init; }
    }

    public class GetStudentQueryHanler : IRequestHandler<GetStudentQuery, BaseResponse<GetStudentResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetStudentQueryHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetStudentResponseModel>> Handle(GetStudentQuery request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
            {
                return new BaseResponse<GetStudentResponseModel>
                {
                    Success = false,
                    Message = "Get student failed",
                    Errors = ["Id required"],
                };
            }

            var student = await _context.Students
                .Include(x => x.ApplicationUser)
                .FirstOrDefaultAsync(x => x.Id.Equals(request.Id), cancellationToken);

            var mappedStudent = _mapper.Map<GetStudentResponseModel>(student);

            return new BaseResponse<GetStudentResponseModel>
            {
                Success = true,
                Message = "Get student successful",
                Data = mappedStudent
            };
        }
    }
}
