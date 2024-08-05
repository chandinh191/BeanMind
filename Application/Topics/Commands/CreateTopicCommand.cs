using System.ComponentModel.DataAnnotations;
using Infrastructure.Data;
using Application.Common;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Topics.Commands;

[AutoMap(typeof(Domain.Entities.Topic), ReverseMap = true)]
public sealed record CreateTopicCommand : IRequest<BaseResponse<GetBriefTopicResponseModel>>
{
    [Required]
    public string Title { get; init; }
    public string? Description { get; init; }
    public int? Order { get; set; }
    [Required]
    public Guid ChapterId { get; set; }
}

public class CreateTopicCommandHanler : IRequestHandler<CreateTopicCommand, BaseResponse<GetBriefTopicResponseModel>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateTopicCommandHanler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse<GetBriefTopicResponseModel>> Handle(CreateTopicCommand request, CancellationToken cancellationToken)
    {
        var chapter = await _context.Chapters.FirstOrDefaultAsync(x => x.Id == request.ChapterId);

        if(chapter == null)
        {
            return new BaseResponse<GetBriefTopicResponseModel>
            {
                Success = false,
                Message = "Chapter not found",
            };
        }

        var topic = _mapper.Map<Domain.Entities.Topic>(request);
        var createTopicResult = await _context.AddAsync(topic, cancellationToken);

        if(createTopicResult.Entity == null)
        {
            return new BaseResponse<GetBriefTopicResponseModel>
            {
                Success = false,
                Message = "Create topic failed",
            };
        }

        await _context.SaveChangesAsync(cancellationToken);

        var mappedTopicResult = _mapper.Map<GetBriefTopicResponseModel>(createTopicResult.Entity);

        return new BaseResponse<GetBriefTopicResponseModel>
        {
            Success = true,
            Message = "Create topic successful",
            Data = mappedTopicResult
        };
    }
}
