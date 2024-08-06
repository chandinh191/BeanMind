using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Application.Common;
using AutoMapper;
using MediatR;
using Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using Application.Topics;

namespace Application.WorksheetTemplates.Commands;

public class CreateLevelTemplateRelationModel
{
    public Guid QuestionLevelId { get; set; }
    public int NoQuestions { get; set; }
}
[AutoMap(typeof(Domain.Entities.WorksheetTemplate), ReverseMap = true)]
public sealed record CreateWorksheetTemplateCommand : IRequest<BaseResponse<GetBriefWorksheetTemplateResponseModel>>
{
    [Required]
    public string Title { get; set; }
    public string? Description { get; set; }
    [Required]
    public int Classification { get; set; }
    public Guid? CourseId { get; set; }
    public Guid? ChapterId { get; set; }
    public Guid? TopicId { get; set; }
    public List<CreateLevelTemplateRelationModel>? LevelTemplateRelations { get; set; }
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
        if (request.CourseId != null)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == request.CourseId);

            if (course == null)
            {
                return new BaseResponse<GetBriefWorksheetTemplateResponseModel>
                {
                    Success = false,
                    Message = "Course not found",
                };
            }
        }
        if (request.ChapterId != null)
        {
            var chapter = await _context.Chapters.FirstOrDefaultAsync(x => x.Id == request.ChapterId);

            if (chapter == null)
            {
                return new BaseResponse<GetBriefWorksheetTemplateResponseModel>
                {
                    Success = false,
                    Message = "Chapter not found",
                };
            }
        }
        if (request.TopicId != null)
        {
            var topic = await _context.Topics.FirstOrDefaultAsync(x => x.Id == request.TopicId);

            if (topic == null)
            {
                return new BaseResponse<GetBriefWorksheetTemplateResponseModel>
                {
                    Success = false,
                    Message = "Topic not found",
                };
            }
        }

        //var worksheetTemplate = _mapper.Map<Domain.Entities.WorksheetTemplate>(request);
        var worksheetTemplate = new WorksheetTemplate()
        {
            CourseId = request.CourseId,
            ChapterId = request.ChapterId,
            TopicId = request.TopicId,  
            Classification = request.Classification,
            Title = request.Title,
            Description = request.Description,
        };

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

        if (request.LevelTemplateRelations != null && request.LevelTemplateRelations.Count > 0)
        {
            foreach(var record in request.LevelTemplateRelations)
            {
                var levelTemplateRelation = new LevelTemplateRelation()
                {
                    WorksheetTemplateId = worksheetTemplate.Id,
                    QuestionLevelId = record.QuestionLevelId,
                    NoQuestions = record.NoQuestions,
                };
                var createLevelTemplateRelationResult = await _context.AddAsync(levelTemplateRelation, cancellationToken);
            }
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
