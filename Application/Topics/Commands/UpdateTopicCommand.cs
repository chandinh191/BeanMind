using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Application.Common;
using AutoMapper;
using MediatR;

namespace Application.Topics.Commands;

[AutoMap(typeof(Domain.Entities.Topic), ReverseMap = true)]
public sealed record UpdateTopicCommand : IRequest<BaseResponse<GetTopicResponseModel>>
{
    [Required]
    public Guid Id { get; init; }
    [Required]
    [StringLength(maximumLength: 50, MinimumLength = 4, ErrorMessage = "Title must be at least 4 characters long.")]
    //[RegularExpression(@"^(?:[A-Z][a-z0-9]*)(?: [A-Z][a-z0-9]*)*$", ErrorMessage = "Title must have the first word capitalized, following words separated by a space, and only contain characters and numbers.")]
    public string? Title { get; init; }
    [Required]
    public string? Description { get; init; }
    [Required]
    public Guid ChapterId { get; set; }
}

public class UpdateTopicCommandHanler : IRequestHandler<UpdateTopicCommand, BaseResponse<GetTopicResponseModel>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateTopicCommandHanler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse<GetTopicResponseModel>> Handle(UpdateTopicCommand request, CancellationToken cancellationToken)
    {
        var chapter = await _context.Chapter.FirstOrDefaultAsync(x => x.Id == request.ChapterId);

        if (chapter == null)
        {
            return new BaseResponse<GetTopicResponseModel>
            {
                Success = false,
                Message = "Chapter not found",
            };
        }

        var topic = await _context.Topic.FirstOrDefaultAsync(x => x.Id == request.Id);

        if(topic == null)
        {
            return new BaseResponse<GetTopicResponseModel>
            {
                Success = false,
                Message = "Topic is not found",
                Errors = ["Topic is not found"]
            };
        }

        _mapper.Map(request, topic);

        var updateTopicResult = _context.Update(topic);

        if (updateTopicResult.Entity == null)
        {
            return new BaseResponse<GetTopicResponseModel>
            {
                Success = false,
                Message = "Update topic failed",
            };
        }

        await _context.SaveChangesAsync(cancellationToken);

        var mappedTopicResult = _mapper.Map<GetTopicResponseModel>(updateTopicResult.Entity);

        return new BaseResponse<GetTopicResponseModel>
        {
            Success = true,
            Message = "Update topic successful",
            Data = mappedTopicResult
        };
    }
}
