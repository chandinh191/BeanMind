using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Application.Common;
using AutoMapper;
using MediatR;

namespace Application.WorksheetTemplates.Commands;

[AutoMap(typeof(Domain.Entities.WorksheetTemplate), ReverseMap = true)]
public sealed record CreateWorksheetTemplateCommand : IRequest<BaseResponse<GetWorksheetTemplateResponseModel>>
{
    [Required]
    public string Title { get; set; }
    [Required]
    public int Classification { get; set; }
}

public class CreateWorksheetTemplateCommandHanler : IRequestHandler<CreateWorksheetTemplateCommand, BaseResponse<GetWorksheetTemplateResponseModel>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateWorksheetTemplateCommandHanler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse<GetWorksheetTemplateResponseModel>> Handle(CreateWorksheetTemplateCommand request, CancellationToken cancellationToken)
    {
       
        var worksheettemplate = _mapper.Map<Domain.Entities.WorksheetTemplate>(request);
        var createWorksheetTemplateResult = await _context.AddAsync(worksheettemplate, cancellationToken);

        if(createWorksheetTemplateResult.Entity == null)
        {
            return new BaseResponse<GetWorksheetTemplateResponseModel>
            {
                Success = false,
                Message = "Create worksheettemplate failed",
            };
        }

        await _context.SaveChangesAsync(cancellationToken);

        var mappedWorksheetTemplateResult = _mapper.Map<GetWorksheetTemplateResponseModel>(createWorksheetTemplateResult.Entity);

        return new BaseResponse<GetWorksheetTemplateResponseModel>
        {
            Success = true,
            Message = "Create worksheettemplate successful",
            Data = mappedWorksheetTemplateResult
        };
    }
}
