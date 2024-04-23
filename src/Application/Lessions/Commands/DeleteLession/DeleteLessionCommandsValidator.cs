﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace BeanMind.Application.Lessions.Commands.DeleteLession;
public class DeleteLessionCommandsValidator : AbstractValidator<DeleteLessionCommands>
{
    public DeleteLessionCommandsValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
