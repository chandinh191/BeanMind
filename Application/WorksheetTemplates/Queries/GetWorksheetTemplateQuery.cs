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
                Message = "Get worksheet template failed",
                Errors = ["Id required"],
            };
        }

        var worksheetTemplate = await _context.WorksheetTemplates
            .Include(x => x.Course)
            .Include(x => x.Chapter)
            .Include(x => x.Topic)
            .Include(x => x.Worksheets)
            .Include(x => x.LevelTemplateRelations).ThenInclude(o => o.QuestionLevel)
            .FirstOrDefaultAsync(x => x.Id.Equals(request.Id), cancellationToken);
        var mappedWorksheetTemplate = _mapper.Map<GetWorksheetTemplateResponseModel>(worksheetTemplate);

        return new BaseResponse<GetWorksheetTemplateResponseModel>
        {
            Success = true,
            Message = "Get worksheet template successful",
            Data = mappedWorksheetTemplate
        };
    }
}
