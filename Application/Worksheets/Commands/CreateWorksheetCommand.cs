using System.ComponentModel.DataAnnotations;
using Infrastructure.Data;
using Application.Common;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Worksheets.Commands;

[AutoMap(typeof(Domain.Entities.Worksheet), ReverseMap = true)]
public sealed record CreateWorksheetCommand : IRequest<BaseResponse<GetWorksheetResponseModel>>
{
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

public class CreateWorksheetCommandHanler : IRequestHandler<CreateWorksheetCommand, BaseResponse<GetWorksheetResponseModel>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateWorksheetCommandHanler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse<GetWorksheetResponseModel>> Handle(CreateWorksheetCommand request, CancellationToken cancellationToken)
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

        var worksheet = _mapper.Map<Domain.Entities.Worksheet>(request);
        var createWorksheetResult = await _context.AddAsync(worksheet, cancellationToken);

        if(createWorksheetResult.Entity == null)
        {
            return new BaseResponse<GetWorksheetResponseModel>
            {
                Success = false,
                Message = "Create worksheet failed",
            };
        }

        await _context.SaveChangesAsync(cancellationToken);

        var mappedWorksheetResult = _mapper.Map<GetWorksheetResponseModel>(createWorksheetResult.Entity);

        return new BaseResponse<GetWorksheetResponseModel>
        {
            Success = true,
            Message = "Create worksheet successful",
            Data = mappedWorksheetResult
        };
    }
}
