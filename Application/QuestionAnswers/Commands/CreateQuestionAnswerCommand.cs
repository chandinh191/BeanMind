using System.ComponentModel.DataAnnotations;
using Infrastructure.Data;
using Application.Common;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.QuestionAnswers.Commands;

[AutoMap(typeof(Domain.Entities.QuestionAnswer), ReverseMap = true)]
public sealed record CreateQuestionAnswerCommand : IRequest<BaseResponse<GetBriefQuestionAnswerResponseModel>>
{
    [Required]
    public Guid QuestionId { get; set; }
    [Required]
    public string Text { get; set; }
    [Required]
    public bool IsCorrect { get; set; }
}

public class CreateQuestionAnswerCommandHanler : IRequestHandler<CreateQuestionAnswerCommand, BaseResponse<GetBriefQuestionAnswerResponseModel>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateQuestionAnswerCommandHanler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse<GetBriefQuestionAnswerResponseModel>> Handle(CreateQuestionAnswerCommand request, CancellationToken cancellationToken)
    {
        var question = await _context.Questions.FirstOrDefaultAsync(x => x.Id == request.QuestionId);

        if (question == null)
        {
            return new BaseResponse<GetBriefQuestionAnswerResponseModel>
            {
                Success = false,
                Message = "Question not found",
            };
        }

        var questionAnswer = _mapper.Map<Domain.Entities.QuestionAnswer>(request);
        var createQuestionAnswerResult = await _context.AddAsync(questionAnswer, cancellationToken);

        if(createQuestionAnswerResult.Entity == null)
        {
            return new BaseResponse<GetBriefQuestionAnswerResponseModel>
            {
                Success = false,
                Message = "Create question answer failed",
            };
        }

        await _context.SaveChangesAsync(cancellationToken);

        var mappedQuestionAnswerResult = _mapper.Map<GetBriefQuestionAnswerResponseModel>(createQuestionAnswerResult.Entity);

        return new BaseResponse<GetBriefQuestionAnswerResponseModel>
        {
            Success = true,
            Message = "Create question answer successful",
            Data = mappedQuestionAnswerResult
        };
    }
}
