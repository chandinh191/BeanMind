using Application.Common;
using AutoMapper;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Application.WorksheetTemplates.Commands;

public sealed record DeleteWorksheetTemplateCommand : IRequest<BaseResponse<GetBriefWorksheetTemplateResponseModel>>
{
    [Required]
    public Guid Id { get; init; }
}

public class DeleteWorksheetTemplateCommandHanler : IRequestHandler<DeleteWorksheetTemplateCommand, BaseResponse<GetBriefWorksheetTemplateResponseModel>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public DeleteWorksheetTemplateCommandHanler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse<GetBriefWorksheetTemplateResponseModel>> Handle(DeleteWorksheetTemplateCommand request, CancellationToken cancellationToken)
    {
        var worksheettemplate = await _context.WorksheetTemplates.FirstOrDefaultAsync(x => x.Id == request.Id);
        if(worksheettemplate == null)
        {
            return new BaseResponse<GetBriefWorksheetTemplateResponseModel>
            {
                Success = false,
                Message = "Worksheet template not found",
            };
        }

        worksheettemplate.IsDeleted = true;

        var updateWorksheetTemplateResult = _context.Update(worksheettemplate);

        if (updateWorksheetTemplateResult.Entity == null)
        {
            return new BaseResponse<GetBriefWorksheetTemplateResponseModel>
            {
                Success = false,
                Message = "Update worksheet template failed",
            };
        }

        await _context.SaveChangesAsync(cancellationToken);

        var mappedWorksheetTemplateResult = _mapper.Map<GetBriefWorksheetTemplateResponseModel>(updateWorksheetTemplateResult.Entity);

        return new BaseResponse<GetBriefWorksheetTemplateResponseModel>
        {
            Success = true,
            Message = "Update worksheet template successful",
            Data = mappedWorksheetTemplateResult
        };
    }
}
