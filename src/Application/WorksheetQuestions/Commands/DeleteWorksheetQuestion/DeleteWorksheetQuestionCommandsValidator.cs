using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace BeanMind.Application.WorksheetQuestions.Commands.DeleteWorksheetQuestion;
public class DeleteWorksheetQuestionCommandsValidator : AbstractValidator<DeleteWorksheetQuestionCommands>
{
    public DeleteWorksheetQuestionCommandsValidator()
    {
        RuleFor(x => x.Id).NotEmpty();  
    }
}
