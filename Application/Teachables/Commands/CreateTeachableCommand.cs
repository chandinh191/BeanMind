using Application.Common;
using Application.Sessions;
using AutoMapper;
using Domain.Entities.UserEntities;
using Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Teachables.Commands
{
    [AutoMap(typeof(Domain.Entities.Teachable), ReverseMap = true)]
    public sealed record CreateTeachableCommand : IRequest<BaseResponse<GetBriefTeachableResponseModel>>
    {
        [Required]
        public string LecturerId { get; set; }
        [Required]
        public Guid CourseId { get; set; }
    }

    public class CreateTeachableCommandHanler : IRequestHandler<CreateTeachableCommand, BaseResponse<GetBriefTeachableResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public CreateTeachableCommandHanler(ApplicationDbContext context, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<BaseResponse<GetBriefTeachableResponseModel>> Handle(CreateTeachableCommand request, CancellationToken cancellationToken)
        {
            var applicationUser = await _userManager.FindByIdAsync(request.LecturerId);
            if (applicationUser == null)
            {
                return new BaseResponse<GetBriefTeachableResponseModel>
                {
                    Success = false,
                    Message = "User not found",
                };
            }
            else
            {
                var isTeacher = await _userManager.IsInRoleAsync(applicationUser, "Teacher");
                if (!isTeacher)
                {
                    return new BaseResponse<GetBriefTeachableResponseModel>
                    {
                        Success = false,
                        Message = "User is not a teacher to get this session",
                    };
                }
            }

            var course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == request.CourseId);

            if (course == null)
            {
                return new BaseResponse<GetBriefTeachableResponseModel>
                {
                    Success = false,
                    Message = "Course not found",
                };
            }
            var teachable = _mapper.Map<Domain.Entities.Teachable>(request);
            var createTeachableResult = await _context.AddAsync(teachable, cancellationToken);

            if (createTeachableResult.Entity == null)
            {
                return new BaseResponse<GetBriefTeachableResponseModel>
                {
                    Success = false,
                    Message = "Create teachable failed",
                };
            }

            await _context.SaveChangesAsync(cancellationToken);

            var mappedTeachableResult = _mapper.Map<GetBriefTeachableResponseModel>(createTeachableResult.Entity);

            return new BaseResponse<GetBriefTeachableResponseModel>
            {
                Success = true,
                Message = "Create teachable successful",
                Data = mappedTeachableResult
            };
        }
    }
}
