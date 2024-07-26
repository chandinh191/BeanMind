using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Application.Common;
using AutoMapper;
using MediatR;
using Domain.Entities;

namespace Application.Worksheets.Commands;

[AutoMap(typeof(Domain.Entities.Worksheet), ReverseMap = true)]
public sealed record UpdateWorksheetCommand : IRequest<BaseResponse<GetBriefWorksheetResponseModel>>
{
    [Required]
    public Guid Id { get; init; }
    public string? Title { get; init; }
    public string? Description { get; init; }
    public Guid? WorksheetTemplateId { get; set; }
}

public class UpdateWorksheetCommandHanler : IRequestHandler<UpdateWorksheetCommand, BaseResponse<GetBriefWorksheetResponseModel>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateWorksheetCommandHanler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse<GetBriefWorksheetResponseModel>> Handle(UpdateWorksheetCommand request, CancellationToken cancellationToken)
    {

        if (request.WorksheetTemplateId != null)
        {
            var worksheetTemplate = await _context.WorksheetTemplates.FirstOrDefaultAsync(x => x.Id == request.WorksheetTemplateId);

            if (worksheetTemplate == null)
            {
                return new BaseResponse<GetBriefWorksheetResponseModel>
                {
                    Success = false,
                    Message = "Worksheet template not found",
                };
            }
        }

        var worksheet = await _context.Worksheets.FirstOrDefaultAsync(x => x.Id == request.Id);

        if(worksheet == null)
        {
            return new BaseResponse<GetBriefWorksheetResponseModel>
            {
                Success = false,
                Message = "Worksheet is not found",
                Errors = ["Worksheet is not found"]
            };
        }

        //_mapper.Map(request, worksheet);
        // Use reflection to update non-null properties
        foreach (var property in request.GetType().GetProperties())
        {
            var requestValue = property.GetValue(request);
            if (requestValue != null)
            {
                var targetProperty = worksheet.GetType().GetProperty(property.Name);
                if (targetProperty != null)
                {
                    targetProperty.SetValue(worksheet, requestValue);
                }
            }
        }

        var updateWorksheetResult = _context.Update(worksheet);

        if (updateWorksheetResult.Entity == null)
        {
            return new BaseResponse<GetBriefWorksheetResponseModel>
            {
                Success = false,
                Message = "Update worksheet failed",
            };
        }

        await _context.SaveChangesAsync(cancellationToken);

        var mappedWorksheetResult = _mapper.Map<GetBriefWorksheetResponseModel>(updateWorksheetResult.Entity);

        return new BaseResponse<GetBriefWorksheetResponseModel>
        {
            Success = true,
            Message = "Update worksheet successful",
            Data = mappedWorksheetResult
        };
    }
}
