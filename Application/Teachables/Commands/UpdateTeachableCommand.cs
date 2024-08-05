using Application.Common;
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
    public sealed record UpdateTeachableCommand : IRequest<BaseResponse<GetBriefTeachableResponseModel>>
    {
        [Required]
        public Guid Id { get; init; }
        public string? LecturerId { get; set; }
        public Guid? CourseId { get; set; }
    }

    public class UpdateTeachableCommandHanler : IRequestHandler<UpdateTeachableCommand, BaseResponse<GetBriefTeachableResponseModel>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public UpdateTeachableCommandHanler(ApplicationDbContext context, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<BaseResponse<GetBriefTeachableResponseModel>> Handle(UpdateTeachableCommand request, CancellationToken cancellationToken)
        {
            if (request.CourseId != null)
            {
                var course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == request.CourseId);
                if (course == null)
                {
                    return new BaseResponse<GetBriefTeachableResponseModel>
                    {
                        Success = false,
                        Message = "Course not found",
                    };
                }
            }
            if (request.LecturerId != null)
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
            }

            var teachable = await _context.Teachables.FirstOrDefaultAsync(x => x.Id == request.Id);

            if (teachable == null)
            {
                return new BaseResponse<GetBriefTeachableResponseModel>
                {
                    Success = false,
                    Message = "Teachable is not found",
                    Errors = ["Teachable is not found"]
                };
            }

            //_mapper.Map(request, question);
            // Use reflection to update non-null properties
            foreach (var property in request.GetType().GetProperties())
            {
                var requestValue = property.GetValue(request);
                if (requestValue != null)
                {
                    var targetProperty = teachable.GetType().GetProperty(property.Name);
                    if (targetProperty != null)
                    {
                        targetProperty.SetValue(teachable, requestValue);
                    }
                }
            }

            var updateTeachableResult = _context.Update(teachable);

            if (updateTeachableResult.Entity == null)
            {
                return new BaseResponse<GetBriefTeachableResponseModel>
                {
                    Success = false,
                    Message = "Update teachable failed",
                };
            }

            await _context.SaveChangesAsync(cancellationToken);

            var mappedTeachableResult = _mapper.Map<GetBriefTeachableResponseModel>(updateTeachableResult.Entity);

            return new BaseResponse<GetBriefTeachableResponseModel>
            {
                Success = true,
                Message = "Update teachable successful",
                Data = mappedTeachableResult
            };
        }
    }
}
