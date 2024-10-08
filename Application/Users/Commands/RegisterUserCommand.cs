﻿using MediatR;
using Application.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.AspNetCore.Routing;
using Infrastructure.Services;
using System.Text.RegularExpressions;
using Domain.Constants;
using Domain.Entities.UserEntities;
using Microsoft.AspNetCore.Http;
using Application.ApplicationUsers;
using Application.Chapters;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;

namespace Application.Users.Commands;

public record class RegisterUserCommand : IRequest<BaseResponse<GetBriefApplicationUserResponseModel>>
{
    public string? LastName { get; set; }
    public string? FirstName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public required string Username { get; init; }
    public required string Password { get; init; }
    public int? YearOfBirth { get; set; }
    public bool EmailConfirmed { get; set; } = false;
    public required List<string> Roles { get; init; }

}
public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, BaseResponse<GetBriefApplicationUserResponseModel>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;
    private readonly LinkGenerator _linkGenerator;
    private readonly IEmailService _emailService;
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public RegisterUserCommandHandler(UserManager<ApplicationUser> userManager, IConfiguration configuration, LinkGenerator linkGenerator,
        IEmailService emailService, IHttpContextAccessor httpContextAccessor, IMapper mapper, ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
        _mapper = mapper;
        _configuration = configuration;
        _linkGenerator = linkGenerator;
        _emailService = emailService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<BaseResponse<GetBriefApplicationUserResponseModel>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        try
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
            var existedUser = await _userManager.FindByEmailAsync(request.Username);

            // user with username already existed
            if (existedUser is not null)
            {
                return new BaseResponse<GetBriefApplicationUserResponseModel>()
                {
                    Success = false,
                    Message = "Username already existed",
                    Errors = new List<string> { "Username already existed" }
                };
            }

            // create new user
            var user = new ApplicationUser
            {
                Email = request.Username,
                UserName = request.Username,
                LastName = request.LastName,
                FirstName = request.FirstName,
                YearOfBirth = request.YearOfBirth,
                EmailConfirmed = request.EmailConfirmed,
            };
            var createUserResult = await _userManager.CreateAsync(user, request.Password);

            // create new user fail
            if (!createUserResult.Succeeded)
            {
                return new BaseResponse<GetBriefApplicationUserResponseModel>()
                {
                    Success = false,
                    Message = "Create user failed",
                    Errors = createUserResult.Errors.Select(e => e.Description).ToList()
                };
            }

            // add role to user
            foreach (var role in request.Roles)
            {
                await _userManager.AddToRoleAsync(user, role);
                if(role == "Teacher")
                {
                    var teacher = new Teacher
                    {
                        ApplicationUserId = user.Id,
                    };
                    await _context.Teachers.AddAsync(teacher);
                    user.TeacherId = teacher.Id;
                    await _context.SaveChangesAsync(cancellationToken);
                }
                if (role == "Student")
                {
                    var imgStudent = new List<string> { "https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/avatar_images%2Fstudent1.png?alt=media&token=a0e25007-7586-44a6-8517-67d185be33d4",
                        "https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/avatar_images%2Fstudent2.png?alt=media&token=b2ee024c-a7f5-4916-b375-27df30f8a253",
                        "https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/avatar_images%2Fstudent3.png?alt=media&token=65b4135c-f0c4-45f4-99fa-16b5736f5a75",
                        };
                    var random = new Random();
                    int randomIndex = random.Next(imgStudent.Count);
                    var student = new Student
                    {
                        Id = new Guid(),
                        ApplicationUserId = user.Id,
                        Image = imgStudent[randomIndex],
                        School = "",
                        Class = ""
                    };
                    await _context.Students.AddAsync(student);
                    user.StudentId = student.Id;
                    await _context.SaveChangesAsync(cancellationToken);
                }
                if (role == "Parent")
                {
                    var parent = new Parent
                    {
                        ApplicationUserId = user.Id,
                    };
                    await _context.Parents.AddAsync(parent);
                    user.ParentId = parent.Id;
                    await _context.SaveChangesAsync(cancellationToken);
                }
            }
            //await _userManager.UpdateAsync(user);

            // if register by email
            if (IsEmail(request.Username))
            {
                // generate new confirmation token and encode it
                var token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(await _userManager.GenerateEmailConfirmationTokenAsync(user)));

                var queryParams = new Dictionary<string, string>()
                {
                    { "userId", user.Id },
                    { "email", user.Email },
                    { "code", token },
                };
                // Ensure HttpContext is available
                var requestContext = _httpContextAccessor.HttpContext.Request;
                var baseUrl = $"{requestContext.Scheme}://{requestContext.Host}{requestContext.PathBase}" + "/api/v1/auth/confirmEmail";

                var confirmEmailUrl = QueryHelpers.AddQueryString(baseUrl, queryParams);

                // send email
                try
                {
                    await _emailService.SendConfirmMailAsync(user.Email, confirmEmailUrl);
                }
                catch
                {
                    return new BaseResponse<GetBriefApplicationUserResponseModel>()
                    {
                        Success = false,
                        Message = "There is some problem at mail service, please contact with admin at (+84)9276122811"
                    };
                }

                // Return success response after email confirmation process
                return new BaseResponse<GetBriefApplicationUserResponseModel>()
                {
                    Success = true,
                    Message = "User registered successfully, please confirm your email.",
                };
            }
            else
            {
                var mappedApplicationUser = _mapper.Map<GetBriefApplicationUserResponseModel>(user);
                return new BaseResponse<GetBriefApplicationUserResponseModel>()
                {
                    Success = true,
                    Message = "User registered successfully",
                    Data = mappedApplicationUser
                };
            }
        }
        catch (Exception ex)
        {
            return new BaseResponse<GetBriefApplicationUserResponseModel>()
            {
                Success = false,
                Message = ex.Message,
            };
        }
    }

    bool IsEmail(string email)
    {
        // Simple regex pattern for validating email addresses
        string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, emailPattern);
    }
}
