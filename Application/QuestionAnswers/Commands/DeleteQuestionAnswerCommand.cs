using Application.Common;
using AutoMapper;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Application.QuestionAnswers.Commands;

public sealed record DeleteQuestionAnswerCommand : IRequest<BaseResponse<GetBriefQuestionAnswerResponseModel>>
{
    [Required]
    public Guid Id { get; init; }
}

public class DeleteQuestionAnswerCommandHanler : IRequestHandler<DeleteQuestionAnswerCommand, BaseResponse<GetBriefQuestionAnswerResponseModel>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public DeleteQuestionAnswerCommandHanler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse<GetBriefQuestionAnswerResponseModel>> Handle(DeleteQuestionAnswerCommand request, CancellationToken cancellationToken)
    {
        var questionAnswer = await _context.QuestionAnswers.FirstOrDefaultAsync(x => x.Id == request.Id);
        if(questionAnswer == null)
        {
            return new BaseResponse<GetBriefQuestionAnswerResponseModel>
            {
                Success = false,
                Message = "QuestionAnswer not found",
            };
        }

        questionAnswer.IsDeleted = true;

        var updateQuestionAnswerResult = _context.Update(questionAnswer);

        if (updateQuestionAnswerResult.Entity == null)
        {
            return new BaseResponse<GetBriefQuestionAnswerResponseModel>
            {
                Success = false,
                Message = "Delete question answer failed",
            };
        }

        await _context.SaveChangesAsync(cancellationToken);

        var mappedQuestionAnswerResult = _mapper.Map<GetBriefQuestionAnswerResponseModel>(updateQuestionAnswerResult.Entity);

        return new BaseResponse<GetBriefQuestionAnswerResponseModel>
        {
            Success = true,
            Message = "Delete question answer successful",
            Data = mappedQuestionAnswerResult
        };
    }
}
