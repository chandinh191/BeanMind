using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace BeanMind.Application.DailyChallengeQuestions.Commands.CreateDailyChallengeQuestions;
public class CreateDailyChallengeQuestionCommandsValidator : AbstractValidator<CreateDailyChallengeQuestionCommands>
{
    public CreateDailyChallengeQuestionCommandsValidator()
    {

    }
}
