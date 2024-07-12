using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Application.Common;
using AutoMapper;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.WorksheetTemplates.Queries;

public sealed record GetWorksheetTemplateQuery : IRequest<BaseResponse<GetWorksheetTemplateResponseModel>>
{
    [Required]
    public Guid Id { get; init; }
}

public class GetWorksheetTemplateQueryHanler : IRequestHandler<GetWorksheetTemplateQuery, BaseResponse<GetWorksheetTemplateResponseModel>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetWorksheetTemplateQueryHanler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse<GetWorksheetTemplateResponseModel>> Handle(GetWorksheetTemplateQuery request, CancellationToken cancellationToken)
    {
        if(request.Id == Guid.Empty)
        {
            return new BaseResponse<GetWorksheetTemplateResponseModel>
            {
                Success = false,
                Message = "Get worksheettemplate failed",
                Errors = ["Id required"],
            };
        }

        var worksheettemplate = await _context.WorksheetTemplates
            
            .Include(x => x.Worksheets)
            .FirstOrDefaultAsync(x => x.Id.Equals(request.Id), cancellationToken);
        var mappedWorksheetTemplate = _mapper.Map<GetWorksheetTemplateResponseModel>(worksheettemplate);

        return new BaseResponse<GetWorksheetTemplateResponseModel>
        {
            Success = true,
            Message = "Get worksheettemplate successful",
            Data = mappedWorksheetTemplate
        };
    }
}
