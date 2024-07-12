using System.ComponentModel.DataAnnotations;
using Infrastructure.Data;
using Application.Common;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.QuestionAnswers.Commands;

[AutoMap(typeof(Domain.Entities.QuestionAnswer), ReverseMap = true)]
public sealed record CreateQuestionAnswerCommand : IRequest<BaseResponse<GetQuestionAnswerResponseModel>>
{
    [Required]
    public string Text { get; set; }

    [Required]
    public bool IsCorrect { get; set; }
    [Required]
    public Guid QuestionId { get; set; }
}

public class CreateQuestionAnswerCommandHanler : IRequestHandler<CreateQuestionAnswerCommand, BaseResponse<GetQuestionAnswerResponseModel>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateQuestionAnswerCommandHanler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse<GetQuestionAnswerResponseModel>> Handle(CreateQuestionAnswerCommand request, CancellationToken cancellationToken)
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

        var existedQuestionAnswer = await _context.QuestionAnswers.FirstOrDefaultAsync(x =>  x.QuestionId == request.QuestionId);
        if (existedQuestionAnswer != null)
        {
            return new BaseResponse<GetQuestionAnswerResponseModel>
            {
                Success = false,
                Message = "QuestionAnswer existed",
            };
        }

        var questionanswer = _mapper.Map<Domain.Entities.QuestionAnswer>(request);
        var createQuestionAnswerResult = await _context.AddAsync(questionanswer, cancellationToken);

        if(createQuestionAnswerResult.Entity == null)
        {
            return new BaseResponse<GetQuestionAnswerResponseModel>
            {
                Success = false,
                Message = "Create questionanswer failed",
            };
        }

        await _context.SaveChangesAsync(cancellationToken);

        var mappedQuestionAnswerResult = _mapper.Map<GetQuestionAnswerResponseModel>(createQuestionAnswerResult.Entity);

        return new BaseResponse<GetQuestionAnswerResponseModel>
        {
            Success = true,
            Message = "Create questionanswer successful",
            Data = mappedQuestionAnswerResult
        };
    }
}
