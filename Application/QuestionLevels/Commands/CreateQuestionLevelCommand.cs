using System.ComponentModel.DataAnnotations;
using Infrastructure.Data;
using Application.Common;
using AutoMapper;
using MediatR;

namespace Application.QuestionLevels.Commands;

[AutoMap(typeof(Domain.Entities.QuestionLevel), ReverseMap = true)]
public sealed record CreateQuestionLevelCommand : IRequest<BaseResponse<GetQuestionLevelResponseModel>>
{
    [Required]
    [StringLength(maximumLength: 50, MinimumLength = 4, ErrorMessage = "Name must be at least 4 characters long.")]
    //[RegularExpression(@"^(?:[A-Z][a-z0-9]*)(?: [A-Z][a-z0-9]*)*$", ErrorMessage = "Title must have the first word capitalized, following words separated by a space, and only contain characters and numbers.")]
    public string Name { get; init; }
}

public class CreateQuestionLevelCommandHanler : IRequestHandler<CreateQuestionLevelCommand, BaseResponse<GetQuestionLevelResponseModel>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateQuestionLevelCommandHanler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse<GetQuestionLevelResponseModel>> Handle(CreateQuestionLevelCommand request, CancellationToken cancellationToken)
    {
        var questionlevel = _mapper.Map<Domain.Entities.QuestionLevel>(request);
        var createQuestionLevelResult = await _context.AddAsync(questionlevel, cancellationToken);

        if(createQuestionLevelResult.Entity == null)
        {
            return new BaseResponse<GetQuestionLevelResponseModel>
            {
                Success = false,
                Message = "Create questionlevel failed",
            };
        }

        await _context.SaveChangesAsync(cancellationToken);

        var mappedQuestionLevelResult = _mapper.Map<GetQuestionLevelResponseModel>(createQuestionLevelResult.Entity);

        return new BaseResponse<GetQuestionLevelResponseModel>
        {
            Success = true,
            Message = "Create questionlevel successful",
            Data = mappedQuestionLevelResult
        };
    }
}
