using System.Reflection;
using BeanMind.Application.Common.Interfaces;
using BeanMind.Domain.Entities;
using BeanMind.Infrastructure.Identity;
using BeanMind.Infrastructure.Persistence.Interceptors;
using Duende.IdentityServer.EntityFramework.Options;
using MediatR;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BeanMind.Infrastructure.Persistence;

public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>, IApplicationDbContext
{
    private readonly IMediator _mediator;
    private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IOptions<OperationalStoreOptions> operationalStoreOptions,
        IMediator mediator,
        AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor)
        : base(options, operationalStoreOptions)
    {
        _mediator = mediator;
        _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
    }

    public DbSet<TodoList> TodoLists => Set<TodoList>();
    public DbSet<TodoItem> TodoItems => Set<TodoItem>();
    public DbSet<Subject> Subjects => Set<Subject>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        builder.Entity<Subject>()
       .HasData(
           new Subject
           {
               Id = new Guid("14b76851-0f86-4dd2-a59c-ae45893c9578"),
               Name = "Toán",
               Description = "Môn Toán trẻ em là một khung chương trình giáo dục nhằm giúp trẻ em phát triển kỹ năng toán học từ khi còn nhỏ. Trong môn này, các khái niệm toán học được trình bày một cách đơn giản và thú vị thông qua các hoạt động, trò chơi và bài tập phù hợp với độ tuổi và khả năng của trẻ. Mục tiêu chính là giúp trẻ phát triển kỹ năng logic, tư duy, và sự tự tin khi tiếp cận với các vấn đề toán học cơ bản. Đồng thời, môn Toán trẻ em cũng khuyến khích sự sáng tạo và khám phá của trẻ thông qua việc áp dụng những khái niệm toán học vào cuộc sống hàng ngày của họ.",
               IsDeleted = false
           },
           new Subject
           {
               Id = new Guid("d7896275-9b79-4955-92f2-e1923b5fa05f"),
               Name = "Khoa học",
               Description = "Môn Khoa học là một phần quan trọng của chương trình giáo dục, giúp học sinh hiểu về thế giới xung quanh thông qua việc nghiên cứu và khám phá các hiện tượng tự nhiên và khoa học. Trong môn này, học sinh được khuyến khích tìm hiểu về các nguyên lý cơ bản của khoa học thông qua các thí nghiệm, quan sát và thảo luận. Mục tiêu của môn Khoa học là khơi dậy sự tò mò, tạo ra nền tảng kiến thức vững chắc và phát triển kỹ năng tư duy logic và phân tích cho học sinh, từ đó giúp họ hiểu biết sâu hơn về thế giới và thúc đẩy sự phát triển cá nhân và xã hội.",
               IsDeleted = false
           }
       );

        builder.Entity<Topic>()
       .HasData(
           new Topic
           {
               Id = new Guid("ceaf0f02-168d-4f69-975f-14a61d492886"),
               SubjectId = new Guid("14b76851-0f86-4dd2-a59c-ae45893c9578"),
               Name = "Đại số",
               Description = "Đại số là một phần quan trọng của toán học, tập trung vào việc nghiên cứu và giải quyết các vấn đề liên quan đến biểu thức, phương trình và hệ phương trình. Trong đại số, học sinh học về cách tạo ra và giải quyết các biểu thức và phương trình để tìm ra giá trị của các biến số. Điều này có thể bao gồm cả các khái niệm như phép cộng, phép trừ, phép nhân, phép chia, cũng như các phương pháp giải các hệ phương trình.",
               ImageURL = "https://vnmedia2.monkeyuni.net/upload/web/storage_web/13-04-2022_18:20:13_toan-lop-2-phep-chia.jpg",
               Status = true,
               IsDeleted = false
           },
           new Topic
           {
               Id = new Guid("555a0815-d0b8-4975-8e1c-245d7acbab45"),
               SubjectId = new Guid("14b76851-0f86-4dd2-a59c-ae45893c9578"),
               Name = "Hình học",
               Description = "Hình học nghiên cứu về các hình học cơ bản như hình vuông, hình tròn, tam giác và các hình khác, cũng như các phép biến đổi hình học như tịnh tiến, quay và phản xạ. Học sinh được giáo dục về cách tính diện tích, chu vi và khám phá các tính chất đặc biệt của các hình học này.",
               ImageURL = "https://giasuviet.net.vn/app/uploads/2017/11/ph%C6%B0%C6%A1ng-ph%C3%A1p-t%E1%BB%91t-nh%E1%BA%A5t-gi%C3%BAp-b%C3%A9-h%E1%BB%8Dc-to%C3%A1n.png",
               Status = true,
               IsDeleted = false
           },
           new Topic
           {
               Id = new Guid("6c215522-0925-4f86-b0fd-72f746ca9fd6"),
               SubjectId = new Guid("14b76851-0f86-4dd2-a59c-ae45893c9578"),
               Name = "Số học",
               Description = "Số học là nền tảng của toán học, tập trung vào việc nghiên cứu và hiểu về các số và phép tính. Trong số học, học sinh học cách thực hiện các phép toán cơ bản như cộng, trừ, nhân, chia, cũng như các khái niệm như số nguyên tố, bội số chung nhỏ nhất và cách áp dụng chúng vào các bài toán thực tế.",
               ImageURL = "https://png.pngtree.com/png-clipart/20210310/ourlarge/pngtree-math-clipart-cartoon-numbers-png-image_2997366.jpg",
               Status = true,
               IsDeleted = false
           }
       );
        builder.Entity<Lession>()
      .HasData(
          new Lession
          {
              Id = new Guid("71d018c0-c040-4116-808f-2c3ae70d9ae9"),
              TopicId = new Guid("ceaf0f02-168d-4f69-975f-14a61d492886"),
              Title = "Biểu thức đại số",
              Description = "Giới thiệu về các biểu thức đại số, bao gồm các phép toán cơ bản như cộng, trừ, nhân và chia biểu thức.",
              Order = 1,
              IsDeleted = false
          },
          new Lession
          {
              Id = new Guid("a4562cbc-b8f8-4bca-b537-77d7b5e4eacc"),
              TopicId = new Guid("ceaf0f02-168d-4f69-975f-14a61d492886"),
              Title = "Phương trình đại số cơ bản",
              Description = "Học cách giải các phương trình đơn giản bằng cách tìm giá trị của biến số.",         
              Order = 2,
              IsDeleted = false
          },
           new Lession
           {
               Id = new Guid("ec7437a6-76b1-4ac0-ab93-20e3a21dd929"),
               TopicId = new Guid("ceaf0f02-168d-4f69-975f-14a61d492886"),
               Title = "Kiểm tra lần 1",
               Description = "Kiểm tra Biểu thức đại số và Phương trình đại số cơ bản",
               Order = 3,
               IsDeleted = false
           },
          new Lession
          {
              Id = new Guid("d0f9feee-92c2-4f01-9fc7-4c801c3202c7"),
              TopicId = new Guid("ceaf0f02-168d-4f69-975f-14a61d492886"),
              Title = "Hệ phương trình đại số",
              Description = "Giải quyết các bài toán liên quan đến hệ phương trình đại số bằng các phương pháp như phương pháp loại trừ hoặc phương pháp thế.",
              Order = 4,
              IsDeleted = false
          },
           new Lession
           {
               Id = new Guid("6b760186-9678-4e66-81f1-cb3aefe56e9f"),
               TopicId = new Guid("ceaf0f02-168d-4f69-975f-14a61d492886"),
               Title = "Bất phương trình và hệ bất phương trình",
               Description = "Nghiên cứu về cách giải và hiểu về các bất phương trình và hệ bất phương trình trong đại số.",
               Order = 5,
               IsDeleted = false
           },
           new Lession
           {
               Id = new Guid("9650cb9a-d8a8-4ca7-a132-8fa8cc01b171"),
               TopicId = new Guid("ceaf0f02-168d-4f69-975f-14a61d492886"),
               Title = "Kiểm tra lần 2",
               Description = "Kiểm tra Hệ phương trình đại số và Bất phương trình và hệ bất phương trình",
               Order = 6,
               IsDeleted = false
           },
           new Lession
           {
               Id = new Guid("3f15e8a2-247e-4d79-85f4-d46a73f7782b"),
               TopicId = new Guid("ceaf0f02-168d-4f69-975f-14a61d492886"),
               Title = "Kiểm tra thành tựu",
               Description = "Kiểm tra chung kiến thức về chủ đề Đại số",
               Order = 7,
               IsDeleted = false
           }

      );


        base.OnModelCreating(builder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _mediator.DispatchDomainEvents(this);

        return await base.SaveChangesAsync(cancellationToken);
    }
}
