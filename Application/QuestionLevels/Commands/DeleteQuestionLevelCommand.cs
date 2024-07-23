using Application.Common;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Application.QuestionLevels.Commands;

public sealed record DeleteQuestionLevelCommand : IRequest<BaseResponse<GetBriefQuestionLevelResponseModel>>
{
    [Required]
    public Guid Id { get; init; }
}

public class DeleteQuestionLevelCommandHanler : IRequestHandler<DeleteQuestionLevelCommand, BaseResponse<GetBriefQuestionLevelResponseModel>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public DeleteQuestionLevelCommandHanler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse<GetBriefQuestionLevelResponseModel>> Handle(DeleteQuestionLevelCommand request, CancellationToken cancellationToken)
    {
        var questionLevel = await _context.QuestionLevels.FirstOrDefaultAsync(x => x.Id == request.Id);
        if(questionLevel == null)
        {
            return new BaseResponse<GetBriefQuestionLevelResponseModel>
            {
                Success = false,
                Message = "Question level not found",
            };
        }

        questionLevel.IsDeleted = true;

        var updateQuestionLevelResult = _context.Update(questionLevel);

        if (updateQuestionLevelResult.Entity == null)
        {
            return new BaseResponse<GetBriefQuestionLevelResponseModel>
            {
                Success = false,
                Message = "Delete question level failed",
            };
        }

        await _context.SaveChangesAsync(cancellationToken);

        var mappedQuestionLevelResult = _mapper.Map<GetBriefQuestionLevelResponseModel>(updateQuestionLevelResult.Entity);

        return new BaseResponse<GetBriefQuestionLevelResponseModel>
        {
            Success = true,
            Message = "Delete question level successful",
            Data = mappedQuestionLevelResult
        };
    }
}
