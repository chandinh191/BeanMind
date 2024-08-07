﻿using Application.Common;
using AutoMapper;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Application.Courses.Commands;

public sealed record DeleteCourseCommand : IRequest<BaseResponse<GetBriefCourseResponseModel>>
{
    [Required]
    public Guid Id { get; init; }
}

public class DeleteCourseCommandHanler : IRequestHandler<DeleteCourseCommand, BaseResponse<GetBriefCourseResponseModel>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public DeleteCourseCommandHanler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse<GetBriefCourseResponseModel>> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
    {
        var course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == request.Id);
        if(course == null)
        {
            return new BaseResponse<GetBriefCourseResponseModel>
            {
                Success = false,
                Message = "Course not found",
            };
        }
        course.IsDeleted = true;
        var updateCourseResult = _context.Update(course);

        if (updateCourseResult.Entity == null)
        {
            return new BaseResponse<GetBriefCourseResponseModel>
            {
                Success = false,
                Message = "Delete course failed",
            };
        }

        await _context.SaveChangesAsync(cancellationToken);

        var mappedCourseResult = _mapper.Map<GetBriefCourseResponseModel>(updateCourseResult.Entity);

        return new BaseResponse<GetBriefCourseResponseModel>
        {
            Success = true,
            Message = "Delete course successful",
            Data = mappedCourseResult
        };
    }
}
