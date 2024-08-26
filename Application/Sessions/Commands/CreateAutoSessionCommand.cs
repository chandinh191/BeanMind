using Application.Common;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.UserEntities;
using Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Application.Sessions.Commands
{
    [AutoMap(typeof(Domain.Entities.Session), ReverseMap = true)]
    public sealed record CreateAutoSessionCommand : IRequest<BaseResponse<string>>
    {
        [Required]
        public DateTime From { get; set; }
        [Required]
        public DateTime To { get; set; }
        [Required]
        public string LecturerId { get; set; }
        [Required]
        public Guid CourseId { get; set; }
    }

    public class CreateAutoSessionCommandHanler : IRequestHandler<CreateAutoSessionCommand, BaseResponse<string>>
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public CreateAutoSessionCommandHanler(ApplicationDbContext context, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<BaseResponse<string>> Handle(CreateAutoSessionCommand request, CancellationToken cancellationToken)
        {
            var applicationUser = await _userManager.FindByIdAsync(request.LecturerId);
            if (applicationUser == null)
            {
                return new BaseResponse<string>
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
                    return new BaseResponse<string>
                    {
                        Success = false,
                        Message = "User is not a teacher to get this session",
                    };
                }
            }
            var course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == request.CourseId);
            if (course == null)
            {
                return new BaseResponse<string>
                {
                    Success = false,
                    Message = "Course is not found"
                };
            }

            var teachingSlots = await _context.TeachingSlots
                                              .Where(o => o.CourseId == request.CourseId)
                                              .Include(ts => ts.Course)
                                              .ThenInclude(course => course.Teachables)
                                              .Where(ts => ts.Course.Teachables.Any(teachable => teachable.ApplicationUserId == request.LecturerId && teachable.IsDeleted == false))
                                              .ToListAsync();

            if (teachingSlots == null || teachingSlots.Count == 0)
            {
                return new BaseResponse<string>
                {
                    Success = false,
                    Message = "Teaching slot not found",
                };
            }
            var sessions = await _context.Sessions
                .Include(o => o.TeachingSlot)
                                         .Where(o => o.Date >= request.From && o.Date <= request.To)
                                         .Where(o => o.ApplicationUserId == request.LecturerId)
                                         .Where(o => o.TeachingSlot.CourseId == request.CourseId)
                                         .ToListAsync();
            if(sessions != null && sessions.Count > 0)
            {
                return new BaseResponse<string>
                {
                    Success = false,
                    Message = "Existed session is created in " + request.From.ToString() + " to " + request.To.ToString()
                            +"\n plesae clear session in that range time to create"
                };
            }
            try
            {
                for (DateTime date = request.From; date <= request.To; date = date.AddDays(1))
                {
                    int dayIndex = (int)date.DayOfWeek;
                    var filterTeachingSlots = teachingSlots.Where(o => o.DayIndex == dayIndex).ToList();
                    foreach (var slot in filterTeachingSlots)
                    {
                        var session = new Session()
                        {
                            Date = date,
                            TeachingSlotId = slot.Id,
                            ApplicationUserId = request.LecturerId,
                        };
                        var createSessionResult = await _context.AddAsync(session, cancellationToken);
                    }
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<string>
                {
                    Success = false,
                    Message = "Create session failed: " + ex.Message,
                };
            }
            await _context.SaveChangesAsync(cancellationToken);

            return new BaseResponse<string>
            {
                Success = true,
                Message = "Create session successful"
            };
        }
    }
}
