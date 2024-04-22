using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanMind.Domain.Entities;

namespace BeanMind.Application.Common.Models;
public class SubjectModel
{
    public string Name { get; set; }
    public string Description { get; set; }
    public IList<Topic>? Topics { get; set; }
}
