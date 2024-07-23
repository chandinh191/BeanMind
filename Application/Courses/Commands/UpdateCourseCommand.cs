using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Application.Common;
using AutoMapper;
using MediatR;
using Domain.Entities;

namespace Application.Courses.Commands;

[AutoMap(typeof(Domain.Entities.Course), ReverseMap = true)]
public sealed record UpdateCourseCommand : IRequest<BaseResponse<GetBriefCourseResponseModel>>
{
    [Required]
    public Guid Id { get; init; }
    [StringLength(maximumLength: 50, MinimumLength = 4, ErrorMessage = "Title must be at least 4 characters long.")]
    public string? Title { get; init; }
    public string? Description { get; init; }
    public Guid? SubjectId { get; set; }
    public Guid? ProgramTypeId { get; set; }
    public Guid? CourseLevelId { get; set; }
}

public class UpdateCourseCommandHanler : IRequestHandler<UpdateCourseCommand, BaseResponse<GetBriefCourseResponseModel>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateCourseCommandHanler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse<GetBriefCourseResponseModel>> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
    {
        if (request.SubjectId != null)
        {
            var subject = await _context.Subjects.FirstOrDefaultAsync(x => x.Id == request.SubjectId);
            if (subject == null)
            {
                return new BaseResponse<GetBriefCourseResponseModel>
                {
                    Success = false,
                    Message = "Subject not found",
                };
            }
        }
        if (request.ProgramTypeId != null)
        {
            var programType = await _context.ProgramTypes.FirstOrDefaultAsync(x => x.Id == request.ProgramTypeId);
            if (programType == null)
            {
                return new BaseResponse<GetBriefCourseResponseModel>
                {
                    Success = false,
                    Message = "Program type not found",
                };
            }
        }
        if (request.CourseLevelId != null)
        {
            var courseLevel = await _context.CourseLevels.FirstOrDefaultAsync(x => x.Id == request.CourseLevelId);
            if (courseLevel == null)
            {
                return new BaseResponse<GetBriefCourseResponseModel>
                {
                    Success = false,
                    Message = "Course level not found",
                };
            }
        }

        var course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == request.Id);

        if(course == null)
        {
            return new BaseResponse<GetBriefCourseResponseModel>
            {
                Success = false,
                Message = "Course is not found",
                Errors = ["Course is not found"]
            };
        }

        //_mapper.Map(request, course);
        // Use reflection to update non-null properties
        foreach (var property in request.GetType().GetProperties())
        {
            var requestValue = property.GetValue(request);
            if (requestValue != null)
            {
                var targetProperty = course.GetType().GetProperty(property.Name);
                if (targetProperty != null)
                {
                    targetProperty.SetValue(course, requestValue);
                }
            }
        }

        var updateCourseResult = _context.Update(course);

        if (updateCourseResult.Entity == null)
        {
            return new BaseResponse<GetBriefCourseResponseModel>
            {
                Success = false,
                Message = "Update course failed",
            };
        }

        await _context.SaveChangesAsync(cancellationToken);

        var mappedCourseResult = _mapper.Map<GetBriefCourseResponseModel>(updateCourseResult.Entity);

        return new BaseResponse<GetBriefCourseResponseModel>
        {
            Success = true,
            Message = "Update course successful",
            Data = mappedCourseResult
        };
    }
}
