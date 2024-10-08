﻿using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Application.Common;
using AutoMapper;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Questions.Queries;

public sealed record GetQuestionQuery : IRequest<BaseResponse<GetQuestionResponseModel>>
{
    [Required]
    public Guid Id { get; init; }
}

public class GetQuestionQueryHanler : IRequestHandler<GetQuestionQuery, BaseResponse<GetQuestionResponseModel>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetQuestionQueryHanler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse<GetQuestionResponseModel>> Handle(GetQuestionQuery request, CancellationToken cancellationToken)
    {
        if(request.Id == Guid.Empty)
        {
            return new BaseResponse<GetQuestionResponseModel>
            {
                Success = false,
                Message = "Get question failed",
                Errors = ["Id required"],
            };
        }

        var question = await _context.Questions
            .Include(x => x.Topic)
            .Include(x => x.QuestionLevel)
            .Include(x => x.QuestionAnswers) 
            .Include(x => x.WorksheetQuestions)
            .FirstOrDefaultAsync(x => x.Id.Equals(request.Id), cancellationToken);
        if (question != null)
        {
            question.QuestionAnswers = question.QuestionAnswers.Where(qa => !qa.IsDeleted).ToList();
        }
        var mappedQuestion = _mapper.Map<GetQuestionResponseModel>(question);

        return new BaseResponse<GetQuestionResponseModel>
        {
            Success = true,
            Message = "Get question successful",
            Data = mappedQuestion
        };
    }
}
