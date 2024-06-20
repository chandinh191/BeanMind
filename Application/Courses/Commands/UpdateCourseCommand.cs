using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Application.Common;
using AutoMapper;
using MediatR;

namespace Application.Courses.Commands;

[AutoMap(typeof(Domain.Entities.Course), ReverseMap = true)]
public sealed record UpdateCourseCommand : IRequest<BaseResponse<GetCourseResponseModel>>
{
    [Required]
    public Guid Id { get; init; }
    [Required]
    [StringLength(maximumLength: 50, MinimumLength = 4, ErrorMessage = "Title must be at least 4 characters long.")]
    // //[RegularExpression(@"^(?:[A-Z][a-z0-9]*)(?: [A-Z][a-z0-9]*)*$", ErrorMessage = "Title must have the first word capitalized, following words separated by a space, and only contain characters and numbers.")]
    public string? Title { get; init; }
    [Required]
    public string? Description { get; init; }
    [Required]
    public Guid SubjectId { get; set; }
}

public class UpdateCourseCommandHanler : IRequestHandler<UpdateCourseCommand, BaseResponse<GetCourseResponseModel>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateCourseCommandHanler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse<GetCourseResponseModel>> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
    {
        var subject = await _context.Subject.FirstOrDefaultAsync(x => x.Id == request.SubjectId);

        if (subject == null)
        {
            return new BaseResponse<GetCourseResponseModel>
            {
                Success = false,
                Message = "Subject not found",
            };
        }

        var course = await _context.Course.FirstOrDefaultAsync(x => x.Id == request.Id);

        if(course == null)
        {
            return new BaseResponse<GetCourseResponseModel>
            {
                Success = false,
                Message = "Course is not found",
                Errors = ["Course is not found"]
            };
        }

        _mapper.Map(request, course);

        var updateCourseResult = _context.Update(course);

        if (updateCourseResult.Entity == null)
        {
            return new BaseResponse<GetCourseResponseModel>
            {
                Success = false,
                Message = "Update course failed",
            };
        }

        await _context.SaveChangesAsync(cancellationToken);

        var mappedCourseResult = _mapper.Map<GetCourseResponseModel>(updateCourseResult.Entity);

        return new BaseResponse<GetCourseResponseModel>
        {
            Success = true,
            Message = "Update course successful",
            Data = mappedCourseResult
        };
    }
}
