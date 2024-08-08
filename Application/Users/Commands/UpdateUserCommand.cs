using Application.ApplicationUsers;
using Application.Common;
using Application.Sessions;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using Domain.Entities.UserEntities;
using Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Application.Users.Commands
{
    public record class UpdateUserCommand : IRequest<BaseResponse<GetBriefApplicationUserResponseModel>>
    {
        [Required]
        public string Id { get; set; }
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public required string? Username { get; init; }
        //public required string? Password { get; init; }
        public int? YearOfBirth { get; set; }
        public required List<string>? Roles { get; init; }

    }
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, BaseResponse<GetBriefApplicationUserResponseModel>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly LinkGenerator _linkGenerator;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;

        public UpdateUserCommandHandler(UserManager<ApplicationUser> userManager, IConfiguration configuration, LinkGenerator linkGenerator,
            IEmailService emailService, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
            _configuration = configuration;
            _linkGenerator = linkGenerator;
            _emailService = emailService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<BaseResponse<GetBriefApplicationUserResponseModel>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {

                // checking role name invalid
                foreach (var role in request.Roles)
                {
                    if (!Roles.AllRoles.Contains(role))
                    {
                        return new BaseResponse<GetBriefApplicationUserResponseModel>()
                        {
                            Success = false,
                            Message = "Invalid role: " + role,
                            Errors = new List<string> { "Invalid role: " + role }
                        };
                    }
                }

                // find user with username in database
                var user = await _userManager.FindByIdAsync(request.Id);

                // user with username no existed
                if (user is null)
                {
                    return new BaseResponse<GetBriefApplicationUserResponseModel>()
                    {
                        Success = false,
                        Message = "User not found",
                    };
                }


                // Use reflection to update non-null properties
                foreach (var property in request.GetType().GetProperties())
                {
                    var requestValue = property.GetValue(request);
                    if (requestValue != null)
                    {
                        var targetProperty = user.GetType().GetProperty(property.Name);
                        if (targetProperty != null)
                        {
                            targetProperty.SetValue(user, requestValue);
                        }
                    }
                }

                var updateUserResult = await _userManager.UpdateAsync(user);

                // create new user fail
                if (!updateUserResult.Succeeded)
                {
                    return new BaseResponse<GetBriefApplicationUserResponseModel>()
                    {
                        Success = false,
                        Message = "Update user failed",
                        Errors = updateUserResult.Errors.Select(e => e.Description).ToList()
                    };
                }

            if (request.Roles.Count > 0 && request.Roles != null)
            {

                // Retrieve the current roles of the user
                var currentRoles = await _userManager.GetRolesAsync(user);

                // Remove all current roles from the user
                foreach (var role in currentRoles)
                {
                    await _userManager.RemoveFromRoleAsync(user, role);
                }

                // Add new roles to the user
                foreach (var role in request.Roles)
                {
                    await _userManager.AddToRoleAsync(user, role);
                }
            }

                var mappedApplicationUser = _mapper.Map<GetBriefApplicationUserResponseModel>(user);
                return new BaseResponse<GetBriefApplicationUserResponseModel>()
                {
                    Success = true,
                    Message = "User registered successfully",
                    Data = mappedApplicationUser
                };
            
          
        }
    }
}
