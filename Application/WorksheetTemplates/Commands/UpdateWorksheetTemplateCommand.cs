using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Application.Common;
using AutoMapper;
using MediatR;

namespace Application.WorksheetTemplates.Commands;

[AutoMap(typeof(Domain.Entities.WorksheetTemplate), ReverseMap = true)]
public sealed record UpdateWorksheetTemplateCommand : IRequest<BaseResponse<GetWorksheetTemplateResponseModel>>
{
    [Required]
    public Guid Id { get; init; }
    [Required]
    public string Title { get; set; }
    [Required]
    public int Classification { get; set; }
}

public class UpdateWorksheetTemplateCommandHanler : IRequestHandler<UpdateWorksheetTemplateCommand, BaseResponse<GetWorksheetTemplateResponseModel>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateWorksheetTemplateCommandHanler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse<GetWorksheetTemplateResponseModel>> Handle(UpdateWorksheetTemplateCommand request, CancellationToken cancellationToken)
    {

        var worksheettemplate = await _context.WorksheetTemplates.FirstOrDefaultAsync(x => x.Id == request.Id);

        if(worksheettemplate == null)
        {
            return new BaseResponse<GetWorksheetTemplateResponseModel>
            {
                Success = false,
                Message = "WorksheetTemplate is not found",
                Errors = ["WorksheetTemplate is not found"]
            };
        }

        _mapper.Map(request, worksheettemplate);

        var updateWorksheetTemplateResult = _context.Update(worksheettemplate);

        if (updateWorksheetTemplateResult.Entity == null)
        {
            return new BaseResponse<GetWorksheetTemplateResponseModel>
            {
                Success = false,
                Message = "Update worksheettemplate failed",
            };
        }

        await _context.SaveChangesAsync(cancellationToken);

        var mappedWorksheetTemplateResult = _mapper.Map<GetWorksheetTemplateResponseModel>(updateWorksheetTemplateResult.Entity);

        return new BaseResponse<GetWorksheetTemplateResponseModel>
        {
            Success = true,
            Message = "Update worksheettemplate successful",
            Data = mappedWorksheetTemplateResult
        };
    }
}
