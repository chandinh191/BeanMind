using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanMind.Domain.Entities;

namespace BeanMind.Application.Common.Models;
public class TopicModel
{
    [ForeignKey(nameof(Subject))]
    public Guid SubjectId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageURL { get; set; }
    public bool Status { get; set; }
    public IList<Lession>? Lessions { get; set; }
}
