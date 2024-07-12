using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Application.Common;
using AutoMapper;
using MediatR;

namespace Application.QuestionAnswers.Commands;

[AutoMap(typeof(Domain.Entities.QuestionAnswer), ReverseMap = true)]
public sealed record UpdateQuestionAnswerCommand : IRequest<BaseResponse<GetQuestionAnswerResponseModel>>
{
    [Required]
    public Guid Id { get; init; }
    [Required]
    public string Text { get; set; }
    [Required]
    public int OrderIndex { get; set; }
    [Required]
    public bool IsCorrect { get; set; }
    [Required]
    public Guid QuestionId { get; set; }
}

public class UpdateQuestionAnswerCommandHanler : IRequestHandler<UpdateQuestionAnswerCommand, BaseResponse<GetQuestionAnswerResponseModel>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateQuestionAnswerCommandHanler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse<GetQuestionAnswerResponseModel>> Handle(UpdateQuestionAnswerCommand request, CancellationToken cancellationToken)
    {
        var question = await _context.Questions.FirstOrDefaultAsync(x => x.Id == request.QuestionId);

        if (question == null)
        {
            return new BaseResponse<GetQuestionAnswerResponseModel>
            {
                Success = false,
                Message = "Question not found",
            };
        }

        var existedQuestionAnswer = await _context.QuestionAnswers.FirstOrDefaultAsync(x => x.QuestionId == request.QuestionId &&  x.Id != request.Id);
        if (existedQuestionAnswer != null)
        {
            return new BaseResponse<GetQuestionAnswerResponseModel>
            {
                Success = false,
                Message = "QuestionAnswer existed",
            };
        }

        var questionanswer = await _context.QuestionAnswers.FirstOrDefaultAsync(x => x.Id == request.Id);

        if(questionanswer == null)
        {
            return new BaseResponse<GetQuestionAnswerResponseModel>
            {
                Success = false,
                Message = "QuestionAnswer is not found",
                Errors = ["QuestionAnswer is not found"]
            };
        }

        _mapper.Map(request, questionanswer);

        var updateQuestionAnswerResult = _context.Update(questionanswer);

        if (updateQuestionAnswerResult.Entity == null)
        {
            return new BaseResponse<GetQuestionAnswerResponseModel>
            {
                Success = false,
                Message = "Update questionanswer failed",
            };
        }

        await _context.SaveChangesAsync(cancellationToken);

        var mappedQuestionAnswerResult = _mapper.Map<GetQuestionAnswerResponseModel>(updateQuestionAnswerResult.Entity);

        return new BaseResponse<GetQuestionAnswerResponseModel>
        {
            Success = true,
            Message = "Update questionanswer successful",
            Data = mappedQuestionAnswerResult
        };
    }
}
