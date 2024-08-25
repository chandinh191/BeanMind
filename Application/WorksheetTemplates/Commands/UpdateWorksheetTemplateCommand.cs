using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Application.Common;
using AutoMapper;
using MediatR;
using Application.Worksheets;
using Domain.Entities;

namespace Application.WorksheetTemplates.Commands;

public class UpdateLevelTemplateRelationModel
{
    public Guid QuestionLevelId { get; set; }
    public int NoQuestions { get; set; }
}
[AutoMap(typeof(Domain.Entities.WorksheetTemplate), ReverseMap = true)]
public sealed record UpdateWorksheetTemplateCommand : IRequest<BaseResponse<GetWorksheetTemplateResponseModel>>
{
    [Required]
    public Guid Id { get; init; }
    public string? Title { get; set; }
    public int? Classification { get; set; }
    public Guid? CourseId { get; set; }
    public Guid? ChapterId { get; set; }
    public Guid? TopicId { get; set; }
    public List<UpdateLevelTemplateRelationModel>? LevelTemplateRelations { get; set; }
    public bool? IsDeleted { get; set; }
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
        if (request.CourseId != null)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == request.CourseId);

            if (course == null)
            {
                return new BaseResponse<GetWorksheetTemplateResponseModel>
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
                return new BaseResponse<GetWorksheetTemplateResponseModel>
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
                return new BaseResponse<GetWorksheetTemplateResponseModel>
                {
                    Success = false,
                    Message = "Topic not found",
                };
            }
        }

        var worksheetTemplate = await _context.WorksheetTemplates.FirstOrDefaultAsync(x => x.Id == request.Id);

        if(worksheetTemplate == null)
        {
            return new BaseResponse<GetWorksheetTemplateResponseModel>
            {
                Success = false,
                Message = "Worksheet template is not found",
                Errors = ["Worksheet template is not found"]
            };
        }

        //_mapper.Map(request, worksheettemplate);
        // Use reflection to update non-null properties
        foreach (var property in request.GetType().GetProperties())
        {
            var requestValue = property.GetValue(request);
            if (requestValue != null)
            {
                var targetProperty = worksheetTemplate.GetType().GetProperty(property.Name);
                if (targetProperty != null && targetProperty.Name != "LevelTemplateRelations")
                {
                    targetProperty.SetValue(worksheetTemplate, requestValue);
                }
            }
        }

        var updateWorksheetTemplateResult = _context.Update(worksheetTemplate);

        if (updateWorksheetTemplateResult.Entity == null)
        {
            return new BaseResponse<GetWorksheetTemplateResponseModel>
            {
                Success = false,
                Message = "Update worksheet template failed",
            };
        }

        await _context.SaveChangesAsync(cancellationToken);

        if (request.LevelTemplateRelations != null && request.LevelTemplateRelations.Count > 0)
        {
            var levelTemplateRelations = _context.LevelTemplateRelations.Where(x => x.WorksheetTemplateId == worksheetTemplate.Id).AsQueryable();
            foreach (var record in levelTemplateRelations)
            {
                _context.Remove(record);
            }
            await _context.SaveChangesAsync(cancellationToken);
            foreach (var record in request.LevelTemplateRelations)
            {
                var existedLevelTemplateRelation = await _context.LevelTemplateRelations.FirstOrDefaultAsync(x => x.QuestionLevelId == record.QuestionLevelId && x.WorksheetTemplateId == worksheetTemplate.Id);
                if (existedLevelTemplateRelation != null)
                {
                    existedLevelTemplateRelation.IsDeleted = false;
                }
                else
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
        }

        await _context.SaveChangesAsync(cancellationToken);

        var mappedWorksheetTemplateResult = _mapper.Map<GetWorksheetTemplateResponseModel>(updateWorksheetTemplateResult.Entity);

        return new BaseResponse<GetWorksheetTemplateResponseModel>
        {
            Success = true,
            Message = "Update worksheet template successful",
            Data = mappedWorksheetTemplateResult
        };
    }
}
