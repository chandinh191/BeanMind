using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace BeanMind.Application.WorkSheets.Commands.UpdateWorkSheet;
public class UpdateWorkSheetCommandsValidator : AbstractValidator<UpdateWorkSheetCommands>
{
    public UpdateWorkSheetCommandsValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
