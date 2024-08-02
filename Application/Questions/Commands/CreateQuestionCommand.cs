using System.ComponentModel.DataAnnotations;
using Infrastructure.Data;
using Application.Common;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.QuestionAnswers;
using Domain.Entities;


namespace Application.Questions.Commands;
public class CreateQuestionAnswerModel
{
    [Required]
    public string Content { get; set; }
    [Required]
    public bool IsCorrect { get; set; }
}


[AutoMap(typeof(Domain.Entities.Question), ReverseMap = true)]
public sealed record CreateQuestionCommand : IRequest<BaseResponse<GetQuestionResponseModel>>
{
    [Required]
    public string Content { get; set; }
    public string ImageUrl { get; set; }
    [Required]
    public Guid TopicId { get; set; }
    [Required]
    public Guid QuestionLevelId { get; set; }
    public List<CreateQuestionAnswerModel>? QuestionAnswers { get; set; }
}

public class CreateQuestionCommandHanler : IRequestHandler<CreateQuestionCommand, BaseResponse<GetQuestionResponseModel>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateQuestionCommandHanler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse<GetQuestionResponseModel>> Handle(CreateQuestionCommand request, CancellationToken cancellationToken)
    {
        var topic = await _context.Topics.FirstOrDefaultAsync(x => x.Id == request.TopicId);

        if (topic == null)
        {
            return new BaseResponse<GetQuestionResponseModel>
            {
                Success = false,
                Message = "Topic not found",
            };
        }

        var questionLevel = await _context.QuestionLevels.FirstOrDefaultAsync(x => x.Id == request.QuestionLevelId);

        if (questionLevel == null)
        {
            return new BaseResponse<GetQuestionResponseModel>
            {
                Success = false,
                Message = "Question level not found",
            };
        }
        var question = new Question
        {
            Content = request.Content,
            ImageUrl = request.ImageUrl,
            TopicId = request.TopicId,
            QuestionLevelId = request.QuestionLevelId,
        };

        //var question = _mapper.Map<Domain.Entities.Question>(request);
        var createQuestionResult = await _context.AddAsync(question, cancellationToken);

        if(createQuestionResult.Entity == null)
        {
            return new BaseResponse<GetQuestionResponseModel>
            {
                Success = false,
                Message = "Create question failed",
            };
        }

        await _context.SaveChangesAsync(cancellationToken);

        //Create Question Answer (if any)
        if (request.QuestionAnswers != null && request.QuestionAnswers.Count > 0) {
            foreach (var questionAnswer in request.QuestionAnswers)
            {
                var questionAnswerModel = new QuestionAnswer
                {
                    QuestionId = question.Id,
                    Content = questionAnswer.Content,
                    IsCorrect = questionAnswer.IsCorrect,
                };
                var createQuestionAnswerResult = await _context.AddAsync(questionAnswerModel, cancellationToken);
            }
            await _context.SaveChangesAsync(cancellationToken);
        }

        var mappedQuestionResult = _mapper.Map<GetQuestionResponseModel>(createQuestionResult.Entity);

        return new BaseResponse<GetQuestionResponseModel>
        {
            Success = true,
            Message = "Create question successful",
            Data = mappedQuestionResult
        };
    }
}
