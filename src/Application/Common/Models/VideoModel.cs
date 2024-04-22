using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanMind.Application.Common.Mappings;

namespace BeanMind.Application.Common.Models;
public class VideoModel : IMapFrom<Domain.Entities.Video>
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string VideoURL { get; set; }
}
