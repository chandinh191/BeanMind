﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace BeanMind.Application.ChallengeLevels.Queries.GetChallengeLevelWithPagination;
public class GetChallengeLevelWithPaginationQueriesValidator : AbstractValidator<GetChallengeLevelWithPaginationQueries>
{
    public GetChallengeLevelWithPaginationQueriesValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("PageNumber at least greater than or equal to 1.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("PageSize at least greater than or equal to 1.");
    }
}
