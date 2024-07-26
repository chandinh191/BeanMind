using Application.Common;
using AutoMapper;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Application.Topics.Commands;

public sealed record DeleteTopicCommand : IRequest<BaseResponse<GetBriefTopicResponseModel>>
{
    [Required]
    public Guid Id { get; init; }
}

public class DeleteTopicCommandHanler : IRequestHandler<DeleteTopicCommand, BaseResponse<GetBriefTopicResponseModel>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public DeleteTopicCommandHanler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse<GetBriefTopicResponseModel>> Handle(DeleteTopicCommand request, CancellationToken cancellationToken)
    {
        var topic = await _context.Topics.FirstOrDefaultAsync(x => x.Id == request.Id);
        if(topic == null)
        {
            return new BaseResponse<GetBriefTopicResponseModel>
            {
                Success = false,
                Message = "Topic not found",
            };
        }

        topic.IsDeleted = true;

        var updateTopicResult = _context.Update(topic);

        if (updateTopicResult.Entity == null)
        {
            return new BaseResponse<GetBriefTopicResponseModel>
            {
                Success = false,
                Message = "Delete topic failed",
            };
        }

        await _context.SaveChangesAsync(cancellationToken);

        var mappedTopicResult = _mapper.Map<GetBriefTopicResponseModel>(updateTopicResult.Entity);

        return new BaseResponse<GetBriefTopicResponseModel>
        {
            Success = true,
            Message = "Delete topic successful",
            Data = mappedTopicResult
        };
    }
}
