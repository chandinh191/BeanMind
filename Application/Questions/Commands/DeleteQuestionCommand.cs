using Application.Common;
using AutoMapper;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Application.Questions.Commands;

public sealed record DeleteQuestionCommand : IRequest<BaseResponse<GetBriefQuestionResponseModel>>
{
    [Required]
    public Guid Id { get; init; }
}

public class DeleteQuestionCommandHanler : IRequestHandler<DeleteQuestionCommand, BaseResponse<GetBriefQuestionResponseModel>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public DeleteQuestionCommandHanler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse<GetBriefQuestionResponseModel>> Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
    {
        var question = await _context.Questions.FirstOrDefaultAsync(x => x.Id == request.Id);
        if(question == null)
        {
            return new BaseResponse<GetBriefQuestionResponseModel>
            {
                Success = false,
                Message = "Question not found",
            };
        }

        question.IsDeleted = true;

        var updateQuestionResult = _context.Update(question);

        if (updateQuestionResult.Entity == null)
        {
            return new BaseResponse<GetBriefQuestionResponseModel>
            {
                Success = false,
                Message = "Delete question failed",
            };
        }

        await _context.SaveChangesAsync(cancellationToken);

        var mappedQuestionResult = _mapper.Map<GetBriefQuestionResponseModel>(updateQuestionResult.Entity);

        return new BaseResponse<GetBriefQuestionResponseModel>
        {
            Success = true,
            Message = "Delete question successful",
            Data = mappedQuestionResult
        };
    }
}
