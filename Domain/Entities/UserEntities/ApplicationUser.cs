using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.UserEntities;

public class ApplicationUser : IdentityUser
{
    public string? LastName { get; set; }
    public string? FirstName { get; set; }
    public int? YearOfBirth { get; set; }
    public DateTime? ParticipationTime { get; set; }
    public IEnumerable<Session>? Sessions { get; set; }
    public IEnumerable<Teachable>? Teachables { get; set; }
    public IEnumerable<Enrollment>? Enrollments { get; set; }
    public IEnumerable<GameHistory>? GameHistories { get; set; }

    public bool? IsDeleted { get; set; } = false;
    [ForeignKey(nameof(Student))]
    public Guid? StudentId { get; set; }
    public Student? Student { get; set; }
    [ForeignKey(nameof(Teacher))]
    public Guid? TeacherId { get; set; }
    public Teacher? Teacher { get; set; }
    [ForeignKey(nameof(Parent))]
    public Guid? ParentId { get; set; }
    public Parent? Parent { get; set; }
}
