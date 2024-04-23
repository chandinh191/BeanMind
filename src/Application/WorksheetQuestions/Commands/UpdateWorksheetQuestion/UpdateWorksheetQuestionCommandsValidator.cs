using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace BeanMind.Application.WorksheetQuestions.Commands.UpdateWorksheetQuestion;
public class UpdateWorksheetQuestionCommandsValidator : AbstractValidator<UpdateWorksheetQuestionCommands>
{
    public UpdateWorksheetQuestionCommandsValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
