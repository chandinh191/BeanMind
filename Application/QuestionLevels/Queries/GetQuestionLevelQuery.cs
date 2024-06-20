using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Application.Common;
using AutoMapper;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.QuestionLevels.Queries;

public sealed record GetQuestionLevelQuery : IRequest<BaseResponse<GetQuestionLevelResponseModel>>
{
    [Required]
    public Guid Id { get; init; }
}

public class GetQuestionLevelQueryHanler : IRequestHandler<GetQuestionLevelQuery, BaseResponse<GetQuestionLevelResponseModel>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetQuestionLevelQueryHanler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse<GetQuestionLevelResponseModel>> Handle(GetQuestionLevelQuery request, CancellationToken cancellationToken)
    {
        if(request.Id == Guid.Empty)
        {
            return new BaseResponse<GetQuestionLevelResponseModel>
            {
                Success = false,
                Message = "Get questionlevel failed",
                Errors = ["Id required"],
            };
        }

        var questionlevel = await _context.QuestionLevel.Include(x => x.Questions).FirstOrDefaultAsync(x => x.Id.Equals(request.Id), cancellationToken);
        var mappedQuestionLevel = _mapper.Map<GetQuestionLevelResponseModel>(questionlevel);

        return new BaseResponse<GetQuestionLevelResponseModel>
        {
            Success = true,
            Message = "Get questionlevel successful",
            Data = mappedQuestionLevel
        };
    }
}
