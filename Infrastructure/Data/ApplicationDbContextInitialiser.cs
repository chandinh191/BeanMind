﻿using Domain.Constants;
using Domain.Entities;
using Domain.Entities.UserEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using System.ComponentModel;
using Newtonsoft.Json;
using System.Threading;

namespace Infrastructure.Data;

public static class InitialiserExtensions
{
    public static async Task InitialiseDatabaseAsync(IServiceProvider provider)
    {
        using (var scope = provider.CreateScope())
        {
            try
            {
                // access Service provider from scope to get instance of ApplicationDbContextInitialiser
                var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();

                await initialiser.InitialiseAsync();
            
                await initialiser.SeedAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while seeding the database: {ex.Message}");
                //throw new Exception(ex.Message, ex);
            }
        }
    }
}

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;

    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, 
        ApplicationDbContext context, 
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager, 
        IConfiguration configuration)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            await _context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }
    public async Task SeedAsync()
    {
        try
        {
            await SeedingUserAccountAsync(); // Seeding Role and Default Application Account
            await SeedingDataAsync(); // Seeding Default DB
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task SeedingUserAccountAsync()
    {
        // Default Role
        var administratorRole = new IdentityRole(Roles.Administrator);
        var managerRole = new IdentityRole(Roles.Manager);
        var parentRole = new IdentityRole(Roles.Parent);
        var studentRole = new IdentityRole(Roles.Student);
        var teacherRole = new IdentityRole(Roles.Teacher);
        if (_roleManager.Roles.FirstOrDefault(r => r.Name.Equals(managerRole.Name)) == null)
        {
            await _roleManager.CreateAsync(managerRole);
        }
        if (_roleManager.Roles.FirstOrDefault(r => r.Name.Equals(administratorRole.Name)) == null)
        {
            await _roleManager.CreateAsync(administratorRole);
        }
        if (_roleManager.Roles.FirstOrDefault(r => r.Name.Equals(parentRole.Name)) == null)
        {
            await _roleManager.CreateAsync(parentRole);
        }
        if (_roleManager.Roles.FirstOrDefault(r => r.Name.Equals(studentRole.Name)) == null)
        {
            await _roleManager.CreateAsync(studentRole);
        }
        if (_roleManager.Roles.FirstOrDefault(r => r.Name.Equals(teacherRole.Name)) == null)
        {
            await _roleManager.CreateAsync(teacherRole);
        }

        // Default Admin: admin@localhost.com 
        var defaultAdministratorUser = new ApplicationUser
        {
            Email = _configuration["AdministratorAccount:Email"], //"admin@localhost.com",
            UserName = _configuration["AdministratorAccount:UserName"], //"admin@localhost.com",
            EmailConfirmed = true
        };
        if (_userManager.Users.FirstOrDefault(u => u.Email.Equals(defaultAdministratorUser.Email)) == null)
        {
            await _userManager.CreateAsync(defaultAdministratorUser, _configuration["AdministratorAccount:Password"]);
            if (!string.IsNullOrWhiteSpace(administratorRole.Name))
            {
                await _userManager.AddToRolesAsync(defaultAdministratorUser, new[] { administratorRole.Name });
            }
        }

        //Default Manager
        var defaultManagerUser = new ApplicationUser
        {
            Email = "manager@gmail.com",
            UserName = "manager@gmail.com",
            EmailConfirmed = true
        };
        if (_userManager.Users.FirstOrDefault(u => u.Email.Equals(defaultManagerUser.Email)) == null)
        {
            await _userManager.CreateAsync(defaultManagerUser, "Abc@123!");
            if (!string.IsNullOrWhiteSpace(managerRole.Name))
            {
                await _userManager.AddToRolesAsync(defaultManagerUser, new[] { managerRole.Name });
            }
        }

        // Teacher Account User
        var levels = new List<string> { "Thạc sĩ", "Tiến sĩ" , "Giáo sư" , "Thực tập viên" , "Giảng viên" };
        var experiences = new List<string> { "Đang giảng dạy tại FPT University",
            "Đã giảng dạy tại FPT University ở các vị trí tương đương", 
            "8 năm kinh nghiệm giảng dạy cho trẻ nhỏ", 
            "Làm việc cho Nasa 12 năm", 
            "Tốt nghiệp khóa đào tạo giảng dạy chuyên nghiệp" };

        var emailTeacher = new List<string> { "vinhtc191", "maitruongthinh08", "marchjeff145", "tuankiet2911", "trieudoublenguyen" };
        var firstNameTeachers = new List<string> { "Dinh", "Trường", "Văn", "Tuấn", "Nguyên" };
        var lastNameTeachers = new List<string> { "Chần", "Mai", "Nguyễn", "Lê", "Nguyễn" };
        var random = new Random();
        for (int i = 0; i < 5; i++)
        {
            var user = new ApplicationUser
            {
                Id = "8e02b95e-6491-4eaf-a75a-06dae6e1ea4" + i.ToString(),
                Email = emailTeacher[i] + "@gmail.com",
                UserName = emailTeacher[i] + "@gmail.com",
                EmailConfirmed = true,
                FirstName = firstNameTeachers[i],
                LastName = lastNameTeachers[i],
            };
            if (_userManager.Users.FirstOrDefault(u => u.Email.Equals(user.Email)) == null)
            {
                await _userManager.CreateAsync(user, "Abc@123!");
                if (!string.IsNullOrWhiteSpace(teacherRole.Name))
                {
                    await _userManager.AddToRolesAsync(user, new[] { teacherRole.Name });
                }
            }
            var teacher  = (new Teacher
            {
                Id = new Guid(),
                ApplicationUserId = user.Id,
                Image = "https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/vinhdepzai%2Fteacher.png?alt=media&token=d693ecbe-c04d-428f-a7a4-16eafd933d14",
                Experience = experiences[i],
                Level = levels[i]
            });
            await _context.Teachers.AddAsync(teacher);
            user.TeacherId = teacher.Id;
        }

        var emailParents = new List<string> { "kietlt", "trieunn" };
        var firstNameParents = new List<string> { "Kiet", "Trieu" };
        var lastNameParents = new List<string> { "Le", "Nguyen" };
        // Parent Account User
        for (int i = 0; i < 2; i++)
        {
            var user = new ApplicationUser
            {
                Id = "ae8ab566-70fb-445a-81da-a414f04462c" + i.ToString(),
                Email = emailParents[i] + "@gmail.com",
                UserName = emailParents[i] + "@gmail.com",
                EmailConfirmed = true,
                FirstName = firstNameParents[i],
                LastName = lastNameParents[i],
            };
            if (_userManager.Users.FirstOrDefault(u => u.Email.Equals(user.Email)) == null)
            {
                await _userManager.CreateAsync(user, "Abc@123!");
                if (!string.IsNullOrWhiteSpace(parentRole.Name))
                {
                    await _userManager.AddToRolesAsync(user, new[] { parentRole.Name });
                }
            }
           var parent = (new Parent
            {
                Id = new Guid("4977e82e-9592-475b-a6fa-10942721c6d"+i.ToString()),
                ApplicationUserId = user.Id,
                Address = "Lô E2a-7, Đường D1, Đ. D1, Long Thạnh Mỹ, Thành Phố Thủ Đức, Hồ Chí Minh 700000, Việt Nam",
                Phone = "0987654321",
                Wallet = 0,
                Gender = (Domain.Enums.Gender)(i % 3)
            });
            await _context.Parents.AddAsync(parent);
            user.ParentId = parent.Id;
        }

        var emailStudent = new List<string> { "vinhtc", "thinhmt", "tiennv" };
        var firstNameStudents = new List<string> { "Vinh", "Thịnh", "Tiến" };
        var lastNameStudents = new List<string> { "Trần", "Mai", "Nguyễn" };
        // Student Account User
        var schools = new List<string> { "Tiểu học Việt Anh", "Tiểu học Quốc tế Hà Nội", "Tiểu học Nam Việt", "Tiểu Học Marie Curie", "Tiểu học Đinh Thiện Lý" };
        var classes = new List<string> { "Lớp 1", "Lớp 2", "Lớp 3", "Lớp 4", "Lớp 5", };
        for (int i = 0; i < 3; i++)
        {
            var user = new ApplicationUser
            {
                Id = "954b8b1b-1b5f-42f6-9e27-4aa65cc7e7b" + i.ToString(),
                Email = emailStudent[i] + "@gmail.com",
                UserName = emailStudent[i] + "@gmail.com",
                EmailConfirmed = true,
                FirstName = firstNameStudents[i],
                LastName  = lastNameStudents[i],
            };
            if (_userManager.Users.FirstOrDefault(u => u.Email.Equals(user.Email)) == null)
            {
                await _userManager.CreateAsync(user, "Abc@123!");
                if (!string.IsNullOrWhiteSpace(studentRole.Name))
                {
                    await _userManager.AddToRolesAsync(user, new[] { studentRole.Name });
                }
            }
            var student = (new Student
            {
                Id = new Guid(),
                ApplicationUserId = user.Id,
                ParentId = new Guid("4977e82e-9592-475b-a6fa-10942721c6d" + (i%2).ToString() ),
                Image = "https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/vinhdepzai%2Fstudent.png?alt=media&token=c9530e62-817f-420b-9d69-59c67ff8c862",
                School = schools[i],
                Class = classes[i]
            });
            await _context.Students.AddAsync(student);
            user.StudentId = student.Id;
        }
    }
  
    public async Task SeedingDataAsync()
    {
        await _context.Database.BeginTransactionAsync();
        try
        {
            await ContextInitializerHelper.Seed_Subject_CourseLevel_ProgamType_Async(_context);

            await ContextInitializerHelper.Seed_Course_TeachingSlot_Chapter_Topic_Async(_context);

            await ContextInitializerHelper.Seed_Session_Enrollment_Participant_Teachable_Async(_context);

            //Question bank
            await ContextInitializerHelper.Seed_QuestionBank_Async(_context);

            //Game table
            await ContextInitializerHelper.Seed_Game_Async(_context);
            await ContextInitializerHelper.Seed_GameHistory_Async(_context);

            //Worksheet đại pháp
            //Seeding base data for all system data relationships of the worksheet
            //Include WorksheetTemplate, LevelTemaplteRelation, Worksheet, WorksheetQuestion
            await ContextInitializerHelper.Seed_Worksheet_Async(_context); 


            _context.SaveChanges();
            await _context.Database.CommitTransactionAsync();
        }
        catch (System.Exception ex)
        {
            await _context.Database.RollbackTransactionAsync();
            //throw new Exception(ex.Message);
        }
    } 
}