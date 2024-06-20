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
using Microsoft.EntityFrameworkCore;

namespace Application.Enrollments.Commands
{
    public sealed record DeleteEnrollmentCommand : IRequest<BaseResponse<GetEnrollmentResponseModel>>
    {
        [Required]
        public Guid Id { get; init; }
    }

    public class DeleteEnrollmentCommandHanler : IRequestHandler<DeleteEnrollmentCommand, BaseResponse<GetEnrollmentResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DeleteEnrollmentCommandHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetEnrollmentResponseModel>> Handle(DeleteEnrollmentCommand request, CancellationToken cancellationToken)
        {
            var enrollment = await _context.Enrollment.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (enrollment == null)
            {
                return new BaseResponse<GetEnrollmentResponseModel>
                {
                    Success = false,
                    Message = "Enrollment not found",
                };
            }
            enrollment.IsDeleted = true;


            var updateEnrollmentResult = _context.Update(enrollment);

            if (updateEnrollmentResult.Entity == null)
            {
                return new BaseResponse<GetEnrollmentResponseModel>
                {
                    Success = false,
                    Message = "Delete enrollment failed",
                };
            }

            await _context.SaveChangesAsync(cancellationToken);

            var mappedEnrollmentResult = _mapper.Map<GetEnrollmentResponseModel>(updateEnrollmentResult.Entity);

            return new BaseResponse<GetEnrollmentResponseModel>
            {
                Success = true,
                Message = "Delete enrollment successful",
                Data = mappedEnrollmentResult
            };
        }
    }
}
