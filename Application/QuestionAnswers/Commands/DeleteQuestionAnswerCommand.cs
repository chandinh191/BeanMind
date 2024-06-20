using Application.Common;
using AutoMapper;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Application.QuestionAnswers.Commands;

public sealed record DeleteQuestionAnswerCommand : IRequest<BaseResponse<GetQuestionAnswerResponseModel>>
{
    [Required]
    public Guid Id { get; init; }
}

public class DeleteQuestionAnswerCommandHanler : IRequestHandler<DeleteQuestionAnswerCommand, BaseResponse<GetQuestionAnswerResponseModel>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public DeleteQuestionAnswerCommandHanler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse<GetQuestionAnswerResponseModel>> Handle(DeleteQuestionAnswerCommand request, CancellationToken cancellationToken)
    {
        var questionanswer = await _context.QuestionAnswer.FirstOrDefaultAsync(x => x.Id == request.Id);
        if(questionanswer == null)
        {
            return new BaseResponse<GetQuestionAnswerResponseModel>
            {
                Success = false,
                Message = "QuestionAnswer not found",
            };
        }

        questionanswer.IsDeleted = true;

        var updateQuestionAnswerResult = _context.Update(questionanswer);

        if (updateQuestionAnswerResult.Entity == null)
        {
            return new BaseResponse<GetQuestionAnswerResponseModel>
            {
                Success = false,
                Message = "Delete questionanswer failed",
            };
        }

        await _context.SaveChangesAsync(cancellationToken);

        var mappedQuestionAnswerResult = _mapper.Map<GetQuestionAnswerResponseModel>(updateQuestionAnswerResult.Entity);

        return new BaseResponse<GetQuestionAnswerResponseModel>
        {
            Success = true,
            Message = "Delete questionanswer successful",
            Data = mappedQuestionAnswerResult
        };
    }
}
