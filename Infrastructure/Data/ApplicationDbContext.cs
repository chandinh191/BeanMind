using Domain.Common;
using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public interface IApplicationDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
{

    public DbSet<Question> Question { get; init; }
    public DbSet<QuestionAnswer> QuestionAnswer { get; init; }
    public DbSet<QuestionLevel> QuestionLevel { get; init; }
    public DbSet<Subject> Subject { get; init; }
    public DbSet<Course> Course { get; init; }
    public DbSet<Topic> Topic { get; init; }
    public DbSet<Chapter> Chapter { get; init; }
    public DbSet<Worksheet> Worksheet { get; init; }
    public DbSet<WorksheetTemplate> WorksheetTemplate { get; init; }
    public DbSet<LevelTemplateRelation> LevelTemplateRelation { get; init; }
    public DbSet<Participant> Participant { get; init; }
    public DbSet<Session> Session { get; init; }
    public DbSet<SessionGroup> SessionGroup { get; init; }
    public DbSet<SessionGroupRecord> SessionGroupRecord { get; init; }
    public DbSet<Enrollment> Enrollment { get; init; }
    public DbSet<ProgramType> ProgramType { get; init; }
    public DbSet<CourseLevel> CourseLevel { get; init; }
    public DbSet<Teachable> Teachable { get; init; }
    public DbSet<ApplicationUser> ApplicationUser { get; init; }
    public DbSet<Game> Game { get; init; }
    public DbSet<GameHistory> GameHistory { get; init; }
    public DbSet<ChapterGame> ChapterGame { get; init; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    public override int SaveChanges()
    {
        updateAuditField();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        updateAuditField();
        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<SessionGroup>(session =>
        {
            session
                .HasOne(r => r.ProgramType)
                .WithMany(c => c.SessionGroups)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            session
                .HasOne(r => r.CourseLevel)
                .WithMany(p => p.SessionGroups)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            session
                .HasOne(r => r.ApplicationUser)
                .WithMany(p => p.SessionGroups)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);
        });
        builder.Entity<Participant>(session =>
        {
            session
                .HasOne(r => r.Session)
                .WithMany(c => c.Participants)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            session
                .HasOne(r => r.Enrollment)
                .WithMany(p => p.Participants)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);
        });
    }

    public void updateAuditField()
    {
        var entries = ChangeTracker.Entries<BaseAuditableEntity>();

        foreach (var entry in entries)
        {
            if(entry.State == EntityState.Added)
            {
                entry.Entity.Created = DateTime.Now;
                entry.Entity.CreatedBy = "Admin";
            } else if (entry.State == EntityState.Modified)
            {
                entry.Entity.LastModified = DateTime.Now;
                entry.Entity.LastModifiedBy = "Admin";
            } else if (entry.State == EntityState.Deleted)
            {
                entry.State = EntityState.Modified;
                entry.Entity.IsDeleted = true;
                entry.Entity.DeletedDate = DateTime.Now;
                entry.Entity.DeletedBy = "Admin";
            } 
        }
    }
}
