using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace BeanMind.Application.DailyChallengeQuestions.Commands.UpdateDailyChallengeQuestions;
public class UpdateDailyChallengeQuestionCommandsValidator : AbstractValidator<UpdateDailyChallengeQuestionCommands>
{
    public UpdateDailyChallengeQuestionCommandsValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
