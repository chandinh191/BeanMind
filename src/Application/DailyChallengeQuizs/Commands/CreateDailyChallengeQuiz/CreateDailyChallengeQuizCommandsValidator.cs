using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace BeanMind.Application.DailyChallengeQuizs.Commands.CreateDailyChallengeQuiz;
public class CreateDailyChallengeQuizCommandsValidator : AbstractValidator<CreateDailyChallengeQuizCommands>
{
    public CreateDailyChallengeQuizCommandsValidator()
    {

    }
}
