using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace BeanMind.Application.Documents.Commands.DeleteDocument;
public class DeleteDocumentCommandsValidator : AbstractValidator<DeleteDocumentCommands>
{
    public DeleteDocumentCommandsValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
