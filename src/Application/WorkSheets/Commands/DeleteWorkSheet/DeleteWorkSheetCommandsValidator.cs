using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace BeanMind.Application.WorkSheets.Commands.DeleteWorkSheet;
public class DeleteWorkSheetCommandsValidator : AbstractValidator<DeleteWorkSheetCommands>
{
    public DeleteWorkSheetCommandsValidator()
    {
        RuleFor(x => x.Id).NotEmpty();  
    }
}
