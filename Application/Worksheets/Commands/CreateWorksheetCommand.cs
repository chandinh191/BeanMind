﻿using System.ComponentModel.DataAnnotations;
using Infrastructure.Data;
using Application.Common;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Worksheets.Commands;

[AutoMap(typeof(Domain.Entities.Worksheet), ReverseMap = true)]
public sealed record CreateWorksheetCommand : IRequest<BaseResponse<GetBriefWorksheetResponseModel>>
{
    [Required]
    public string Title { get; init; }
    public string Description { get; init; }
    [Required]
    public Guid? WorksheetTemplateId { get; set; }
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

        var worksheet = _mapper.Map<Domain.Entities.Worksheet>(request);
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

        var mappedWorksheetResult = _mapper.Map<GetBriefWorksheetResponseModel>(createWorksheetResult.Entity);

        return new BaseResponse<GetBriefWorksheetResponseModel>
        {
            Success = true,
            Message = "Create worksheet successful",
            Data = mappedWorksheetResult
        };
    }
}
