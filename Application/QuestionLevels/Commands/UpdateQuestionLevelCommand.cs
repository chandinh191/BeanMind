using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Application.Common;
using AutoMapper;
using MediatR;
using Domain.Entities;

namespace Application.QuestionLevels.Commands;

[AutoMap(typeof(Domain.Entities.QuestionLevel), ReverseMap = true)]
public sealed record UpdateQuestionLevelCommand : IRequest<BaseResponse<GetBriefQuestionLevelResponseModel>>
{
    [Required]
    public Guid Id { get; init; }
    public string? Title { get; init; }
    public string? Description { get; set; }
}

public class UpdateQuestionLevelCommandHanler : IRequestHandler<UpdateQuestionLevelCommand, BaseResponse<GetBriefQuestionLevelResponseModel>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateQuestionLevelCommandHanler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse<GetBriefQuestionLevelResponseModel>> Handle(UpdateQuestionLevelCommand request, CancellationToken cancellationToken)
    {
        var questionLevel = await _context.QuestionLevels.FirstOrDefaultAsync(x => x.Id == request.Id);

        if(questionLevel == null)
        {
            return new BaseResponse<GetBriefQuestionLevelResponseModel>
            {
                Success = false,
                Message = "Question level is not found",
                Errors = ["Question level is not found"]
            };
        }

        //_mapper.Map(request, questionlevel);
        // Use reflection to update non-null properties
        foreach (var property in request.GetType().GetProperties())
        {
            var requestValue = property.GetValue(request);
            if (requestValue != null)
            {
                var targetProperty = questionLevel.GetType().GetProperty(property.Name);
                if (targetProperty != null)
                {
                    targetProperty.SetValue(questionLevel, requestValue);
                }
            }
        }

        var updateQuestionLevelResult = _context.Update(questionLevel);

        if (updateQuestionLevelResult.Entity == null)
        {
            return new BaseResponse<GetBriefQuestionLevelResponseModel>
            {
                Success = false,
                Message = "Update questionlevel failed",
            };
        }

        await _context.SaveChangesAsync(cancellationToken);

        var mappedQuestionLevelResult = _mapper.Map<GetBriefQuestionLevelResponseModel>(updateQuestionLevelResult.Entity);

        return new BaseResponse<GetBriefQuestionLevelResponseModel>
        {
            Success = true,
            Message = "Update questionlevel successful",
            Data = mappedQuestionLevelResult
        };
    }
}
