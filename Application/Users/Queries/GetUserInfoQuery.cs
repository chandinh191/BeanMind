using Application.Common;
using AutoMapper;
using Domain.Entities.UserEntities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Infrastructure.Data;
using Org.BouncyCastle.Math.EC.Rfc7748;

namespace Application.Users.Queries;

public record class GetUserInfoQuery : IRequest<BaseResponse<GetUserInfoResponseModel>>
{
    public required string UserId { get; init; }
}

public class GetUserInfoQueryHandler : IRequestHandler<GetUserInfoQuery, BaseResponse<GetUserInfoResponseModel>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;

    public GetUserInfoQueryHandler(UserManager<ApplicationUser> userManager, IMapper mapper, ApplicationDbContext context)
    {
        _userManager = userManager;
        _mapper = mapper;
        _context = context;
    }

    public async Task<BaseResponse<GetUserInfoResponseModel>> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.ApplicationUsers
            .Include(x => x.Parent)
            .Include(x => x.Student).ThenInclude(o => o.Parent)
            .Include(x => x.Teacher)
            .Include(x => x.Enrollments)
            .Include(x => x.Teachables)
            .Include(x => x.Sessions)
            .FirstOrDefaultAsync(x => x.Id.Equals(request.UserId), cancellationToken);
        // user not found
        if (user is null)
        {
            return new BaseResponse<GetUserInfoResponseModel> { Success = false, Message = "User is not existed" };
        }

        var userRoles = (await _userManager.GetRolesAsync(user)).ToList();

        foreach (var enrollment in user.Enrollments)
        {
            enrollment.PercentTopicCompletion = CactulatePercentTopicCompletion(enrollment.Id, enrollment.CourseId);
            enrollment.PercentWorksheetCompletion = 1.0;
        }
        var userResponse = _mapper.Map<GetUserInfoResponseModel>(user);

        userResponse.Roles = userRoles;

        return new BaseResponse<GetUserInfoResponseModel>
        {
            Success = true,
            Message = "Get user info successfully",
            Data = userResponse,
        };
    }
    public double CactulatePercentTopicCompletion(Guid enrollmentId, Guid courseId)
    {
        var processions = _context.Processions
            .Include(o => o.Participant).ThenInclude(o => o.Enrollment)
            //.Where(o => o.Participant.IsPresent == true && o.Participant.Status == Domain.Enums.ParticipantStatus.Done)
            .Where(o => o.Participant.Enrollment.Id == enrollmentId)
            .ToList();
        var topics = _context.Topics
           .Include(o => o.Chapter).ThenInclude(o => o.Course)
           .Where(o => o.Chapter.Course.Id == courseId)
           .ToList();
        return ((double)processions.Count() / topics.Count()) * 100;
    }
}
