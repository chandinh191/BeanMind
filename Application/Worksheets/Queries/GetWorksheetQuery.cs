using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Application.Common;
using AutoMapper;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Worksheets.Queries;

public sealed record GetWorksheetQuery : IRequest<BaseResponse<GetWorksheetResponseModel>>
{
    [Required]
    public Guid Id { get; init; }
}

public class GetWorksheetQueryHanler : IRequestHandler<GetWorksheetQuery, BaseResponse<GetWorksheetResponseModel>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetWorksheetQueryHanler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse<GetWorksheetResponseModel>> Handle(GetWorksheetQuery request, CancellationToken cancellationToken)
    {
        if(request.Id == Guid.Empty)
        {
            return new BaseResponse<GetWorksheetResponseModel>
            {
                Success = false,
                Message = "Get worksheet failed",
                Errors = ["Id required"],
            };
        }

        var worksheet = await _context.Worksheets
            
            .Include(x => x.WorksheetTemplate)
           
            .FirstOrDefaultAsync(x => x.Id.Equals(request.Id), cancellationToken);

        var mappedWorksheet = _mapper.Map<GetWorksheetResponseModel>(worksheet);

        return new BaseResponse<GetWorksheetResponseModel>
        {
            Success = true,
            Message = "Get worksheet successful",
            Data = mappedWorksheet
        };
    }
}
