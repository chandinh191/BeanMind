using Application.Common;
using AutoMapper;
using Domain.Enums;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Enrollments.Commands
{
    public sealed record CheckAbleEnrollmentCommand : IRequest<BaseResponse<Object>>
    {
        [Required]
        public string ApplicationUserId { get; set; }
        [Required]
        public Guid CourseId { get; set; }
    }

    public class CheckAbleEnrollmentCommandHanler : IRequestHandler<CheckAbleEnrollmentCommand, BaseResponse<Object>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CheckAbleEnrollmentCommandHanler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<Object>> Handle(CheckAbleEnrollmentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var enrollment = await _context.Enrollments.FirstOrDefaultAsync(x => x.CourseId == request.CourseId
                    && x.ApplicationUserId == request.ApplicationUserId
                    && x.Status == EnrollmentStatus.OnGoing
                );

                if (enrollment == null)
                {
                    return new BaseResponse<Object>
                    {
                        Success = true,
                        Message = "true",
                        Data = true
                    };
                }

                return new BaseResponse<Object>
                {
                    Success = true,
                    Message = "false",
                    Data = false
                };
            }
            catch (Exception ex) {
                return new BaseResponse<Object>
                {
                    Success = true,
                    Message = "false",
                    Data = false
                };
            }
        }
    }
}
