using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Application.Common;
using AutoMapper;
using MediatR;
using Domain.Entities;

namespace Application.Worksheets.Commands;

public class UpdateWorkSheetQuestionrModel
{
    [Required]
    public Guid QuestionId { get; set; }
}

[AutoMap(typeof(Domain.Entities.Worksheet), ReverseMap = true)]
public sealed record UpdateWorksheetCommand : IRequest<BaseResponse<GetBriefWorksheetResponseModel>>
{
    [Required]
    public Guid Id { get; init; }
    public string? Title { get; init; }
    public string? Description { get; init; }
    public Guid? WorksheetTemplateId { get; set; }
    public List<UpdateWorkSheetQuestionrModel>? WorksheetQuestions { get; set; }
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
                if (targetProperty != null && targetProperty.Name != "WorksheetQuestions")
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

        if (request.WorksheetQuestions != null && request.WorksheetQuestions.Count > 0)
        {
            var worksheetQuestions = _context.WorksheetQuestions.Where(x => x.WorksheetId == worksheet.Id).AsQueryable();
            foreach (var record in worksheetQuestions)
            {
                _context.Remove(record);
            }
            await _context.SaveChangesAsync(cancellationToken);
            foreach (var question in request.WorksheetQuestions)
            {
                var existedWorksheetQuestion = await _context.WorksheetQuestions
                    .FirstOrDefaultAsync(x => x.WorksheetId == worksheet.Id && x.QuestionId == question.QuestionId);
                if (existedWorksheetQuestion != null)
                {
                    existedWorksheetQuestion.IsDeleted = false;
                }
                else
                {
                    var worksheetQuestion = new WorksheetQuestion()
                    {
                        WorksheetId = worksheet.Id,
                        QuestionId = question.QuestionId,
                    };
                    var createWorksheetQuestionResult = await _context.AddAsync(worksheetQuestion, cancellationToken);
                }
            }
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
