using Application.Enrollments;
using Application.Common;
using AutoMapper;
using Infrastructure.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Chapters;
using Microsoft.EntityFrameworkCore;

namespace Application.Enrollments.Queries
{
    public sealed record GetEnrollmentQuery : IRequest<BaseResponse<GetEnrollmentResponseModel>>
    {
        [Required]
        public Guid Id { get; init; }
    }

    public class GetEnrollmentQueryHanler : IRequestHandler<GetEnrollmentQuery, BaseResponse<GetEnrollmentResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetEnrollmentQueryHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetEnrollmentResponseModel>> Handle(GetEnrollmentQuery request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
            {
                return new BaseResponse<GetEnrollmentResponseModel>
                {
                    Success = false,
                    Message = "Get enrollment failed",
                    Errors = ["Id required"],
                };
            }

            var enrollment = await _context.Enrollments
                .Include(o => o.ApplicationUser)
                .Include(o => o.Course)
                .FirstOrDefaultAsync(x => x.Id.Equals(request.Id), cancellationToken);

            var mappedEnrollment= _mapper.Map<GetEnrollmentResponseModel>(enrollment);

            return new BaseResponse<GetEnrollmentResponseModel>
            {
                Success = true,
                Message = "Get enrollment successful",
                Data = mappedEnrollment
            };
        }
    }
}
