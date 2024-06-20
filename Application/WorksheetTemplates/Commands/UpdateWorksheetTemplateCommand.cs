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
    public string Classification { get; set; }
    [Required]
    public int EasyQuestionCount { get; set; }
    [Required]
    public int NormalQuestionCount { get; set; }
    [Required]
    public int HardQuestionCount { get; set; }
    [Required]
    public int TotalQuestionCount { get; set; }
    [Required]
    public bool Suffle { get; set; }
    [Required]
    public Guid SubjectId { get; set; }
    public Guid? ChapterId { get; set; }
    public Guid? TopicId { get; set; }
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
        var subject = await _context.Subject.FirstOrDefaultAsync(x => x.Id == request.SubjectId);

        if (subject == null)
        {
            return new BaseResponse<GetWorksheetTemplateResponseModel>
            {
                Success = false,
                Message = "Subject not found",
            };
        }

        var chapter = await _context.Chapter.FirstOrDefaultAsync(x => x.Id == request.ChapterId);

        if (request.ChapterId != null && chapter == null)
        {
            return new BaseResponse<GetWorksheetTemplateResponseModel>
            {
                Success = false,
                Message = "Chapter not found",
            };
        }

        var topic = await _context.Topic.FirstOrDefaultAsync(x => x.Id == request.TopicId);

        if (request.TopicId != null && topic == null)
        {
            return new BaseResponse<GetWorksheetTemplateResponseModel>
            {
                Success = false,
                Message = "Topic not found",
            };
        }

        var worksheettemplate = await _context.WorksheetTemplate.FirstOrDefaultAsync(x => x.Id == request.Id);

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
