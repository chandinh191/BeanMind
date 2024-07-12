using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Application.Common;
using AutoMapper;
using MediatR;

namespace Application.QuestionLevels.Commands;

[AutoMap(typeof(Domain.Entities.QuestionLevel), ReverseMap = true)]
public sealed record UpdateQuestionLevelCommand : IRequest<BaseResponse<GetQuestionLevelResponseModel>>
{
    [Required]
    public Guid Id { get; init; }
    [Required]
    [StringLength(maximumLength: 50, MinimumLength = 4, ErrorMessage = "Name must be at least 4 characters long.")]
    //[RegularExpression(@"^(?:[A-Z][a-z0-9]*)(?: [A-Z][a-z0-9]*)*$", ErrorMessage = "Title must have the first word capitalized, following words separated by a space, and only contain characters and numbers.")]
    public string? Name { get; init; }
}

public class UpdateQuestionLevelCommandHanler : IRequestHandler<UpdateQuestionLevelCommand, BaseResponse<GetQuestionLevelResponseModel>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateQuestionLevelCommandHanler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse<GetQuestionLevelResponseModel>> Handle(UpdateQuestionLevelCommand request, CancellationToken cancellationToken)
    {
        var questionlevel = await _context.QuestionLevels.FirstOrDefaultAsync(x => x.Id == request.Id);

        if(questionlevel == null)
        {
            return new BaseResponse<GetQuestionLevelResponseModel>
            {
                Success = false,
                Message = "QuestionLevel is not found",
                Errors = ["QuestionLevel is not found"]
            };
        }

        _mapper.Map(request, questionlevel);

        var updateQuestionLevelResult = _context.Update(questionlevel);

        if (updateQuestionLevelResult.Entity == null)
        {
            return new BaseResponse<GetQuestionLevelResponseModel>
            {
                Success = false,
                Message = "Update questionlevel failed",
            };
        }

        await _context.SaveChangesAsync(cancellationToken);

        var mappedQuestionLevelResult = _mapper.Map<GetQuestionLevelResponseModel>(updateQuestionLevelResult.Entity);

        return new BaseResponse<GetQuestionLevelResponseModel>
        {
            Success = true,
            Message = "Update questionlevel successful",
            Data = mappedQuestionLevelResult
        };
    }
}
