using System.ComponentModel.DataAnnotations;
using Infrastructure.Data;
using Application.Common;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Worksheets.Commands;
public class CreateWorkSheetQuestionrModel
{
    [Required]
    public Guid QuestionId { get; set; }
}


[AutoMap(typeof(Domain.Entities.Worksheet), ReverseMap = true)]
public sealed record CreateWorksheetCommand : IRequest<BaseResponse<GetBriefWorksheetResponseModel>>
{
    [Required]
    public string Title { get; init; }
    public string? Description { get; init; }
    [Required]
    public Guid WorksheetTemplateId { get; set; }
    public List<CreateWorkSheetQuestionrModel> WorksheetQuestions { get; set; }
}

public class CreateWorksheetCommandHanler : IRequestHandler<CreateWorksheetCommand, BaseResponse<GetBriefWorksheetResponseModel>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateWorksheetCommandHanler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse<GetBriefWorksheetResponseModel>> Handle(CreateWorksheetCommand request, CancellationToken cancellationToken)
    {
        var worksheetTemplate = await _context.WorksheetTemplates.FirstOrDefaultAsync(x => x.Id == request.WorksheetTemplateId);

        if (worksheetTemplate == null)
        {
            return new BaseResponse<GetBriefWorksheetResponseModel>
            {
                Success = false,
                Message = "WorksheetTemplate not found",
            };
        }

        //var worksheet = _mapper.Map<Domain.Entities.Worksheet>(request);
        var worksheet = new Worksheet()
        {
            Title = request.Title,
            Description = request.Description
        };
        var createWorksheetResult = await _context.AddAsync(worksheet, cancellationToken);

        if(createWorksheetResult.Entity == null)
        {
            return new BaseResponse<GetBriefWorksheetResponseModel>
            {
                Success = false,
                Message = "Create worksheet failed",
            };
        }

        await _context.SaveChangesAsync(cancellationToken);

        if (request.WorksheetQuestions != null && request.WorksheetQuestions.Count > 0)
        {
            foreach (var question in request.WorksheetQuestions)
            {
                var worksheetQuestion = new WorksheetQuestion()
                {
                    QuestionId = question.QuestionId,
                    WorksheetId = worksheet.Id,
                };
                var createWorksheetQuestionResult = await _context.AddAsync(worksheetQuestion, cancellationToken);
            }
        }


        var mappedWorksheetResult = _mapper.Map<GetBriefWorksheetResponseModel>(createWorksheetResult.Entity);

        return new BaseResponse<GetBriefWorksheetResponseModel>
        {
            Success = true,
            Message = "Create worksheet successful",
            Data = mappedWorksheetResult
        };
    }
}
