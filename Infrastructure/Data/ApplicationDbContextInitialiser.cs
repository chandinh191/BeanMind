using Domain.Constants;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data;

public static class InitialiserExtensions
{
    public static async Task InitialiseDatabaseAsync(IServiceProvider provider)
    {
        using (var scope = provider.CreateScope())
        {
            // access Service provider from scope to get instance of ApplicationDbContextInitialiser
            var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();

            await initialiser.InitialiseAsync();

            await initialiser.SeedAsync();
        }
    }
}

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration configuration;

    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        this.configuration = configuration;
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
            await TrySeedAsync();
            await SeedingData();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        // Default Role: Administrator, Manager
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

        // Default User: admin@localhost.com (name and email - RaeKyoAdmin
        var defaultAdministratorUser = new ApplicationUser
        {
            //Email = "admin@localhost.com",
            //UserName = "admin@localhost.com",
            Email = configuration["AdministratorAccount:Email"],
            UserName = configuration["AdministratorAccount:UserName"],
            EmailConfirmed = true
        };

        if (_userManager.Users.FirstOrDefault(u => u.Email.Equals(defaultAdministratorUser.Email)) == null)
        {
            await _userManager.CreateAsync(defaultAdministratorUser, configuration["AdministratorAccount:Password"]);
            if (!string.IsNullOrWhiteSpace(administratorRole.Name))
            {
                await _userManager.AddToRolesAsync(defaultAdministratorUser, new[] { administratorRole.Name });
            }
        }
        //Default User
        var defaultStudentUser = new ApplicationUser
        {
            Email = "student@localhost.com",
            UserName = "student@localhost.com",
            EmailConfirmed = true
        };

        if (_userManager.Users.FirstOrDefault(u => u.Email.Equals(defaultStudentUser.Email)) == null)
        {
            await _userManager.CreateAsync(defaultStudentUser, "Abc@123!");
            if (!string.IsNullOrWhiteSpace(studentRole.Name))
            {
                await _userManager.AddToRolesAsync(defaultStudentUser, new[] { studentRole.Name });
            }
        }
    }

    public async Task SeedingData()
    {
        await _context.Database.BeginTransactionAsync();

        try
        {
            // Application User table
            await _context.ApplicationUser.AddAsync(new ApplicationUser
            {
                Id = "d7896275-9b79-4955-92f2-e1923b5fa05a",
                UserName = "vinhtc191@gmail.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                AccessFailedCount = 0,
            });
            await _context.ApplicationUser.AddAsync(new ApplicationUser
            {
                Id = "d7896275-9b79-4955-92f2-e1923b5fa05b",
                UserName = "vinhtc192@gmail.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                AccessFailedCount = 0,
            });
            //------------
            // Subject table
            await _context.Subject.AddAsync(new Subject
            {
                Id = new Guid("14b76851-0f86-4dd2-a59c-ae45893c9578"),
                Title = "Toán",
                Description = "Môn Toán trẻ em giúp phát triển kỹ năng toán học cơ bản cho trẻ qua các hoạt động và trò chơi thú vị. Mục tiêu là tạo nền tảng vững chắc cho sự hiểu biết và tự tin của trẻ khi tiếp cận với toán học."
            });
            await _context.Subject.AddAsync(new Subject
            {
                Id = new Guid("d7896275-9b79-4955-92f2-e1923b5fa05f"),
                Title = "Khoa học",
                Description = "Môn Khoa học là một phần quan trọng của chương trình giáo dục, giúp học sinh hiểu về thế giới xung quanh thông qua việc nghiên cứu và khám phá các hiện tượng tự nhiên và khoa học. Trong môn này, học sinh được khuyến khích tìm hiểu về các nguyên lý cơ bản của khoa học thông qua các thí nghiệm, quan sát và thảo luận. Mục tiêu của môn Khoa học là khơi dậy sự tò mò, tạo ra nền tảng kiến thức vững chắc và phát triển kỹ năng tư duy logic và phân tích cho học sinh, từ đó giúp họ hiểu biết sâu hơn về thế giới và thúc đẩy sự phát triển cá nhân và xã hội.",
            });
            // ------------------
            // Program type table
            await _context.ProgramType.AddAsync(new ProgramType
            {
                Id = new Guid("ed464990-9599-4d6c-99d9-b0ff18be795a"),
                Title = "Toán Cambridge",
                Description = "Chương trình Toán Cambridge dành cho học sinh tiểu học tập trung vào việc phát triển hiểu biết khái niệm, kỹ năng giải quyết vấn đề và lý luận toán học. Học sinh sử dụng các vật dụng học tập như khối đếm và dải phân số để hiểu rõ hơn các khái niệm trừu tượng. Bài toán được đặt trong ngữ cảnh thực tế để làm cho việc học trở nên gần gũi và dễ hiểu hơn. Khuyến khích học sinh giải thích quá trình suy nghĩ và lý luận toán học giúp nâng cao kỹ năng trình bày và suy luận logic. Các bài tập ngắn và các thử thách có giới hạn thời gian được sử dụng để cải thiện sự lưu loát và chính xác trong tính toán."
            });
            await _context.ProgramType.AddAsync(new ProgramType
            {
                Id = new Guid("96cd2a6c-1013-4bb9-8927-047fc25e0402"),
                Title = "Toán Tư Duy",
                Description = "Toán tư duy là phương pháp giảng dạy giúp học sinh phát triển khả năng suy luận logic, phân tích và giải quyết vấn đề thông qua các bài toán sáng tạo và thực tế. Thay vì chỉ tập trung vào việc học thuộc lòng các công thức, toán tư duy khuyến khích học sinh hiểu sâu các khái niệm toán học và áp dụng chúng vào các tình huống khác nhau. Phương pháp này sử dụng các bài toán mở, câu hỏi kích thích tư duy và các hoạt động nhóm để thúc đẩy sự tương tác và hợp tác giữa các học sinh. Kết quả là, học sinh không chỉ nắm vững kiến thức toán học mà còn phát triển kỹ năng tư duy phản biện và sáng tạo, chuẩn bị tốt hơn cho việc học tập và ứng dụng toán học trong cuộc sống hàng ngày.",
            });
            await _context.ProgramType.AddAsync(new ProgramType
            {
                Id = new Guid("8c368591-a7f0-4679-b059-31c22fa46c1c"),
                Title = "Toán Bộ Giáo Dục",
                Description = "Chương trình Toán học của Bộ Giáo dục và Đào tạo Việt Nam dành cho học sinh tiểu học tập trung vào việc xây dựng nền tảng vững chắc về các khái niệm toán học cơ bản như số học, hình học, đo lường và đại số đơn giản. Học sinh được khuyến khích sử dụng các vật dụng học tập và phương pháp trực quan như khối đếm, bảng biểu và sơ đồ để hiểu rõ hơn về các khái niệm trừu tượng. Bên cạnh đó, chương trình cũng chú trọng đến việc phát triển kỹ năng giải quyết vấn đề thông qua các bài toán thực tế và các hoạt động nhóm, giúp học sinh rèn luyện khả năng tư duy logic và sáng tạo. Việc thực hành thường xuyên và liên tục được ưu tiên nhằm nâng cao sự chính xác và lưu loát trong các phép tính toán học. Chương trình còn khuyến khích sự tham gia của phụ huynh trong quá trình học tập của con em, tạo môi trường học tập tích cực và hỗ trợ tối đa cho học sinh.",
            });
            // ------------------
            // CourseLevel table
            await _context.CourseLevel.AddAsync(new CourseLevel
            {
                Id = new Guid("b8fc90e5-a56f-4ac0-b6bb-cd3eea88d4a1"),
                Title = "Tiền Tiểu Học",
                Description = "Chương trình Toán tư duy dành cho cấp độ Tiền Tiểu Học tập trung vào việc phát triển khả năng suy luận và tư duy logic của trẻ nhỏ thông qua các hoạt động học tập vui nhộn và sáng tạo. Các bài học được thiết kế để khơi dậy sự tò mò và hứng thú của trẻ với toán học bằng cách sử dụng đồ chơi giáo dục, trò chơi, và các bài toán đơn giản liên quan đến cuộc sống hàng ngày. Phương pháp này giúp trẻ hiểu các khái niệm cơ bản về số đếm, hình dạng, và các mẫu hình học, đồng thời phát triển kỹ năng giải quyết vấn đề và khả năng tư duy độc lập. Việc tạo môi trường học tập tích cực và tương tác giúp trẻ không chỉ nắm bắt kiến thức toán học một cách tự nhiên mà còn xây dựng nền tảng vững chắc cho việc học tập trong các cấp học tiếp theo."
            });
            await _context.CourseLevel.AddAsync(new CourseLevel
            {
                Id = new Guid("8a7b78a9-d209-473e-a133-919479d61d5c"),
                Title = "Toán Lớp 1 2",
                Description = "Chương trình Toán cho học sinh lớp 1 và lớp 2 tập trung vào việc xây dựng nền tảng toán học vững chắc và phát triển các kỹ năng toán học cơ bản. Học sinh được giảng dạy về các khái niệm số học cơ bản như số đếm, phép cộng, phép trừ và các mẫu hình học đơn giản như hình vuông, hình tròn và hình tam giác. Các bài học thường sử dụng phương pháp học tập trực quan và thực hành để giúp học sinh hiểu sâu hơn về các khái niệm toán học. Ngoài ra, việc sử dụng trò chơi, hoạt động nhóm và bài toán thực tế giúp kích thích sự hứng thú và tích cực trong việc học toán. Chương trình cũng nhấn mạnh vào việc phát triển kỹ năng giải quyết vấn đề và tư duy logic của học sinh, chuẩn bị cho họ cho những thách thức toán học phức tạp hơn ở cấp độ tiếp theo.",
            });
            await _context.CourseLevel.AddAsync(new CourseLevel
            {
                Id = new Guid("3f6798e9-eed3-40db-ae92-7dd0da9a6435"),
                Title = "Toán Lớp 3 4",
                Description = "Chương trình Toán cho học sinh lớp 3 và lớp 4 tập trung vào việc củng cố và mở rộng kiến thức về các khái niệm số học cơ bản như cộng, trừ, nhân, chia, cũng như các khái niệm hình học và đo lường. Học sinh học về phép nhân đơn giản, chia không dư, và tiếp tục phát triển kỹ năng giải quyết các bài toán có liên quan. Ngoài ra, họ cũng học về các khái niệm hình học như đo đạc đơn vị, diện tích, và dựng các hình học cơ bản. Chương trình tạo cơ hội cho học sinh thực hành và áp dụng kiến thức vào các bài toán thực tế, từ đó phát triển kỹ năng tư duy logic và giải quyết vấn đề. Việc sử dụng các phương pháp giảng dạy linh hoạt như học tập trực quan, thực hành, và học tập theo nhóm giúp tăng cường sự hiểu biết và tích cực trong việc học tập toán học.",
            });
            await _context.CourseLevel.AddAsync(new CourseLevel
            {
                Id = new Guid("9e735658-0002-45c9-a6d5-1a579dba49ee"),
                Title = "Toán Lớp 5",
                Description = "Chương trình Toán cho học sinh lớp 5 tập trung vào việc củng cố và mở rộng kiến thức về các khái niệm số học, hình học và đại số. Học sinh tiếp tục học về phép nhân, chia với dư, và phát triển kỹ năng giải quyết bài toán phức tạp hơn. Ngoài ra, họ cũng học về tỉ lệ, phần trăm, và các khái niệm liên quan đến đại số như biểu thức đơn giản và đại số đơn giản. Trong phần hình học, họ tiếp tục nghiên cứu về diện tích, thể tích, và các hình học không gian như hình hộp chữ nhật và hình hình cầu. Chương trình cũng tập trung vào việc phát triển kỹ năng giải quyết vấn đề, phân tích và tự tin trong việc áp dụng kiến thức toán học vào các tình huống thực tế. Đồng thời, việc sử dụng các phương pháp giảng dạy linh hoạt như học tập trực quan, thực hành, và học tập theo nhóm tiếp tục được ưu tiên để tạo điều kiện học tập tích cực và hiệu quả.",
            });
            // ------------------
            // Course table
            await _context.Course.AddAsync(new Course
            {
                Id = new Guid("ceaf0f02-168d-4f69-975f-14a61d492886"),
                SubjectId = new Guid("14b76851-0f86-4dd2-a59c-ae45893c9578"),
                CourseLevelId = new Guid("9e735658-0002-45c9-a6d5-1a579dba49ee"),
                ProgramTypeId = new Guid("8c368591-a7f0-4679-b059-31c22fa46c1c"),
                Title = "Đại số đại cương",
                Description = "Đại số là một phần quan trọng của toán học, tập trung vào việc nghiên cứu và giải quyết các vấn đề liên quan đến biểu thức, phương trình và hệ phương trình. Trong đại số, học sinh học về cách tạo ra và giải quyết các biểu thức và phương trình để tìm ra giá trị của các biến số. Điều này có thể bao gồm cả các khái niệm như phép cộng, phép trừ, phép nhân, phép chia, cũng như các phương pháp giải các hệ phương trình.",
                TotalSlot = 20,
            });
            await _context.Course.AddAsync(new Course
            {
                Id = new Guid("555a0815-d0b8-4975-8e1c-245d7acbab45"),
                SubjectId = new Guid("14b76851-0f86-4dd2-a59c-ae45893c9578"),
                CourseLevelId = new Guid("9e735658-0002-45c9-a6d5-1a579dba49ee"),
                ProgramTypeId = new Guid("8c368591-a7f0-4679-b059-31c22fa46c1c"),
                Title = "Hình học đại cương",
                Description = "Hình học nghiên cứu về các hình học cơ bản như hình vuông, hình tròn, tam giác và các hình khác, cũng như các phép biến đổi hình học như tịnh tiến, quay và phản xạ. Học sinh được giáo dục về cách tính diện tích, chu vi và khám phá các tính chất đặc biệt của các hình học này.",
                TotalSlot = 20,
            });
            await _context.Course.AddAsync(new Course
            {
                Id = new Guid("6c215522-0925-4f86-b0fd-72f746ca9fd6"),
                SubjectId = new Guid("14b76851-0f86-4dd2-a59c-ae45893c9578"),
                CourseLevelId = new Guid("9e735658-0002-45c9-a6d5-1a579dba49ee"),
                ProgramTypeId = new Guid("8c368591-a7f0-4679-b059-31c22fa46c1c"),
                Title = "Số học ứng dụng",
                Description = "Số học là nền tảng của toán học, tập trung vào việc nghiên cứu và hiểu về các số và phép tính. Trong số học, học sinh học cách thực hiện các phép toán cơ bản như cộng, trừ, nhân, chia, cũng như các khái niệm như số nguyên tố, bội số chung nhỏ nhất và cách áp dụng chúng vào các bài toán thực tế.",
                TotalSlot = 20,
            });
            await _context.Course.AddAsync(new Course
            {
                Id = new Guid("c2ad8bc5-d7ef-4639-87b2-d251854656a1"),
                SubjectId = new Guid("14b76851-0f86-4dd2-a59c-ae45893c9578"),
                CourseLevelId = new Guid("3f6798e9-eed3-40db-ae92-7dd0da9a6435"),
                ProgramTypeId = new Guid("96cd2a6c-1013-4bb9-8927-047fc25e0402"),
                Title = "Tư duy Logic",
                Description = "Tư duy logic là khả năng quan trọng giúp con người suy luận và giải quyết vấn đề một cách có cấu trúc và logic. Trong giáo dục, việc phát triển tư duy logic không chỉ là mục tiêu của quá trình học tập mà còn là công cụ quan trọng giúp học sinh phát triển kỹ năng suy nghĩ sâu sắc, phân tích thông tin và đưa ra nhận định có chất lượng. Thông qua việc thực hành giải quyết các bài toán phức tạp và tham gia vào các hoạt động nghiên cứu và phân tích, học sinh có cơ hội rèn luyện tư duy logic và áp dụng nó vào các tình huống thực tế, từ đó phát triển sự tự tin và thành công trong học tập và cuộc sống.",
                TotalSlot = 20,
            });
            // --------------
            // Chapter table
            await _context.Chapter.AddAsync(new Chapter
            {
                Id = new Guid("71d018c0-c040-4116-808f-2c3ae70d9ae9"),
                CourseId = new Guid("ceaf0f02-168d-4f69-975f-14a61d492886"),
                Title = "Biểu thức đại số",
                Description = "Giới thiệu về các biểu thức đại số, bao gồm các phép toán cơ bản như cộng, trừ, nhân và chia biểu thức.",
            });
            await _context.Chapter.AddAsync(new Chapter
            {
                Id = new Guid("a4562cbc-b8f8-4bca-b537-77d7b5e4eacc"),
                CourseId = new Guid("ceaf0f02-168d-4f69-975f-14a61d492886"),
                Title = "Phương trình đại số cơ bản",
                Description = "Học cách giải các phương trình đơn giản bằng cách tìm giá trị của biến số.",
            });
            await _context.Chapter.AddAsync(new Chapter
            {
                Id = new Guid("d0f9feee-92c2-4f01-9fc7-4c801c3202c7"),
                CourseId = new Guid("ceaf0f02-168d-4f69-975f-14a61d492886"),
                Title = "Hệ phương trình đại số",
                Description = "Giải quyết các bài toán liên quan đến hệ phương trình đại số bằng các phương pháp như phương pháp loại trừ hoặc phương pháp thế.",
            });
            await _context.Chapter.AddAsync(new Chapter
            {
                Id = new Guid("6b760186-9678-4e66-81f1-cb3aefe56e9f"),
                CourseId = new Guid("ceaf0f02-168d-4f69-975f-14a61d492886"),
                Title = "Bất phương trình và hệ bất phương trình",
                Description = "Nghiên cứu về cách giải và hiểu về các bất phương trình và hệ bất phương trình trong đại số.",
            });
            await _context.Chapter.AddAsync(new Chapter
            {
                Id = new Guid("3f15e8a2-247e-4d79-85f4-d46a73f7782b"),
                CourseId = new Guid("ceaf0f02-168d-4f69-975f-14a61d492886"),
                Title = "Kiểm tra thành tựu",
                Description = "Kiểm tra chung kiến thức về chủ đề Đại số",
            });
            // ------------
            // Topic table
            await _context.Topic.AddAsync(new Topic
            {
                Id = new Guid("6d8c088e-bc7e-409f-8c79-31066d6df42e"),
                ChapterId = new Guid("71d018c0-c040-4116-808f-2c3ae70d9ae9"),
                Title = "Biểu thức đại số cơ bản",
                Description = "Giới thiệu về cấu trúc của biểu thức đại số, bao gồm các phép toán cơ bản như cộng, trừ, nhân và chia.",
            });
            await _context.Topic.AddAsync(new Topic
            {
                Id = new Guid("b78c510e-f524-4a78-a8c1-58c22063e0a0"),
                ChapterId = new Guid("71d018c0-c040-4116-808f-2c3ae70d9ae9"),
                Title = "Rút gọn biểu thức",
                Description = "Học cách rút gọn và đơn giản hóa các biểu thức đại số bằng cách sử dụng các quy tắc phù hợp.",
            });
            await _context.Topic.AddAsync(new Topic
            {
                Id = new Guid("59ee169a-c77f-431b-9cf6-b2c36ab3a4fe"),
                ChapterId = new Guid("71d018c0-c040-4116-808f-2c3ae70d9ae9"),
                Title = "Phân tích và định giá trị của biểu thức",
                Description = "Phân tích cấu trúc và tính chất của các biểu thức đại số và tìm giá trị của chúng cho các giá trị cụ thể của biến số.",
            });
            await _context.Topic.AddAsync(new Topic
            {
                Id = new Guid("afb89987-0405-4641-b595-b634f422cbed"),
                ChapterId = new Guid("71d018c0-c040-4116-808f-2c3ae70d9ae9"),
                Title = "Biểu thức đại số có mũ và căn bậc hai",
                Description = "Nghiên cứu về các biểu thức chứa mũ và căn bậc hai, bao gồm các quy tắc biến đổi và tính toán.",
            });
            await _context.Topic.AddAsync(new Topic
            {
                Id = new Guid("3829a480-2d22-44da-82c1-38da6fd0a6c9"),
                ChapterId = new Guid("71d018c0-c040-4116-808f-2c3ae70d9ae9"),
                Title = "Biểu thức đại số đa biến",
                Description = "Khám phá về cách xử lý và giải quyết các biểu thức đại số có nhiều hơn một biến số.",
            });

            await _context.Topic.AddAsync(new Topic
            {
                Id = new Guid("b4f46040-35ea-43f3-a586-0816f17b219b"),
                ChapterId = new Guid("71d018c0-c040-4116-808f-2c3ae70d9ae9"),
                Title = "Ứng dụng của biểu thức đại số",
                Description = "Áp dụng kiến thức về biểu thức đại số vào các bài toán thực tế trong các lĩnh vực như kinh tế, khoa học, và kỹ thuật.",
            });
            // -----------
            // SessionGroup table
            await _context.SessionGroup.AddAsync(new SessionGroup
            {
                Id = new Guid("d0cb5a7d-b3a3-4cd4-acf7-4393f70bee67"),
                CourseLevelId = new Guid("b8fc90e5-a56f-4ac0-b6bb-cd3eea88d4a1"),
                ApplicationUserId = "d7896275-9b79-4955-92f2-e1923b5fa05b",
                ProgramTypeId = new Guid("8c368591-a7f0-4679-b059-31c22fa46c1c"),
                Title = "Tiền tiểu học, bộ giáo dục",
            });
            // -----------
            // Slot table
            await _context.Slot.AddAsync(new Slot
            {
                Id = new Guid("0fdf0d5a-9f6d-41f1-a6fa-81e8db262a23"),
                SlotNumber = 1,
                StartTime = new TimeOnly(7,0),
                EndTime = new TimeOnly(9,0)
            });
            await _context.Slot.AddAsync(new Slot
            {
                Id = new Guid("1c1d13cb-68cc-44ba-a89d-68f72b1ea90e"),
                SlotNumber = 2,
                StartTime = new TimeOnly(9, 30),
                EndTime = new TimeOnly(11, 30)
            });
            await _context.Slot.AddAsync(new Slot
            {
                Id = new Guid("c12f0d96-a70d-4460-a369-5c787824b51a"),
                SlotNumber = 3,
                StartTime = new TimeOnly(13, 0),
                EndTime = new TimeOnly(15, 0)
            });
            await _context.Slot.AddAsync(new Slot
            {
                Id = new Guid("26e79457-6e3f-4beb-9c6b-b6dd3b6c6ab3"),
                SlotNumber = 4,
                StartTime = new TimeOnly(15, 30),
                EndTime = new TimeOnly(17, 30)
            });
            // -----------
            // DayInWeek table
            /*await _context.DayInWeek.AddAsync(new DayInWeek
            {
                Id = new Guid("01b6494d-5d1d-41a5-82ad-636467c6fc72"),
                DayOfTheWeek = 1,
                Available = false
            });
            await _context.DayInWeek.AddAsync(new DayInWeek
            {
                Id = new Guid("a21006db-39ba-4148-98d4-8e9755be24a3"),
                DayOfTheWeek = 2,
                Available = true
            });
            await _context.DayInWeek.AddAsync(new DayInWeek
            {
                Id = new Guid("3b370ec6-b7d9-4c96-92da-a9cc92adfa34"),
                DayOfTheWeek = 3,
                Available = true
            });
            await _context.DayInWeek.AddAsync(new DayInWeek
            {
                Id = new Guid("b3c25590-0d74-4f62-8361-36cfd45d37e3"),
                DayOfTheWeek = 4,
                Available = true
            });
            await _context.DayInWeek.AddAsync(new DayInWeek
            {
                Id = new Guid("eaf7a786-2e76-4fc4-9436-ed32f02cb595"),
                DayOfTheWeek = 5,
                Available = true
            });
            await _context.DayInWeek.AddAsync(new DayInWeek
            {
                Id = new Guid("2018a8f2-c119-4e9d-9d42-ec10b8d86e94"),
                DayOfTheWeek = 6,
                Available = true
            });
            await _context.DayInWeek.AddAsync(new DayInWeek
            {
                Id = new Guid("3010f7ff-489c-4410-a949-4b45b298c83c"),
                DayOfTheWeek = 7,
                Available = false
            });*/
            // -----------
            // SessionGroupRecord table
            await _context.SessionGroupRecord.AddAsync(new SessionGroupRecord
            {
                Id = new Guid("b0aa5504-136d-4e21-8e0c-c1f82547ead3"),
                SessionGroupId = new Guid("d0cb5a7d-b3a3-4cd4-acf7-4393f70bee67"),
                DayInWeek = 2,
                SlotId = new Guid("0fdf0d5a-9f6d-41f1-a6fa-81e8db262a23"),
            });
            await _context.SessionGroupRecord.AddAsync(new SessionGroupRecord
            {
                Id = new Guid("8257f6e9-8f83-4606-8100-f8a747883c4b"),
                SessionGroupId = new Guid("d0cb5a7d-b3a3-4cd4-acf7-4393f70bee67"),
                DayInWeek =3,
                SlotId = new Guid("1c1d13cb-68cc-44ba-a89d-68f72b1ea90e"),
            });
            await _context.SessionGroupRecord.AddAsync(new SessionGroupRecord
            {
                Id = new Guid("fc5e34af-cc89-41a9-b644-5dc87951901c"),
                SessionGroupId = new Guid("d0cb5a7d-b3a3-4cd4-acf7-4393f70bee67"),
                DayInWeek = 5,
                SlotId = new Guid("1c1d13cb-68cc-44ba-a89d-68f72b1ea90e"),
            });
            // -----------
            // Session table
            await _context.Session.AddAsync(new Session
            {
                Id = new Guid("c8f560be-8762-4cb6-bc1f-ad64f3dac67e"),
                SessionGroupRecordId = new Guid("b0aa5504-136d-4e21-8e0c-c1f82547ead3"),
                ApplicationUserId = "d7896275-9b79-4955-92f2-e1923b5fa05b",
                Date = GetRandomDateOnly(),
            });
            await _context.Session.AddAsync(new Session
            {
                Id = new Guid("92a70117-01f5-41c2-805a-bcacddc872c1"),
                SessionGroupRecordId = new Guid("b0aa5504-136d-4e21-8e0c-c1f82547ead3"),
                ApplicationUserId = "d7896275-9b79-4955-92f2-e1923b5fa05b",
                Date = GetRandomDateOnly(),
            });
            await _context.Session.AddAsync(new Session
            {
                Id = new Guid("501aad6e-40e9-4a4e-ba0f-247e1c7f97a0"),
                SessionGroupRecordId = new Guid("b0aa5504-136d-4e21-8e0c-c1f82547ead3"),
                ApplicationUserId = "d7896275-9b79-4955-92f2-e1923b5fa05b",
                Date = GetRandomDateOnly(),
            });
            await _context.Session.AddAsync(new Session
            {
                Id = new Guid("d4390f9a-f21a-404f-8fdc-5d4b132bb2f3"),
                SessionGroupRecordId = new Guid("8257f6e9-8f83-4606-8100-f8a747883c4b"),
                ApplicationUserId = "d7896275-9b79-4955-92f2-e1923b5fa05b",
                Date = GetRandomDateOnly(),
            });
            await _context.Session.AddAsync(new Session
            {
                Id = new Guid("26a7510c-0d5b-4b4b-9775-9578d01120b9"),
                SessionGroupRecordId = new Guid("8257f6e9-8f83-4606-8100-f8a747883c4b"),
                ApplicationUserId = "d7896275-9b79-4955-92f2-e1923b5fa05b",
                Date = GetRandomDateOnly(),
            });
            // -----------
            // Enroll table
            await _context.Enrollment.AddAsync(new Enrollment
            {
                Id = new Guid("091e0476-0b32-412b-9437-e3990a6aa529"),
                CourseId = new Guid("ceaf0f02-168d-4f69-975f-14a61d492886"),
                ApplicationUserId = "d7896275-9b79-4955-92f2-e1923b5fa05a",
                SessionGroupId = new Guid("d0cb5a7d-b3a3-4cd4-acf7-4393f70bee67"),
                Status = true,
            });
            // -----------
            // Participant table
            await _context.Participant.AddAsync(new Participant
            {
                Id = new Guid("4e21ac1d-1c74-440d-8208-df31fd60aff4"),
                EnrollmentId = new Guid("091e0476-0b32-412b-9437-e3990a6aa529"),
                SessionId = new Guid("c8f560be-8762-4cb6-bc1f-ad64f3dac67e"),
                IsPresent = true,
            });
            await _context.Participant.AddAsync(new Participant
            {
                Id = new Guid("99a17610-466b-403c-b161-0fd728b41dac"),
                EnrollmentId = new Guid("091e0476-0b32-412b-9437-e3990a6aa529"),
                SessionId = new Guid("92a70117-01f5-41c2-805a-bcacddc872c1"),
                IsPresent = true,
            });
            // -----------
            // Teachable table
            await _context.Teachable.AddAsync(new Teachable
            {
                Id = new Guid("82cc29db-7118-40ee-b989-7fff95cc3469"),
                CourseLevelId = new Guid("b8fc90e5-a56f-4ac0-b6bb-cd3eea88d4a1"),
                ApplicationUserId = "d7896275-9b79-4955-92f2-e1923b5fa05b",
                ProgramTypeId = new Guid("8c368591-a7f0-4679-b059-31c22fa46c1c"),
                //Title = "Tiền tiểu học, bộ giáo dục",
                Status = true,
            });
            // -----------
            // QuestionLevel table
            await _context.QuestionLevel.AddAsync(new QuestionLevel
            {
                Id = new Guid("26fb0c3c-2f79-4940-ac2c-6ef7ba427d92"),
                Title = "Dễ"
            });
            await _context.QuestionLevel.AddAsync(new QuestionLevel
            {
                Id = new Guid("871d2de9-cfca-4ed0-a9a9-658639d664df"),
                Title = "Trung bình"
            });
            await _context.QuestionLevel.AddAsync(new QuestionLevel
            {
                Id = new Guid("8abb4833-8443-4aab-b996-dc1eff84bd41"),
                Title = "Khá"
            });
            await _context.QuestionLevel.AddAsync(new QuestionLevel
            {
                Id = new Guid("8f45dab4-c1f8-4528-8ca4-ba5f682f847d"),
                Title = "Khó"
            });
            await _context.QuestionLevel.AddAsync(new QuestionLevel
            {
                Id = new Guid("ca44a423-5b69-4953-8e94-8e4b771bef19"),
                Title = "Ác mộng"
            });
            // -----------
            // Question table
            await _context.Question.AddAsync(new Question
            {
                Id = new Guid("0b886f5c-f730-4189-a8d1-51fe4dd2081d"),
                Content = "Câu hỏi dễ nè...",
                TopicId = new Guid("6d8c088e-bc7e-409f-8c79-31066d6df42e"),
                QuestionLevelId = new Guid("26fb0c3c-2f79-4940-ac2c-6ef7ba427d92")
            });
            await _context.Question.AddAsync(new Question
            {
                Id = new Guid("f37ae7b9-d211-487a-8f50-ace1ab5c35af"),
                Content = "Câu hỏi khó nè...",
                TopicId = new Guid("6d8c088e-bc7e-409f-8c79-31066d6df42e"),
                QuestionLevelId = new Guid("8f45dab4-c1f8-4528-8ca4-ba5f682f847d")
            });
            // -----------
            // QuestionAnswer table
            // câu hỏi dễ
            await _context.QuestionAnswer.AddAsync(new QuestionAnswer
            {
                Id = new Guid("302a0c60-b02e-402f-971a-c2f46aede894"),
                QuestionId = new Guid("0b886f5c-f730-4189-a8d1-51fe4dd2081d"),
                Content = "Câu trả lời 1",
                IsCorrect = false,
            });
            await _context.QuestionAnswer.AddAsync(new QuestionAnswer
            {
                Id = new Guid("08911be8-9667-45e3-8a91-2d423246d646"),
                QuestionId = new Guid("0b886f5c-f730-4189-a8d1-51fe4dd2081d"),
                Content = "Câu trả lời 2",
                IsCorrect = true,
            });
            await _context.QuestionAnswer.AddAsync(new QuestionAnswer
            {
                Id = new Guid("d14bc339-3ac0-4571-81df-212db5977141"),
                QuestionId = new Guid("0b886f5c-f730-4189-a8d1-51fe4dd2081d"),
                Content = "Câu trả lời 3",
                IsCorrect = false,
            });
            await _context.QuestionAnswer.AddAsync(new QuestionAnswer
            {
                Id = new Guid("ed426ec9-63ed-4c3f-9cc9-538f15fec767"),
                QuestionId = new Guid("0b886f5c-f730-4189-a8d1-51fe4dd2081d"),
                Content = "Câu trả lời 4",
                IsCorrect = false,
            });
            // câu hỏi khó
            await _context.QuestionAnswer.AddAsync(new QuestionAnswer
            {
                Id = new Guid("1e198ad8-b2f6-42b2-8259-31045df4972c"),
                QuestionId = new Guid("f37ae7b9-d211-487a-8f50-ace1ab5c35af"),
                Content = "Câu trả lời 1",
                IsCorrect = false,
            });
            await _context.QuestionAnswer.AddAsync(new QuestionAnswer
            {
                Id = new Guid("73913a99-0cd9-49e4-b615-1504a29ec0ac"),
                QuestionId = new Guid("f37ae7b9-d211-487a-8f50-ace1ab5c35af"),
                Content = "Câu trả lời 2",
                IsCorrect = false,
            });
            await _context.QuestionAnswer.AddAsync(new QuestionAnswer
            {
                Id = new Guid("61062a84-4ec0-4127-825f-7e74eab0aaee"),
                QuestionId = new Guid("f37ae7b9-d211-487a-8f50-ace1ab5c35af"),
                Content = "Câu trả lời 3",
                IsCorrect = true,
            });
            await _context.QuestionAnswer.AddAsync(new QuestionAnswer
            {
                Id = new Guid("afc6229c-a9a5-442a-9b8b-6ef2ab899477"),
                QuestionId = new Guid("f37ae7b9-d211-487a-8f50-ace1ab5c35af"),
                Content = "Câu trả lời 4",
                IsCorrect = true,
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

}