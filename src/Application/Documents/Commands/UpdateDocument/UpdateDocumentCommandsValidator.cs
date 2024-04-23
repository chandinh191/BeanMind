using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace BeanMind.Application.Documents.Commands.UpdateDocument;
public class UpdateDocumentCommandsValidator : AbstractValidator<UpdateDocumentCommands>
{
    public UpdateDocumentCommandsValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
