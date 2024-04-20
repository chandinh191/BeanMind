﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeanMind.Domain.Entities;
public class Video : BaseAuditableEntity
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string VideoURL { get; set; }
   
}
