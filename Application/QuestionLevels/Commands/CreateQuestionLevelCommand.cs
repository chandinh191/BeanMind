using System.ComponentModel.DataAnnotations;
using Infrastructure.Data;
using Application.Common;
using AutoMapper;
using MediatR;

namespace Application.QuestionLevels.Commands;

[AutoMap(typeof(Domain.Entities.QuestionLevel), ReverseMap = true)]
public sealed record CreateQuestionLevelCommand : IRequest<BaseResponse<GetBriefQuestionLevelResponseModel>>
{
    [Required]
    public string Title { get; init; }
    public string Description { get; set; }
}

public class CreateQuestionLevelCommandHanler : IRequestHandler<CreateQuestionLevelCommand, BaseResponse<GetBriefQuestionLevelResponseModel>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateQuestionLevelCommandHanler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse<GetBriefQuestionLevelResponseModel>> Handle(CreateQuestionLevelCommand request, CancellationToken cancellationToken)
    {
        var questionLevel = _mapper.Map<Domain.Entities.QuestionLevel>(request);
        var createQuestionLevelResult = await _context.AddAsync(questionLevel, cancellationToken);

        if(createQuestionLevelResult.Entity == null)
        {
            return new BaseResponse<GetBriefQuestionLevelResponseModel>
            {
                Success = false,
                Message = "Create question level failed",
            };
        }

        await _context.SaveChangesAsync(cancellationToken);

        var mappedQuestionLevelResult = _mapper.Map<GetBriefQuestionLevelResponseModel>(createQuestionLevelResult.Entity);

        return new BaseResponse<GetBriefQuestionLevelResponseModel>
        {
            Success = true,
            Message = "Create question level successful",
            Data = mappedQuestionLevelResult
        };
    }
}
