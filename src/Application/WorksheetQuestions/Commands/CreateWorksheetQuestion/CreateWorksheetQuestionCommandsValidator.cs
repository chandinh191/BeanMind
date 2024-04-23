using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace BeanMind.Application.WorksheetQuestions.Commands.CreateWorksheetQuestion;
public class CreateWorksheetQuestionCommandsValidator : AbstractValidator<CreateWorksheetQuestionCommands>
{
    public CreateWorksheetQuestionCommandsValidator()
    {

    }
}
