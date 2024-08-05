using Application.Chapters;
using Application.Common;
using AutoMapper;
using Domain.Entities.UserEntities;
using Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ApplicationUsers.Commands
{
    public sealed record DeleteUserCommand : IRequest<BaseResponse<string>>
    {
        [Required]
        public string Id { get; init; }
    }

    public class DeleteUserCommandHanler : IRequestHandler<DeleteUserCommand, BaseResponse<string>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public DeleteUserCommandHanler(UserManager<ApplicationUser> userManager, ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<BaseResponse<string>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var applicationUser = await _context.ApplicationUsers.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (applicationUser == null)
            {
                return new BaseResponse<string>
                {
                    Success = false,
                    Message = "User not found",
                };
            }
            applicationUser.IsDeleted = true;

            var updateUserResult = _context.Update(applicationUser);

            if (updateUserResult.Entity == null)
            {
                return new BaseResponse<string>
                {
                    Success = false,
                    Message = "Delete user failed",
                };
            }

            await _context.SaveChangesAsync(cancellationToken);

            var user = await _userManager.FindByIdAsync(request.Id);

            if (_userManager.IsInRoleAsync(user, "Teacher").Result)
            {
                var teachables =  _context.Teachables.Where(x => x.ApplicationUserId == user.Id).ToList();
                foreach(var teachable in teachables)
                {
                    teachable.IsDeleted = true;
                }
            }
            await _context.SaveChangesAsync(cancellationToken);

            return new BaseResponse<string>
            {
                Success = true,
                Message = "Delete user successful",
            };
        }
    }

}
