using Application.Common;
using Application.Students;
using Domain.Entities.UserEntities;
using Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.Commands
{
    public record class SendAccountStudentInfoCommand : IRequest<BaseResponse<string>>
    {
        public required string ParentEmail { get; init; }
        public required string Username { get; init; }
        public required string Password { get; init; }
    }

    public class SendAccountStudentInfoCommandHandler : IRequestHandler<SendAccountStudentInfoCommand, BaseResponse<string>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly LinkGenerator _linkGenerator;
        private readonly IEmailService _emailService;

        public SendAccountStudentInfoCommandHandler(UserManager<ApplicationUser> userManager, IConfiguration configuration, LinkGenerator linkGenerator,
            IEmailService emailService, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _configuration = configuration;
            _linkGenerator = linkGenerator;
            _emailService = emailService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<BaseResponse<string>> Handle(SendAccountStudentInfoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _emailService.SendInfoStudentAsync(request.ParentEmail, request.Username, request.Password);
            }
            catch
            {
                return new BaseResponse<string>()
                {
                    Success = false,
                    Message = "There is some problem at mail service, please contact with admin at (+84)9276122811"
                };
            }

            return new BaseResponse<string>()
            {
                Success = true,
                Message = "Email resent, Please check your inbox to get the login information of your student"
            };
        }
    }
}
