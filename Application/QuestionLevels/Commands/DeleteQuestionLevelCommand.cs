using Application.Common;
using AutoMapper;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Application.QuestionLevels.Commands;

public sealed record DeleteQuestionLevelCommand : IRequest<BaseResponse<GetQuestionLevelResponseModel>>
{
    [Required]
    public Guid Id { get; init; }
}

public class DeleteQuestionLevelCommandHanler : IRequestHandler<DeleteQuestionLevelCommand, BaseResponse<GetQuestionLevelResponseModel>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public DeleteQuestionLevelCommandHanler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse<GetQuestionLevelResponseModel>> Handle(DeleteQuestionLevelCommand request, CancellationToken cancellationToken)
    {
        var questionlevel = await _context.QuestionLevels.FirstOrDefaultAsync(x => x.Id == request.Id);
        if(questionlevel == null)
        {
            return new BaseResponse<GetQuestionLevelResponseModel>
            {
                Success = false,
                Message = "QuestionLevel not found",
            };
        }

        questionlevel.IsDeleted = true;

        var updateQuestionLevelResult = _context.Update(questionlevel);

        if (updateQuestionLevelResult.Entity == null)
        {
            return new BaseResponse<GetQuestionLevelResponseModel>
            {
                Success = false,
                Message = "Delete questionlevel failed",
            };
        }

        await _context.SaveChangesAsync(cancellationToken);

        var mappedQuestionLevelResult = _mapper.Map<GetQuestionLevelResponseModel>(updateQuestionLevelResult.Entity);

        return new BaseResponse<GetQuestionLevelResponseModel>
        {
            Success = true,
            Message = "Delete questionlevel successful",
            Data = mappedQuestionLevelResult
        };
    }
}
