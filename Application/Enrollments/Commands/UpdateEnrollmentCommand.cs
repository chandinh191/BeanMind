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
    [AutoMap(typeof(Domain.Entities.Enrollment), ReverseMap = true)]
    public sealed record UpdateEnrollmentCommand : IRequest<BaseResponse<GetEnrollmentResponseModel>>
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string ApplicationUserId { get; set; }
        [Required]
        public Guid CourseId { get; set; }
        public bool Status { get; set; }
    }

    public class UpdateEnrollmentCommandHanler : IRequestHandler<UpdateEnrollmentCommand, BaseResponse<GetEnrollmentResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateEnrollmentCommandHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetEnrollmentResponseModel>> Handle(UpdateEnrollmentCommand request, CancellationToken cancellationToken)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == request.CourseId);
            if (course == null)
            {
                return new BaseResponse<GetEnrollmentResponseModel>
                {
                    Success = false,
                    Message = "Course not found",
                };
            }


            var applicationUser = await _context.ApplicationUsers.FirstOrDefaultAsync(x => x.Id.Equals(request.ApplicationUserId));
            if (course == null)
            {
                return new BaseResponse<GetEnrollmentResponseModel>
                {
                    Success = false,
                    Message = "User not found",
                };
            }

            var enrollment = await _context.Enrollments.FirstOrDefaultAsync(x => x.Id == request.Id);

            if (enrollment == null)
            {
                return new BaseResponse<GetEnrollmentResponseModel>
                {
                    Success = false,
                    Message = "Enrollment is not found",
                    Errors = ["Enrollment is not found"]
                };
            }

            _mapper.Map(request, enrollment);

            var updateEnrollmentResult = _context.Update(enrollment);

            if (updateEnrollmentResult.Entity == null)
            {
                return new BaseResponse<GetEnrollmentResponseModel>
                {
                    Success = false,
                    Message = "Update Enrollment failed",
                };
            }

            await _context.SaveChangesAsync(cancellationToken);

            var mappedEnrollmentResult = _mapper.Map<GetEnrollmentResponseModel>(updateEnrollmentResult.Entity);

            return new BaseResponse<GetEnrollmentResponseModel>
            {
                Success = true,
                Message = "Update enrollment successful",
                Data = mappedEnrollmentResult
            };
        }
    }

}
