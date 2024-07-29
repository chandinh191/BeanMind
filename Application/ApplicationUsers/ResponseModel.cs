using Application.Common;
using Application.Participants;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ApplicationUsers
{
    [AutoMap(typeof(Domain.Entities.ApplicationUser))]
    public class GetBriefApplicationUserResponseModel : BaseResponseModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }    
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public string? Email { get; set; }
    }

    [AutoMap(typeof(Domain.Entities.ApplicationUser))]
    public class GetApplicationUserResponseModel : BaseResponseModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public string? Email { get; set; }

    }
}
