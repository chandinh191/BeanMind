using System.ComponentModel.DataAnnotations;
using Infrastructure.Data;
using Application.Common;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Application.Questions.Commands;

[AutoMap(typeof(Domain.Entities.Question), ReverseMap = true)]
public sealed record CreateQuestionCommand : IRequest<BaseResponse<GetQuestionResponseModel>>
{
    [Required]
    public string Text { get; set; }
    [Required]
    public string ImageUrl { get; set; }
    [Required]
    public Guid TopicId { get; set; }
    [Required]
    public Guid QuestionLevelId { get; set; }
    [Required]
    public Guid QuestionTypeId { get; set; }
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
        var topic = await _context.Topic.FirstOrDefaultAsync(x => x.Id == request.TopicId);

        if (topic == null)
        {
            return new BaseResponse<GetQuestionResponseModel>
            {
                Success = false,
                Message = "Topic not found",
            };
        }

        var questionLevel = await _context.QuestionLevel.FirstOrDefaultAsync(x => x.Id == request.QuestionLevelId);

        if (questionLevel == null)
        {
            return new BaseResponse<GetQuestionResponseModel>
            {
                Success = false,
                Message = "QuestionLevel not found",
            };
        }

        



        var question = _mapper.Map<Domain.Entities.Question>(request);
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

        var mappedQuestionResult = _mapper.Map<GetQuestionResponseModel>(createQuestionResult.Entity);

        return new BaseResponse<GetQuestionResponseModel>
        {
            Success = true,
            Message = "Create question successful",
            Data = mappedQuestionResult
        };
    }
}
