using Domain.Constants;
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

    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
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
            await TrySeedAsync(); // Seeding Role and Default Application Account
            await SeedingData(); // Seeding Default DB
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
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
            Email = "manager@localhost.com",
            UserName = "manager@localhost.com",
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
        var random = new Random();
        for (int i = 0; i < 5; i++)
        {
            var user = new ApplicationUser
            {
                Id = "8e02b95e-6491-4eaf-a75a-06dae6e1ea4" + i.ToString(),
                Email = "TeacherTesting" + i + "@localhost.com",
                UserName = "TeacherTesting" + i + "@localhost.com",
                EmailConfirmed = true
            };
            if (_userManager.Users.FirstOrDefault(u => u.Email.Equals(user.Email)) == null)
            {
                await _userManager.CreateAsync(user, "Abc@123!");
                if (!string.IsNullOrWhiteSpace(teacherRole.Name))
                {
                    await _userManager.AddToRolesAsync(user, new[] { teacherRole.Name });
                }
            }
            await _context.Teachers.AddAsync(new Teacher
            {
                Id = new Guid(),
                ApplicationUserId = user.Id,
                Image = "https://static.vecteezy.com/system/resources/previews/019/153/517/original/avatar-of-a-teacher-character-free-vector.jpg",
                Experience = experiences[i],
                Level = levels[i]
            });
        }
        // Parent Account User
        for (int i = 0; i < 5; i++)
        {
            var user = new ApplicationUser
            {
                Id = "ae8ab566-70fb-445a-81da-a414f04462c" + i.ToString(),
                Email = "ParentTesting" + i + "@localhost.com",
                UserName = "ParentTesting" + i + "@localhost.com",
                EmailConfirmed = true
            };
            if (_userManager.Users.FirstOrDefault(u => u.Email.Equals(user.Email)) == null)
            {
                await _userManager.CreateAsync(user, "Abc@123!");
                if (!string.IsNullOrWhiteSpace(parentRole.Name))
                {
                    await _userManager.AddToRolesAsync(user, new[] { parentRole.Name });
                }
            }
            await _context.Parents.AddAsync(new Parent
            {
                Id = new Guid("4977e82e-9592-475b-a6fa-10942721c6d"+i.ToString()),
                ApplicationUserId = user.Id,
                Address = "Lô E2a-7, Đường D1, Đ. D1, Long Thạnh Mỹ, Thành Phố Thủ Đức, Hồ Chí Minh 700000, Việt Nam",
                Phone = "0987654321",
                Wallet = 0,
                Gender = (Domain.Enums.Gender)(i % 3)
            });
        }
        // Student Account User
        var schools = new List<string> { "Tiểu học Việt Anh", "Tiểu học Quốc tế Hà Nội", "Tiểu học Nam Việt", "Tiểu Học Marie Curie", "Tiểu học Đinh Thiện Lý" };
        var classes = new List<string> { "Lớp 1", "Lớp 2", "Lớp 3", "Lớp 4", "Lớp 5", };
        for (int i = 0; i < 5; i++)
        {
            var user = new ApplicationUser
            {
                Id = "954b8b1b-1b5f-42f6-9e27-4aa65cc7e7b" + i.ToString(),
                Email = "StudentTesting" + i + "@localhost.com",
                UserName = "StudentTesting" + i + "@localhost.com",
                EmailConfirmed = true
            };
            if (_userManager.Users.FirstOrDefault(u => u.Email.Equals(user.Email)) == null)
            {
                await _userManager.CreateAsync(user, "Abc@123!");
                if (!string.IsNullOrWhiteSpace(studentRole.Name))
                {
                    await _userManager.AddToRolesAsync(user, new[] { studentRole.Name });
                }
            }
            await _context.Students.AddAsync(new Student
            {
                Id = new Guid(),
                ApplicationUserId = user.Id,
                ParentId = new Guid("4977e82e-9592-475b-a6fa-10942721c6d" + i.ToString()),
                Image = i,
                School = schools[i],
                Class = classes[i]
            });
        }
    }
  
     
    public async Task SeedingData()
    {
        await _context.Database.BeginTransactionAsync();
        try
        {
            // Application User table
            await _context.ApplicationUsers.AddAsync(new ApplicationUser
            {
                Id = "d7896275-9b79-4955-92f2-e1923b5fa05a",
                UserName = "vinhtc191@gmail.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                AccessFailedCount = 0,
            });
            await _context.ApplicationUsers.AddAsync(new ApplicationUser
            {
                Id = "d7896275-9b79-4955-92f2-e1923b5fa05b",
                UserName = "Taooooo@gmail.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                AccessFailedCount = 0,
            });
            //------------
            // Subject table
            await _context.Subjects.AddAsync(new Subject
            {
                Id = new Guid("14b76851-0f86-4dd2-a59c-ae45893c9578"),
                Title = "Toán",
                Description = "Môn Toán trẻ em giúp phát triển kỹ năng toán học cơ bản cho trẻ qua các hoạt động và trò chơi thú vị. Mục tiêu là tạo nền tảng vững chắc cho sự hiểu biết và tự tin của trẻ khi tiếp cận với toán học."
            });
            await _context.Subjects.AddAsync(new Subject
            {
                Id = new Guid("d7896275-9b79-4955-92f2-e1923b5fa05f"),
                Title = "Khoa học",
                Description = "Môn Khoa học là một phần quan trọng của chương trình giáo dục, giúp học sinh hiểu về thế giới xung quanh thông qua việc nghiên cứu và khám phá các hiện tượng tự nhiên và khoa học. Trong môn này, học sinh được khuyến khích tìm hiểu về các nguyên lý cơ bản của khoa học thông qua các thí nghiệm, quan sát và thảo luận. Mục tiêu của môn Khoa học là khơi dậy sự tò mò, tạo ra nền tảng kiến thức vững chắc và phát triển kỹ năng tư duy logic và phân tích cho học sinh, từ đó giúp họ hiểu biết sâu hơn về thế giới và thúc đẩy sự phát triển cá nhân và xã hội.",
            });
            // ------------------
            // Program type table
            await _context.ProgramTypes.AddAsync(new ProgramType
            {
                Id = new Guid("ed464990-9599-4d6c-99d9-b0ff18be795a"),
                Title = "Toán Cambridge",
                Description = "Chương trình Toán Cambridge dành cho học sinh tiểu học tập trung vào việc phát triển hiểu biết khái niệm, kỹ năng giải quyết vấn đề và lý luận toán học. Học sinh sử dụng các vật dụng học tập như khối đếm và dải phân số để hiểu rõ hơn các khái niệm trừu tượng. Bài toán được đặt trong ngữ cảnh thực tế để làm cho việc học trở nên gần gũi và dễ hiểu hơn. Khuyến khích học sinh giải thích quá trình suy nghĩ và lý luận toán học giúp nâng cao kỹ năng trình bày và suy luận logic. Các bài tập ngắn và các thử thách có giới hạn thời gian được sử dụng để cải thiện sự lưu loát và chính xác trong tính toán."
            });
            await _context.ProgramTypes.AddAsync(new ProgramType
            {
                Id = new Guid("96cd2a6c-1013-4bb9-8927-047fc25e0402"),
                Title = "Toán Tư Duy",
                Description = "Toán tư duy là phương pháp giảng dạy giúp học sinh phát triển khả năng suy luận logic, phân tích và giải quyết vấn đề thông qua các bài toán sáng tạo và thực tế. Thay vì chỉ tập trung vào việc học thuộc lòng các công thức, toán tư duy khuyến khích học sinh hiểu sâu các khái niệm toán học và áp dụng chúng vào các tình huống khác nhau. Phương pháp này sử dụng các bài toán mở, câu hỏi kích thích tư duy và các hoạt động nhóm để thúc đẩy sự tương tác và hợp tác giữa các học sinh. Kết quả là, học sinh không chỉ nắm vững kiến thức toán học mà còn phát triển kỹ năng tư duy phản biện và sáng tạo, chuẩn bị tốt hơn cho việc học tập và ứng dụng toán học trong cuộc sống hàng ngày.",
            });
            await _context.ProgramTypes.AddAsync(new ProgramType
            {
                Id = new Guid("8c368591-a7f0-4679-b059-31c22fa46c1c"),
                Title = "Toán Bộ Giáo Dục",
                Description = "Chương trình Toán học của Bộ Giáo dục và Đào tạo Việt Nam dành cho học sinh tiểu học tập trung vào việc xây dựng nền tảng vững chắc về các khái niệm toán học cơ bản như số học, hình học, đo lường và đại số đơn giản. Học sinh được khuyến khích sử dụng các vật dụng học tập và phương pháp trực quan như khối đếm, bảng biểu và sơ đồ để hiểu rõ hơn về các khái niệm trừu tượng. Bên cạnh đó, chương trình cũng chú trọng đến việc phát triển kỹ năng giải quyết vấn đề thông qua các bài toán thực tế và các hoạt động nhóm, giúp học sinh rèn luyện khả năng tư duy logic và sáng tạo. Việc thực hành thường xuyên và liên tục được ưu tiên nhằm nâng cao sự chính xác và lưu loát trong các phép tính toán học. Chương trình còn khuyến khích sự tham gia của phụ huynh trong quá trình học tập của con em, tạo môi trường học tập tích cực và hỗ trợ tối đa cho học sinh.",
            });
            // ------------------
            // CourseLevel table
            await _context.CourseLevels.AddAsync(new CourseLevel
            {
                Id = new Guid("b8fc90e5-a56f-4ac0-b6bb-cd3eea88d4a1"),
                Title = "Tiền Tiểu Học",
                Description = "Chương trình Toán tư duy cấp Tiền Tiểu Học tập trung vào việc phát triển khả năng tư duy và suy luận logic của trẻ nhỏ thông qua các hoạt động học tập vui nhộn và sáng tạo. Trẻ sẽ học các khái niệm cơ bản về số đếm, hình dạng và mẫu hình học, đồng thời phát triển kỹ năng giải quyết vấn đề qua các trò chơi và bài toán đơn giản liên quan đến cuộc sống hàng ngày."
            });
            await _context.CourseLevels.AddAsync(new CourseLevel
            {
                Id = new Guid("8a7b78a9-d209-473e-a133-919479d61d5c"),
                Title = "Lớp 1",
                Description = "Chương trình Toán cho học sinh lớp 1 tập trung vào việc xây dựng nền tảng toán học vững chắc với các khái niệm số học cơ bản như số đếm, phép cộng và phép trừ. Các bài học sử dụng phương pháp học tập trực quan và thực hành để giúp học sinh hiểu sâu hơn về các khái niệm toán học, kích thích sự hứng thú và tích cực trong việc học toán qua các trò chơi và bài toán thực tế."
            });
            await _context.CourseLevels.AddAsync(new CourseLevel
            {
                Id = new Guid("dd885d8d-0ea4-4c19-9b06-5e02bb44e7bb"),
                Title = "Lớp 2",
                Description = "Chương trình Toán cho học sinh lớp 2 tiếp tục củng cố nền tảng toán học cơ bản với các phép cộng, trừ và các mẫu hình học đơn giản. Các bài học sử dụng phương pháp học tập trực quan, thực hành và trò chơi để phát triển kỹ năng giải quyết vấn đề và tư duy logic của học sinh, chuẩn bị cho những thách thức toán học phức tạp hơn."
            });
            await _context.CourseLevels.AddAsync(new CourseLevel
            {
                Id = new Guid("3f6798e9-eed3-40db-ae92-7dd0da9a6435"),
                Title = "Lớp 3",
                Description = "Chương trình Toán cho học sinh lớp 3 tập trung vào việc mở rộng kiến thức số học với các phép nhân, chia không dư, và các khái niệm hình học như đo đạc đơn vị và diện tích. Học sinh sẽ thực hành và áp dụng kiến thức vào các bài toán thực tế, phát triển kỹ năng tư duy logic và giải quyết vấn đề qua các phương pháp giảng dạy linh hoạt và học tập theo nhóm."
            });
            await _context.CourseLevels.AddAsync(new CourseLevel
            {
                Id = new Guid("6e4cf1a0-6e6b-4b08-a76c-11d3c51d3bea"),
                Title = "Lớp 4",
                Description = "Chương trình Toán cho học sinh lớp 4 tập trung vào việc củng cố và mở rộng kiến thức số học, bao gồm các phép nhân, chia không dư và các khái niệm hình học như diện tích và thể tích. Học sinh sẽ tiếp tục phát triển kỹ năng giải quyết bài toán phức tạp, sử dụng các phương pháp học tập trực quan, thực hành và học tập theo nhóm để tăng cường sự hiểu biết và tích cực trong học tập toán học."
            });
            await _context.CourseLevels.AddAsync(new CourseLevel
            {
                Id = new Guid("9e735658-0002-45c9-a6d5-1a579dba49ee"),
                Title = "Lớp 5",
                Description = "Chương trình Toán cho học sinh lớp 5 tập trung vào việc mở rộng kiến thức số học, hình học và đại số. Học sinh sẽ học về phép nhân, chia với dư, tỉ lệ, phần trăm, và các khái niệm đại số đơn giản. Họ cũng sẽ nghiên cứu về diện tích, thể tích và các hình học không gian. Chương trình giúp học sinh phát triển kỹ năng giải quyết vấn đề, phân tích và tự tin áp dụng kiến thức toán học vào các tình huống thực tế."
            });
            // ------------------
            // Course table
            await _context.Courses.AddAsync(new Course
            {
                Id = new Guid("ceaf0f02-168d-4f69-975f-14a61d492886"),
                SubjectId = new Guid("14b76851-0f86-4dd2-a59c-ae45893c9578"),
                CourseLevelId = new Guid("b8fc90e5-a56f-4ac0-b6bb-cd3eea88d4a1"),
                ProgramTypeId = new Guid("8c368591-a7f0-4679-b059-31c22fa46c1c"),
                Title = "Đại số đại cương",
                Image = "https://static.vecteezy.com/system/resources/previews/013/400/591/non_2x/education-concept-with-cartoon-students-vector.jpg",
                Description = "Đại số là một phần quan trọng của toán học, tập trung vào việc nghiên cứu và giải quyết các vấn đề liên quan đến biểu thức, phương trình và hệ phương trình. Trong đại số, học sinh học về cách tạo ra và giải quyết các biểu thức và phương trình để tìm ra giá trị của các biến số. Điều này có thể bao gồm cả các khái niệm như phép cộng, phép trừ, phép nhân, phép chia, cũng như các phương pháp giải các hệ phương trình.",
                TotalSlot = 20,
            });
            await _context.Courses.AddAsync(new Course
            {
                Id = new Guid("555a0815-d0b8-4975-8e1c-245d7acbab45"),
                SubjectId = new Guid("14b76851-0f86-4dd2-a59c-ae45893c9578"),
                CourseLevelId = new Guid("dd885d8d-0ea4-4c19-9b06-5e02bb44e7bb"),
                ProgramTypeId = new Guid("8c368591-a7f0-4679-b059-31c22fa46c1c"),
                Title = "Hình học đại cương",
                Image = " https://static.vecteezy.com/system/resources/thumbnails/002/399/898/small_2x/education-concept-with-funny-characters-vector.jpg",
                Description = "Hình học nghiên cứu về các hình học cơ bản như hình vuông, hình tròn, tam giác và các hình khác, cũng như các phép biến đổi hình học như tịnh tiến, quay và phản xạ. Học sinh được giáo dục về cách tính diện tích, chu vi và khám phá các tính chất đặc biệt của các hình học này.",
                TotalSlot = 30,
            });
            await _context.Courses.AddAsync(new Course
            {
                Id = new Guid("6c215522-0925-4f86-b0fd-72f746ca9fd6"),
                SubjectId = new Guid("14b76851-0f86-4dd2-a59c-ae45893c9578"),
                CourseLevelId = new Guid("9e735658-0002-45c9-a6d5-1a579dba49ee"),
                ProgramTypeId = new Guid("8c368591-a7f0-4679-b059-31c22fa46c1c"),
                Title = "Số học ứng dụng",
                Image = "https://c8.alamy.com/comp/M71DKY/vector-illustration-of-three-stick-kids-jumping-together-in-the-field-M71DKY.jpg",
                Description = "Số học là nền tảng của toán học, tập trung vào việc nghiên cứu và hiểu về các số và phép tính. Trong số học, học sinh học cách thực hiện các phép toán cơ bản như cộng, trừ, nhân, chia, cũng như các khái niệm như số nguyên tố, bội số chung nhỏ nhất và cách áp dụng chúng vào các bài toán thực tế.",
                TotalSlot = 20,
            });
            await _context.Courses.AddAsync(new Course
            {
                Id = new Guid("c2ad8bc5-d7ef-4639-87b2-d251854656a1"),
                SubjectId = new Guid("14b76851-0f86-4dd2-a59c-ae45893c9578"),
                CourseLevelId = new Guid("3f6798e9-eed3-40db-ae92-7dd0da9a6435"),
                ProgramTypeId = new Guid("96cd2a6c-1013-4bb9-8927-047fc25e0402"),
                Title = "Tư duy Logic",
                Image = "https://i.pinimg.com/736x/fa/3a/1a/fa3a1ac70a55ba27576e41d2335e253c.jpg",
                Description = "Tư duy logic là khả năng quan trọng giúp con người suy luận và giải quyết vấn đề một cách có cấu trúc và logic. Trong giáo dục, việc phát triển tư duy logic không chỉ là mục tiêu của quá trình học tập mà còn là công cụ quan trọng giúp học sinh phát triển kỹ năng suy nghĩ sâu sắc, phân tích thông tin và đưa ra nhận định có chất lượng. Thông qua việc thực hành giải quyết các bài toán phức tạp và tham gia vào các hoạt động nghiên cứu và phân tích, học sinh có cơ hội rèn luyện tư duy logic và áp dụng nó vào các tình huống thực tế, từ đó phát triển sự tự tin và thành công trong học tập và cuộc sống.",
                TotalSlot = 24,
            });
            // --------------
            // Chapter table
            await _context.Chapters.AddAsync(new Chapter
            {
                Id = new Guid("71d018c0-c040-4116-808f-2c3ae70d9ae9"),
                CourseId = new Guid("ceaf0f02-168d-4f69-975f-14a61d492886"),
                Title = "Biểu thức đại số",
                Description = "Giới thiệu về các biểu thức đại số, bao gồm các phép toán cơ bản như cộng, trừ, nhân và chia biểu thức.",
            });
            await _context.Chapters.AddAsync(new Chapter
            {
                Id = new Guid("a4562cbc-b8f8-4bca-b537-77d7b5e4eacc"),
                CourseId = new Guid("ceaf0f02-168d-4f69-975f-14a61d492886"),
                Title = "Phương trình đại số cơ bản",
                Description = "Học cách giải các phương trình đơn giản bằng cách tìm giá trị của biến số.",
            });
            await _context.Chapters.AddAsync(new Chapter
            {
                Id = new Guid("d0f9feee-92c2-4f01-9fc7-4c801c3202c7"),
                CourseId = new Guid("ceaf0f02-168d-4f69-975f-14a61d492886"),
                Title = "Hệ phương trình đại số",
                Description = "Giải quyết các bài toán liên quan đến hệ phương trình đại số bằng các phương pháp như phương pháp loại trừ hoặc phương pháp thế.",
            });
            await _context.Chapters.AddAsync(new Chapter
            {
                Id = new Guid("6b760186-9678-4e66-81f1-cb3aefe56e9f"),
                CourseId = new Guid("ceaf0f02-168d-4f69-975f-14a61d492886"),
                Title = "Bất phương trình và hệ bất phương trình",
                Description = "Nghiên cứu về cách giải và hiểu về các bất phương trình và hệ bất phương trình trong đại số.",
            });
            await _context.Chapters.AddAsync(new Chapter
            {
                Id = new Guid("3f15e8a2-247e-4d79-85f4-d46a73f7782b"),
                CourseId = new Guid("ceaf0f02-168d-4f69-975f-14a61d492886"),
                Title = "Kiểm tra thành tựu",
                Description = "Kiểm tra chung kiến thức về chủ đề Đại số",
            });
            // ------------
            // Topic table
            await _context.Topics.AddAsync(new Topic
            {
                Id = new Guid("6d8c088e-bc7e-409f-8c79-31066d6df42e"),
                ChapterId = new Guid("71d018c0-c040-4116-808f-2c3ae70d9ae9"),
                Title = "Biểu thức đại số cơ bản",
                Description = "Giới thiệu về cấu trúc của biểu thức đại số, bao gồm các phép toán cơ bản như cộng, trừ, nhân và chia.",
            });
            await _context.Topics.AddAsync(new Topic
            {
                Id = new Guid("b78c510e-f524-4a78-a8c1-58c22063e0a0"),
                ChapterId = new Guid("71d018c0-c040-4116-808f-2c3ae70d9ae9"),
                Title = "Rút gọn biểu thức",
                Description = "Học cách rút gọn và đơn giản hóa các biểu thức đại số bằng cách sử dụng các quy tắc phù hợp.",
            });
            await _context.Topics.AddAsync(new Topic
            {
                Id = new Guid("59ee169a-c77f-431b-9cf6-b2c36ab3a4fe"),
                ChapterId = new Guid("71d018c0-c040-4116-808f-2c3ae70d9ae9"),
                Title = "Phân tích và định giá trị của biểu thức",
                Description = "Phân tích cấu trúc và tính chất của các biểu thức đại số và tìm giá trị của chúng cho các giá trị cụ thể của biến số.",
            });
            await _context.Topics.AddAsync(new Topic
            {
                Id = new Guid("afb89987-0405-4641-b595-b634f422cbed"),
                ChapterId = new Guid("71d018c0-c040-4116-808f-2c3ae70d9ae9"),
                Title = "Biểu thức đại số có mũ và căn bậc hai",
                Description = "Nghiên cứu về các biểu thức chứa mũ và căn bậc hai, bao gồm các quy tắc biến đổi và tính toán.",
            });
            await _context.Topics.AddAsync(new Topic
            {
                Id = new Guid("3829a480-2d22-44da-82c1-38da6fd0a6c9"),
                ChapterId = new Guid("71d018c0-c040-4116-808f-2c3ae70d9ae9"),
                Title = "Biểu thức đại số đa biến",
                Description = "Khám phá về cách xử lý và giải quyết các biểu thức đại số có nhiều hơn một biến số.",
            });

            await _context.Topics.AddAsync(new Topic
            {
                Id = new Guid("b4f46040-35ea-43f3-a586-0816f17b219b"),
                ChapterId = new Guid("71d018c0-c040-4116-808f-2c3ae70d9ae9"),
                Title = "Ứng dụng của biểu thức đại số",
                Description = "Áp dụng kiến thức về biểu thức đại số vào các bài toán thực tế trong các lĩnh vực như kinh tế, khoa học, và kỹ thuật.",
            });
            // -----------
            // TeachingSlot table
            await _context.TeachingSlots.AddAsync(new TeachingSlot
            {
                Id = new Guid("3efbbaca-4aa1-45f2-98a0-12fbc2399185"),
                Title = "TeachingSlot Tilte",
                Slot = 1,
                DayInWeek = 3,
                CourseId = new Guid("ceaf0f02-168d-4f69-975f-14a61d492886"),
            });
            await _context.TeachingSlots.AddAsync(new TeachingSlot
            {
                Id = new Guid("811c17cb-527e-4658-9db4-454fdeeca3ef"),
                Slot = 2,
                DayInWeek = 5,
                Title = "TeachingSlot Tilte",
                CourseId = new Guid("ceaf0f02-168d-4f69-975f-14a61d492886"),
            });
            await _context.TeachingSlots.AddAsync(new TeachingSlot
            {
                Id = new Guid("0471144b-8ed3-4e78-b032-d5ca3c5ddfa5"),
                Slot = 1,
                DayInWeek = 6,
                Title = "TeachingSlot Tilte",
                CourseId = new Guid("ceaf0f02-168d-4f69-975f-14a61d492886"),
            });
            // ------------
            // Session table
            await _context.Sessions.AddAsync(new Session
            {
                Id = new Guid("c8f560be-8762-4cb6-bc1f-ad64f3dac67e"),
                TeachingSlotId = new Guid("0471144b-8ed3-4e78-b032-d5ca3c5ddfa5"),
                ApplicationUserId = "8e02b95e-6491-4eaf-a75a-06dae6e1ea40",
                Date = GetRandomDateOnly(),
            });
            await _context.Sessions.AddAsync(new Session
            {
                Id = new Guid("92a70117-01f5-41c2-805a-bcacddc872c1"),
                TeachingSlotId = new Guid("811c17cb-527e-4658-9db4-454fdeeca3ef"),
                ApplicationUserId = "8e02b95e-6491-4eaf-a75a-06dae6e1ea40",
                Date = GetRandomDateOnly(),
            });
            await _context.Sessions.AddAsync(new Session
            {
                Id = new Guid("501aad6e-40e9-4a4e-ba0f-247e1c7f97a0"),
                TeachingSlotId = new Guid("811c17cb-527e-4658-9db4-454fdeeca3ef"),
                ApplicationUserId = "8e02b95e-6491-4eaf-a75a-06dae6e1ea41",
                Date = GetRandomDateOnly(),
            });
            await _context.Sessions.AddAsync(new Session
            {
                Id = new Guid("d4390f9a-f21a-404f-8fdc-5d4b132bb2f3"),
                TeachingSlotId = new Guid("3efbbaca-4aa1-45f2-98a0-12fbc2399185"),
                ApplicationUserId = "8e02b95e-6491-4eaf-a75a-06dae6e1ea43",
                Date = GetRandomDateOnly(),
            });
            await _context.Sessions.AddAsync(new Session
            {
                Id = new Guid("26a7510c-0d5b-4b4b-9775-9578d01120b9"),
                TeachingSlotId = new Guid("0471144b-8ed3-4e78-b032-d5ca3c5ddfa5"),
                ApplicationUserId = "8e02b95e-6491-4eaf-a75a-06dae6e1ea44",
                Date = GetRandomDateOnly(),
            });
            // -----------
            // Enrollment table
            await _context.Enrollments.AddAsync(new Enrollment
            {
                Id = new Guid("091e0476-0b32-412b-9437-e3990a6aa529"),
                CourseId = new Guid("ceaf0f02-168d-4f69-975f-14a61d492886"),
                ApplicationUserId = "d7896275-9b79-4955-92f2-e1923b5fa05a",
                Status = true,
            });
            await _context.Enrollments.AddAsync(new Enrollment
            {
                Id = new Guid("877ddfd8-2ec2-445c-aeaf-1a51a6a40cd5"),
                CourseId = new Guid("555a0815-d0b8-4975-8e1c-245d7acbab45"),
                ApplicationUserId = "d7896275-9b79-4955-92f2-e1923b5fa05b",
                Status = true,
            });
            // -----------
            // Participant table
            await _context.Participants.AddAsync(new Participant
            {
                Id = new Guid("4e21ac1d-1c74-440d-8208-df31fd60aff4"),
                EnrollmentId = new Guid("091e0476-0b32-412b-9437-e3990a6aa529"),
                SessionId = new Guid("c8f560be-8762-4cb6-bc1f-ad64f3dac67e"),
                IsPresent = true,
            });
            await _context.Participants.AddAsync(new Participant
            {
                Id = new Guid("99a17610-466b-403c-b161-0fd728b41dac"),
                EnrollmentId = new Guid("091e0476-0b32-412b-9437-e3990a6aa529"),
                SessionId = new Guid("92a70117-01f5-41c2-805a-bcacddc872c1"),
                IsPresent = true,
            });
            // -----------
            // Teachable table
            await _context.Teachables.AddAsync(new Teachable
            {
                Id = new Guid("82cc29db-7118-40ee-b989-7fff95cc3469"),
                CourseId = new Guid("ceaf0f02-168d-4f69-975f-14a61d492886"),
                ApplicationUserId = "8e02b95e-6491-4eaf-a75a-06dae6e1ea40",
                Status = true,
            });
            await _context.Teachables.AddAsync(new Teachable
            {
                Id = new Guid("871c0c1a-63fe-42f6-87c6-6eb599ee9526"),
                CourseId = new Guid("ceaf0f02-168d-4f69-975f-14a61d492886"),
                ApplicationUserId = "8e02b95e-6491-4eaf-a75a-06dae6e1ea41",
                Status = true,
            });
            await _context.Teachables.AddAsync(new Teachable
            {
                Id = new Guid("d828cbec-50e4-498d-8385-63ed13a8a558"),
                CourseId = new Guid("555a0815-d0b8-4975-8e1c-245d7acbab45"),
                ApplicationUserId = "8e02b95e-6491-4eaf-a75a-06dae6e1ea42",
                Status = true,
            });
            await _context.Teachables.AddAsync(new Teachable
            {
                Id = new Guid("054d8c0c-9037-45d0-84bc-17b1bdc2f28b"),
                CourseId = new Guid("555a0815-d0b8-4975-8e1c-245d7acbab45"),
                ApplicationUserId = "8e02b95e-6491-4eaf-a75a-06dae6e1ea41",
                Status = true,
            });
            await _context.Teachables.AddAsync(new Teachable
            {
                Id = new Guid("ff7a0187-c8a6-478b-aa1e-91d2e6867111"),
                CourseId = new Guid("555a0815-d0b8-4975-8e1c-245d7acbab45"),
                ApplicationUserId = "8e02b95e-6491-4eaf-a75a-06dae6e1ea43",
                Status = true,
            });
            // -----------
            // QuestionLevel table
            await _context.QuestionLevels.AddAsync(new QuestionLevel
            {
                Id = new Guid("26fb0c3c-2f79-4940-ac2c-6ef7ba427d92"),
                Title = "Dễ"
            });
            await _context.QuestionLevels.AddAsync(new QuestionLevel
            {
                Id = new Guid("871d2de9-cfca-4ed0-a9a9-658639d664df"),
                Title = "Trung bình"
            });
            await _context.QuestionLevels.AddAsync(new QuestionLevel
            {
                Id = new Guid("8abb4833-8443-4aab-b996-dc1eff84bd41"),
                Title = "Khá"
            });
            await _context.QuestionLevels.AddAsync(new QuestionLevel
            {
                Id = new Guid("8f45dab4-c1f8-4528-8ca4-ba5f682f847d"),
                Title = "Khó"
            });
            await _context.QuestionLevels.AddAsync(new QuestionLevel
            {
                Id = new Guid("ca44a423-5b69-4953-8e94-8e4b771bef19"),
                Title = "Ác mộng"
            });
            // -----------
            // Question table
            await _context.Questions.AddAsync(new Question
            {
                Id = new Guid("0b886f5c-f730-4189-a8d1-51fe4dd2081d"),
                Content = "Câu hỏi dễ nè...",
                TopicId = new Guid("6d8c088e-bc7e-409f-8c79-31066d6df42e"),
                QuestionLevelId = new Guid("26fb0c3c-2f79-4940-ac2c-6ef7ba427d92")
            });
            await _context.Questions.AddAsync(new Question
            {
                Id = new Guid("f37ae7b9-d211-487a-8f50-ace1ab5c35af"),
                Content = "Câu hỏi khó nè...",
                TopicId = new Guid("6d8c088e-bc7e-409f-8c79-31066d6df42e"),
                QuestionLevelId = new Guid("8f45dab4-c1f8-4528-8ca4-ba5f682f847d")
            });
            // -----------
            // QuestionAnswer table
            // câu hỏi dễ
            await _context.QuestionAnswers.AddAsync(new QuestionAnswer
            {
                Id = new Guid("302a0c60-b02e-402f-971a-c2f46aede894"),
                QuestionId = new Guid("0b886f5c-f730-4189-a8d1-51fe4dd2081d"),
                Content = "Câu trả lời 1",
                IsCorrect = false,
            });
            await _context.QuestionAnswers.AddAsync(new QuestionAnswer
            {
                Id = new Guid("08911be8-9667-45e3-8a91-2d423246d646"),
                QuestionId = new Guid("0b886f5c-f730-4189-a8d1-51fe4dd2081d"),
                Content = "Câu trả lời 2",
                IsCorrect = true,
            });
            await _context.QuestionAnswers.AddAsync(new QuestionAnswer
            {
                Id = new Guid("d14bc339-3ac0-4571-81df-212db5977141"),
                QuestionId = new Guid("0b886f5c-f730-4189-a8d1-51fe4dd2081d"),
                Content = "Câu trả lời 3",
                IsCorrect = false,
            });
            await _context.QuestionAnswers.AddAsync(new QuestionAnswer
            {
                Id = new Guid("ed426ec9-63ed-4c3f-9cc9-538f15fec767"),
                QuestionId = new Guid("0b886f5c-f730-4189-a8d1-51fe4dd2081d"),
                Content = "Câu trả lời 4",
                IsCorrect = false,
            });
            // câu hỏi khó
            await _context.QuestionAnswers.AddAsync(new QuestionAnswer
            {
                Id = new Guid("1e198ad8-b2f6-42b2-8259-31045df4972c"),
                QuestionId = new Guid("f37ae7b9-d211-487a-8f50-ace1ab5c35af"),
                Content = "Câu trả lời 1",
                IsCorrect = false,
            });
            await _context.QuestionAnswers.AddAsync(new QuestionAnswer
            {
                Id = new Guid("73913a99-0cd9-49e4-b615-1504a29ec0ac"),
                QuestionId = new Guid("f37ae7b9-d211-487a-8f50-ace1ab5c35af"),
                Content = "Câu trả lời 2",
                IsCorrect = false,
            });
            await _context.QuestionAnswers.AddAsync(new QuestionAnswer
            {
                Id = new Guid("61062a84-4ec0-4127-825f-7e74eab0aaee"),
                QuestionId = new Guid("f37ae7b9-d211-487a-8f50-ace1ab5c35af"),
                Content = "Câu trả lời 3",
                IsCorrect = true,
            });
            await _context.QuestionAnswers.AddAsync(new QuestionAnswer
            {
                Id = new Guid("afc6229c-a9a5-442a-9b8b-6ef2ab899477"),
                QuestionId = new Guid("f37ae7b9-d211-487a-8f50-ace1ab5c35af"),
                Content = "Câu trả lời 4",
                IsCorrect = true,
            });
            // -----------
            // Game table
            await ContextInitializerHelper.SeedGamesAsync(_context);

            // -----------
            // GameHistories table
            await _context.GameHistories.AddAsync(new GameHistory
            {
                Id = new Guid("f9033f16-8f40-4cce-a687-ac1fac7712a7"),
                GameId = new Guid("3ae42c10-7dbe-4e71-a52c-c19c44e3c4a0"),
                ApplicationUserId = "d7896275-9b79-4955-92f2-e1923b5fa05a",
                Point = 10,
                Duration = 8
            });
            await _context.GameHistories.AddAsync(new GameHistory
            {
                Id = new Guid("cb23cacd-7e73-4c32-a488-6ea58f1beacf"),
                GameId = new Guid("3ae42c10-7dbe-4e71-a52c-c19c44e3c4a0"),
                ApplicationUserId = "d7896275-9b79-4955-92f2-e1923b5fa05b",
                Point = 14,
                Duration = 8
            });
            await _context.GameHistories.AddAsync(new GameHistory
            {
                Id = new Guid("352a4f2a-13a7-4ac7-a92e-3cf900ac4425"),
                GameId = new Guid("3ae42c10-7dbe-4e71-a52c-c19c44e3c4a0"),
                ApplicationUserId = "d7896275-9b79-4955-92f2-e1923b5fa05a",
                Point = 95,
                Duration = 4
            });
            await _context.GameHistories.AddAsync(new GameHistory
            {
                Id = new Guid("6c78454f-f667-4d4c-949a-d81aa082df44"),
                GameId = new Guid("3ae42c10-7dbe-4e71-a52c-c19c44e3c4a0"),
                ApplicationUserId = "d7896275-9b79-4955-92f2-e1923b5fa05b",
                Point = 34,
                Duration = 2
            });
            await _context.GameHistories.AddAsync(new GameHistory
            {
                Id = new Guid("5d1addc2-56b0-4c8e-81ee-9049facac523"),
                GameId = new Guid("49299e7c-fa16-45fd-84e4-1a725c118a9f"),
                ApplicationUserId = "d7896275-9b79-4955-92f2-e1923b5fa05a",
                Point = 34,
                Duration = 3
            });
            await _context.GameHistories.AddAsync(new GameHistory
            {
                Id = new Guid("2c8fb39d-1112-403e-8eb3-2252b21f3307"),
                GameId = new Guid("49299e7c-fa16-45fd-84e4-1a725c118a9f"),
                ApplicationUserId = "d7896275-9b79-4955-92f2-e1923b5fa05b",
                Point = 34,
                Duration = 1
            });

            // -----------
            _context.SaveChanges();
            await _context.Database.CommitTransactionAsync();
        }
        catch (System.Exception)
        {
            await _context.Database.RollbackTransactionAsync();
            throw;
        }
    }
    public static DateOnly GetRandomDateOnly()
    {
        Random random = new Random();
        DateOnly start = new DateOnly(2000, 1, 1);
        int range = (DateTime.Today - start.ToDateTime(TimeOnly.MinValue)).Days;
        return start.AddDays(random.Next(range));
    }


    public class GameSeed
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string ItemStoreJson { get; set; }
        public string AnimalJson { get; set; }
        public Guid Id { get; set; }
        public bool IsDeleted { get; set; }
    }

   
}