﻿using System.ComponentModel.DataAnnotations;
using Infrastructure.Data;
using Application.Common;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Teachables;
using Newtonsoft.Json;

namespace Application.Courses.Commands;


[AutoMap(typeof(Domain.Entities.Course), ReverseMap = true)]
public sealed record CreateCourseCommand : IRequest<BaseResponse<GetBriefCourseResponseModel>>
{
    [Required]
    public string Title { get; init; }
    public string ContentURL { get; set; }
    [Required]
    public string Description { get; init; }
    [Required]
    public int Price { get; set; }
    public string? ImageURL { get; set; }
    [Required]
    public int TotalSlot { get; set; }
    [Required]
    public Guid SubjectId { get; set; }
    [Required]
    public Guid ProgramTypeId { get; set; }
    [Required]
    public Guid CourseLevelId { get; set; }
    public List<CreateTeacherIdModel>? Teachables { get; set; }
    public bool IsDeleted { get; set; } = false;
}
public class CreateTeacherIdModel
{
    [Required]
    public string lecturerId { get; set; } 
}



public class CreateCourseCommandHanler : IRequestHandler<CreateCourseCommand, BaseResponse<GetBriefCourseResponseModel>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateCourseCommandHanler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse<GetBriefCourseResponseModel>> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
    {
        var subject = await _context.Subjects.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.SubjectId);
        if (subject == null)
        {
            return new BaseResponse<GetBriefCourseResponseModel>
            {
                Success = false,
                Message = "Subject not found",
            };
        }
        var programType = await _context.ProgramTypes.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.ProgramTypeId);
        if (programType == null)
        {
            return new BaseResponse<GetBriefCourseResponseModel>
            {
                Success = false,
                Message = "Program type not found",
            };
        }
        var courseLevel = await _context.CourseLevels.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.CourseLevelId);
        if (courseLevel == null)
        {
            return new BaseResponse<GetBriefCourseResponseModel>
            {
                Success = false,
                Message = "Course level not found",
            };
        }

        var course = new Domain.Entities.Course
        {
            Title = request.Title,
            ContentURL = request.ContentURL,
            Description = request.Description,
            Price = request.Price,
            ImageURL = request.ImageURL,
            TotalSlot = request.TotalSlot,
            SubjectId = request.SubjectId,
            ProgramTypeId = request.ProgramTypeId,
            CourseLevelId = request.CourseLevelId,
            IsDeleted = request.IsDeleted            
        };
        var createCourseResult = await _context.AddAsync(course, cancellationToken);

        if (createCourseResult.Entity == null)
        {
            return new BaseResponse<GetBriefCourseResponseModel>
            {
                Success = false,
                Message = "Create course failed",
            };
        }

        if(request.Teachables != null && request.Teachables.Count > 0)
        {
            foreach(var userId in request.Teachables)
            {
                var applicationUser = await _context.ApplicationUsers.FirstOrDefaultAsync(x => x.Id == userId.lecturerId);

                if (applicationUser == null)
                {
                    return new BaseResponse<GetBriefCourseResponseModel>
                    {
                        Success = false,
                        Message = "User not found",
                    };
                }

                var teachable = new Domain.Entities.Teachable
                {
                    CourseId = course.Id,
                    ApplicationUserId = userId.lecturerId
                };
                var createTeachableResult = await _context.AddAsync(teachable, cancellationToken);

                if (createTeachableResult.Entity == null)
                {
                    return new BaseResponse<GetBriefCourseResponseModel>
                    {
                        Success = false,
                        Message = "Create teachable failed",
                    };
                }
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        var mappedCourseResult = _mapper.Map<GetBriefCourseResponseModel>(createCourseResult.Entity);

        return new BaseResponse<GetBriefCourseResponseModel>
        {
            Success = true,
            Message = "Create course successful",
            Data = mappedCourseResult
        };
    }
}
