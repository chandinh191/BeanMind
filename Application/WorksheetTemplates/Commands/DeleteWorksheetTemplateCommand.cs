using Application.Common;
using AutoMapper;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Application.WorksheetTemplates.Commands;

public sealed record DeleteWorksheetTemplateCommand : IRequest<BaseResponse<GetWorksheetTemplateResponseModel>>
{
    [Required]
    public Guid Id { get; init; }
}

public class DeleteWorksheetTemplateCommandHanler : IRequestHandler<DeleteWorksheetTemplateCommand, BaseResponse<GetWorksheetTemplateResponseModel>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public DeleteWorksheetTemplateCommandHanler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse<GetWorksheetTemplateResponseModel>> Handle(DeleteWorksheetTemplateCommand request, CancellationToken cancellationToken)
    {
        var worksheettemplate = await _context.WorksheetTemplates.FirstOrDefaultAsync(x => x.Id == request.Id);
        if(worksheettemplate == null)
        {
            return new BaseResponse<GetWorksheetTemplateResponseModel>
            {
                Success = false,
                Message = "WorksheetTemplate not found",
            };
        }

        worksheettemplate.IsDeleted = true;

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
