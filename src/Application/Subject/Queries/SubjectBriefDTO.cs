using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanMind.Application.Common.Mappings;

using BeanMind.Domain.Entities;

namespace BeanMind.Application.Subject.Queries;
public class SubjectBriefDTO : IMapFrom<BeanMind.Domain.Entities.Subject>
{
    public string Name { get; set; }
    public string Description { get; set; }
}
