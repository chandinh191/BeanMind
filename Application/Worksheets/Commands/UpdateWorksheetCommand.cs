using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Application.Common;
using AutoMapper;
using MediatR;

namespace Application.Worksheets.Commands;

[AutoMap(typeof(Domain.Entities.Worksheet), ReverseMap = true)]
public sealed record UpdateWorksheetCommand : IRequest<BaseResponse<GetWorksheetResponseModel>>
{
    [Required]
    public Guid Id { get; init; }
    [Required]
    [StringLength(maximumLength: 50, MinimumLength = 4, ErrorMessage = "Title must be at least 4 characters long.")]
    //[RegularExpression(@"^(?:[A-Z][a-z0-9]*)(?: [A-Z][a-z0-9]*)*$", ErrorMessage = "Title must have the first word capitalized, following words separated by a space, and only contain characters and numbers.")]
    public string Title { get; init; }
    [Required]
    public string Description { get; init; }
    [Required]
    public Guid ActivityId { get; set; }
    [Required]
    public Guid? WorksheetTemplateId { get; set; }
}

public class UpdateWorksheetCommandHanler : IRequestHandler<UpdateWorksheetCommand, BaseResponse<GetWorksheetResponseModel>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateWorksheetCommandHanler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse<GetWorksheetResponseModel>> Handle(UpdateWorksheetCommand request, CancellationToken cancellationToken)
    {
        

        var worksheetTemplate = await _context.WorksheetTemplate.FirstOrDefaultAsync(x => x.Id == request.WorksheetTemplateId);

        if (worksheetTemplate == null)
        {
            return new BaseResponse<GetWorksheetResponseModel>
            {
                Success = false,
                Message = "WorksheetTemplate not found",
            };
        }

        var worksheet = await _context.Worksheet.FirstOrDefaultAsync(x => x.Id == request.Id);

        if(worksheet == null)
        {
            return new BaseResponse<GetWorksheetResponseModel>
            {
                Success = false,
                Message = "Worksheet is not found",
                Errors = ["Worksheet is not found"]
            };
        }

        _mapper.Map(request, worksheet);

        var updateWorksheetResult = _context.Update(worksheet);

        if (updateWorksheetResult.Entity == null)
        {
            return new BaseResponse<GetWorksheetResponseModel>
            {
                Success = false,
                Message = "Update worksheet failed",
            };
        }

        await _context.SaveChangesAsync(cancellationToken);

        var mappedWorksheetResult = _mapper.Map<GetWorksheetResponseModel>(updateWorksheetResult.Entity);

        return new BaseResponse<GetWorksheetResponseModel>
        {
            Success = true,
            Message = "Update worksheet successful",
            Data = mappedWorksheetResult
        };
    }
}
