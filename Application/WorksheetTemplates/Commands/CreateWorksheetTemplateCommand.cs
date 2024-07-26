using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Application.Common;
using AutoMapper;
using MediatR;

namespace Application.WorksheetTemplates.Commands;

[AutoMap(typeof(Domain.Entities.WorksheetTemplate), ReverseMap = true)]
public sealed record CreateWorksheetTemplateCommand : IRequest<BaseResponse<GetBriefWorksheetTemplateResponseModel>>
{
    [Required]
    public string Title { get; set; }
    [Required]
    public int Classification { get; set; }
    public Guid? CourseId { get; set; }
    public Guid? ChapterId { get; set; }
    public Guid? TopicId { get; set; }
}

public class CreateWorksheetTemplateCommandHanler : IRequestHandler<CreateWorksheetTemplateCommand, BaseResponse<GetBriefWorksheetTemplateResponseModel>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateWorksheetTemplateCommandHanler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse<GetBriefWorksheetTemplateResponseModel>> Handle(CreateWorksheetTemplateCommand request, CancellationToken cancellationToken)
    {
       
        var worksheetTemplate = _mapper.Map<Domain.Entities.WorksheetTemplate>(request);
        var createWorksheetTemplateResult = await _context.AddAsync(worksheetTemplate, cancellationToken);

        if(createWorksheetTemplateResult.Entity == null)
        {
            return new BaseResponse<GetBriefWorksheetTemplateResponseModel>
            {
                Success = false,
                Message = "Create worksheet template failed",
            };
        }

        await _context.SaveChangesAsync(cancellationToken);

        var mappedWorksheetTemplateResult = _mapper.Map<GetBriefWorksheetTemplateResponseModel>(createWorksheetTemplateResult.Entity);

        return new BaseResponse<GetBriefWorksheetTemplateResponseModel>
        {
            Success = true,
            Message = "Create worksheet template successful",
            Data = mappedWorksheetTemplateResult
        };
    }
}
