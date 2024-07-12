using System.ComponentModel.DataAnnotations;
using Infrastructure.Data;
using Application.Common;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Topics.Commands;

[AutoMap(typeof(Domain.Entities.Topic), ReverseMap = true)]
public sealed record CreateTopicCommand : IRequest<BaseResponse<GetTopicResponseModel>>
{
    [Required]
    [StringLength(maximumLength: 50, MinimumLength = 4, ErrorMessage = "Title must be at least 4 characters long.")]
    //[RegularExpression(@"^(?:[A-Z][a-z0-9]*)(?: [A-Z][a-z0-9]*)*$", ErrorMessage = "Title must have the first word capitalized, following words separated by a space, and only contain characters and numbers.")]
    public string Title { get; init; }
    [Required]
    public string Description { get; init; }
    [Required]
    public Guid ChapterId { get; set; }
}

public class CreateTopicCommandHanler : IRequestHandler<CreateTopicCommand, BaseResponse<GetTopicResponseModel>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateTopicCommandHanler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse<GetTopicResponseModel>> Handle(CreateTopicCommand request, CancellationToken cancellationToken)
    {
        var chapter = await _context.Chapters.FirstOrDefaultAsync(x => x.Id == request.ChapterId);

        if(chapter == null)
        {
            return new BaseResponse<GetTopicResponseModel>
            {
                Success = false,
                Message = "Chapter not found",
            };
        }

        var topic = _mapper.Map<Domain.Entities.Topic>(request);
        var createTopicResult = await _context.AddAsync(topic, cancellationToken);

        if(createTopicResult.Entity == null)
        {
            return new BaseResponse<GetTopicResponseModel>
            {
                Success = false,
                Message = "Create topic failed",
            };
        }

        await _context.SaveChangesAsync(cancellationToken);

        var mappedTopicResult = _mapper.Map<GetTopicResponseModel>(createTopicResult.Entity);

        return new BaseResponse<GetTopicResponseModel>
        {
            Success = true,
            Message = "Create topic successful",
            Data = mappedTopicResult
        };
    }
}
