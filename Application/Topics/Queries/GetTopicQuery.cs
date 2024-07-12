using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Application.Common;
using AutoMapper;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Topics.Queries;

public sealed record GetTopicQuery : IRequest<BaseResponse<GetTopicResponseModel>>
{
    [Required]
    public Guid Id { get; init; }
}

public class GetTopicQueryHanler : IRequestHandler<GetTopicQuery, BaseResponse<GetTopicResponseModel>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetTopicQueryHanler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse<GetTopicResponseModel>> Handle(GetTopicQuery request, CancellationToken cancellationToken)
    {
        if(request.Id == Guid.Empty)
        {
            return new BaseResponse<GetTopicResponseModel>
            {
                Success = false,
                Message = "Get topic failed",
                Errors = ["Id required"],
            };
        }

        var topic = await _context.Topics
            .Include(x => x.Chapter)
            .FirstOrDefaultAsync(x => x.Id.Equals(request.Id), cancellationToken);

        var mappedTopic = _mapper.Map<GetTopicResponseModel>(topic);

        return new BaseResponse<GetTopicResponseModel>
        {
            Success = true,
            Message = "Get topic successful",
            Data = mappedTopic
        };
    }
}
