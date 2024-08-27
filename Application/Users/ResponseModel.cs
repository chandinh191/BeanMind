using Application.Enrollments;
using Application.Parents;
using Application.Sessions;
using Application.Students;
using Application.Teachables;
using Application.Teachers;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.UserEntities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Users;

public sealed record class AccessTokenResponseModel
{
    public string TokenType { get; } = "Bearer";
    public required string AccessToken { get; init; }
    public required long ExpiresIn { get; init; }
    public required string RefreshToken { get; init; }
}

[AutoMap(typeof(ApplicationUser))]
public sealed record class GetUserInfoResponseModel
{
    public required string Id { get; set; }
    public required string UserName { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public int YearOfBirth { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Email { get; set; }
    public required bool EmailConfirmed { get; set; }
    public Guid StudentId { get; set; }
    public GetBriefStudentResponseModel Student { get; set; }
    public Guid TeacherId { get; set; }
    public GetBriefTeacherResponseModel Teacher { get; set; }
    public Guid ParentId { get; set; }
    public GetBriefParentResponseModel Parent { get; set; }
    public List<GetBriefSessionResponseModel>? Sessions { get; set; }
    public List<GetBriefTeachableResponseModel>? Teachables { get; set; }
    public List<GetBriefEnrollmentResponseModel>? Enrollments { get; set; }
    public required List<string> Roles { get; set; }
    public DateTime Created { get; set; }
    public bool IsDeleted { get; set; }
}
[AutoMap(typeof(ApplicationUser))]
public sealed record class GetParentUserInfoResponseModel
{
    public required string Id { get; set; }
    public required string UserName { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public int YearOfBirth { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Email { get; set; }
    public required bool EmailConfirmed { get; set; }
    public Guid ParentId { get; set; }
    public GetParentResponseModel Parent { get; set; }
    public required List<string> Roles { get; set; }
    public DateTime Created { get; set; }
}
