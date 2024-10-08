﻿using Domain.Common;
using Domain.Entities;
using Domain.Entities.UserEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public interface IApplicationDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
{

    public DbSet<Question> Questions { get; init; }
    public DbSet<QuestionAnswer> QuestionAnswers { get; init; }
    public DbSet<QuestionLevel> QuestionLevels { get; init; }
    public DbSet<Subject> Subjects { get; init; }
    public DbSet<Course> Courses { get; init; }
    public DbSet<Topic> Topics { get; init; }
    public DbSet<Chapter> Chapters { get; init; }
    public DbSet<Worksheet> Worksheets { get; init; }
    public DbSet<WorksheetTemplate> WorksheetTemplates { get; init; }
    public DbSet<LevelTemplateRelation> LevelTemplateRelations { get; init; }
    public DbSet<Participant> Participants { get; init; }
    public DbSet<Session> Sessions { get; init; }
    public DbSet<Enrollment> Enrollments { get; init; }
    public DbSet<ProgramType> ProgramTypes { get; init; }
    public DbSet<CourseLevel> CourseLevels { get; init; }
    public DbSet<Teachable> Teachables { get; init; }
    public DbSet<ApplicationUser> ApplicationUsers { get; init; }
    public DbSet<Game> Games { get; init; }
    public DbSet<GameHistory> GameHistories { get; init; }
    public DbSet<ChapterGame> ChapterGames { get; init; }
    public DbSet<WorksheetAttempt> WorksheetAttempts { get; init; }
    public DbSet<WorksheetQuestion> WorksheetQuestions { get; init; }
    public DbSet<WorksheetAttemptAnswer> WorksheetAttemptAnswers { get; init; }
    public DbSet<TeachingSlot> TeachingSlots { get; init; }
    public DbSet<Procession> Processions { get; init; }
    public DbSet<Teacher> Teachers { get; init; }
    public DbSet<Student> Students { get; init; }
    public DbSet<Parent> Parents { get; init; }
    public DbSet<Order> Orders { get; init; }
    public DbSet<Transaction> Transactions { get; init; }
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
/*        builder.Entity<Student>(student =>
        {
            student
                .HasOne(r => r.Parent)
                .WithMany(p => p.Students)
                .OnDelete(DeleteBehavior.NoAction);
        });*/
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
            if (entry.State == EntityState.Added)
            {
                entry.Entity.Created = DateTime.Now.AddHours(14);
                entry.Entity.CreatedBy = "Admin";
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.LastModified = DateTime.Now;
                entry.Entity.LastModifiedBy = "Admin";
            }
            else if (entry.State == EntityState.Deleted)
            {
                if (entry.Entity is LevelTemplateRelation || entry.Entity is Teachable || entry.Entity is QuestionAnswer || entry.Entity is Procession || entry.Entity is ChapterGame)
                {
                    entry.State = EntityState.Deleted;
                }
                else
                {
                    entry.State = EntityState.Modified;
                    entry.Entity.IsDeleted = true;
                    entry.Entity.DeletedDate = DateTime.Now;
                    entry.Entity.DeletedBy = "Admin";
                }
            }
        }
    }
}
