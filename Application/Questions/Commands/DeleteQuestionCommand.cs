using Application.Common;
using AutoMapper;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Application.Questions.Commands;

public sealed record DeleteQuestionCommand : IRequest<BaseResponse<GetQuestionResponseModel>>
{
    [Required]
    public Guid Id { get; init; }
}

public class DeleteQuestionCommandHanler : IRequestHandler<DeleteQuestionCommand, BaseResponse<GetQuestionResponseModel>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public DeleteQuestionCommandHanler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse<GetQuestionResponseModel>> Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
    {
        var question = await _context.Questions.FirstOrDefaultAsync(x => x.Id == request.Id);
        if(question == null)
        {
            return new BaseResponse<GetQuestionResponseModel>
            {
                Success = false,
                Message = "Question not found",
            };
        }

        question.IsDeleted = true;

        var updateQuestionResult = _context.Update(question);

        if (updateQuestionResult.Entity == null)
        {
            return new BaseResponse<GetQuestionResponseModel>
            {
                Success = false,
                Message = "Delete question failed",
            };
        }

        await _context.SaveChangesAsync(cancellationToken);

        var mappedQuestionResult = _mapper.Map<GetQuestionResponseModel>(updateQuestionResult.Entity);

        return new BaseResponse<GetQuestionResponseModel>
        {
            Success = true,
            Message = "Delete question successful",
            Data = mappedQuestionResult
        };
    }
}
