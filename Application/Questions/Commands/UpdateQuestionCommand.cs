using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Application.Common;
using AutoMapper;
using MediatR;

namespace Application.Questions.Commands;

[AutoMap(typeof(Domain.Entities.Question), ReverseMap = true)]
public sealed record UpdateQuestionCommand : IRequest<BaseResponse<GetQuestionResponseModel>>
{
    [Required]
    public Guid Id { get; init; }
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

public class UpdateQuestionCommandHanler : IRequestHandler<UpdateQuestionCommand, BaseResponse<GetQuestionResponseModel>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateQuestionCommandHanler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse<GetQuestionResponseModel>> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
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

        



        var question = await _context.Subject.FirstOrDefaultAsync(x => x.Id == request.Id);

        if(question == null)
        {
            return new BaseResponse<GetQuestionResponseModel>
            {
                Success = false,
                Message = "Question is not found",
                Errors = ["Question is not found"]
            };
        }

        _mapper.Map(request, question);

        var updateQuestionResult = _context.Update(question);

        if (updateQuestionResult.Entity == null)
        {
            return new BaseResponse<GetQuestionResponseModel>
            {
                Success = false,
                Message = "Update question failed",
            };
        }

        await _context.SaveChangesAsync(cancellationToken);

        var mappedQuestionResult = _mapper.Map<GetQuestionResponseModel>(updateQuestionResult.Entity);

        return new BaseResponse<GetQuestionResponseModel>
        {
            Success = true,
            Message = "Update question successful",
            Data = mappedQuestionResult
        };
    }
}
