﻿using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Application.Common;
using AutoMapper;
using MediatR;
using Domain.Entities;

namespace Application.Topics.Commands;

[AutoMap(typeof(Domain.Entities.Topic), ReverseMap = true)]
public sealed record UpdateTopicCommand : IRequest<BaseResponse<GetBriefTopicResponseModel>>
{
    [Required]
    public Guid Id { get; init; }
    [Required]
    [StringLength(maximumLength: 50, MinimumLength = 4, ErrorMessage = "Title must be at least 4 characters long.")]
    //[RegularExpression(@"^(?:[A-Z][a-z0-9]*)(?: [A-Z][a-z0-9]*)*$", ErrorMessage = "Title must have the first word capitalized, following words separated by a space, and only contain characters and numbers.")]
    public string? Title { get; init; }
    [Required]
    public string? Description { get; init; }
    [Required]
    public Guid ChapterId { get; set; }
}

public class UpdateTopicCommandHanler : IRequestHandler<UpdateTopicCommand, BaseResponse<GetBriefTopicResponseModel>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateTopicCommandHanler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse<GetBriefTopicResponseModel>> Handle(UpdateTopicCommand request, CancellationToken cancellationToken)
    {
        var chapter = await _context.Chapters.FirstOrDefaultAsync(x => x.Id == request.ChapterId);

        if (chapter == null)
        {
            return new BaseResponse<GetBriefTopicResponseModel>
            {
                Success = false,
                Message = "Chapter not found",
            };
        }

        var topic = await _context.Topics.FirstOrDefaultAsync(x => x.Id == request.Id);

        if(topic == null)
        {
            return new BaseResponse<GetBriefTopicResponseModel>
            {
                Success = false,
                Message = "Topic is not found",
                Errors = ["Topic is not found"]
            };
        }

        //_mapper.Map(request, topic);
        // Use reflection to update non-null properties
        foreach (var property in request.GetType().GetProperties())
        {
            var requestValue = property.GetValue(request);
            if (requestValue != null)
            {
                var targetProperty = topic.GetType().GetProperty(property.Name);
                if (targetProperty != null)
                {
                    targetProperty.SetValue(topic, requestValue);
                }
            }
        }

        var updateTopicResult = _context.Update(topic);

        if (updateTopicResult.Entity == null)
        {
            return new BaseResponse<GetBriefTopicResponseModel>
            {
                Success = false,
                Message = "Update topic failed",
            };
        }

        await _context.SaveChangesAsync(cancellationToken);

        var mappedTopicResult = _mapper.Map<GetBriefTopicResponseModel>(updateTopicResult.Entity);

        return new BaseResponse<GetBriefTopicResponseModel>
        {
            Success = true,
            Message = "Update topic successful",
            Data = mappedTopicResult
        };
    }
}
