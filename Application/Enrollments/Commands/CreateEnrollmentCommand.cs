﻿using Application.Enrollments;
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
using Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Domain.Enums;

namespace Application.Enrollments.Commands
{
    [AutoMap(typeof(Domain.Entities.Enrollment), ReverseMap = true)]
    public sealed record CreateEnrollmentCommand : IRequest<BaseResponse<GetBriefEnrollmentResponseModel>>
    {
        [Required]
        public string ApplicationUserId { get; set; }
        [Required]
        public Guid CourseId { get; set; }
        public EnrollmentStatus Status { get; set; }
    }

    public class CreateEnrollmentCommandHanler : IRequestHandler<CreateEnrollmentCommand, BaseResponse<GetBriefEnrollmentResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateEnrollmentCommandHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetBriefEnrollmentResponseModel>> Handle(CreateEnrollmentCommand request, CancellationToken cancellationToken)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == request.CourseId);
            if (course == null)
            {
                return new BaseResponse<GetBriefEnrollmentResponseModel>
                {
                    Success = false,
                    Message = "Course not found",
                };
            }


            var applicationUser = await _context.ApplicationUsers.FirstOrDefaultAsync(x => x.Id.Equals(request.ApplicationUserId));
            if (applicationUser == null)
            {
                return new BaseResponse<GetBriefEnrollmentResponseModel>
                {
                    Success = false,
                    Message = "User not found",
                };
            }

            var enrollment = _mapper.Map<Domain.Entities.Enrollment>(request);
            var createEnrollmentResult = await _context.AddAsync(enrollment, cancellationToken);

            if (createEnrollmentResult.Entity == null)
            {
                return new BaseResponse<GetBriefEnrollmentResponseModel>
                {
                    Success = false,
                    Message = "Create enrollment failed",
                };
            }

            await _context.SaveChangesAsync(cancellationToken);

            var mappedEnrollmentResult = _mapper.Map<GetBriefEnrollmentResponseModel>(createEnrollmentResult.Entity);

            return new BaseResponse<GetBriefEnrollmentResponseModel>
            {
                Success = true,
                Message = "Create enrollment successful",
                Data = mappedEnrollmentResult
            };
        }
    }
}
