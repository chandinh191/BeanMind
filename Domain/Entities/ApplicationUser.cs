using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public string? LastName { get; set; }
    public string? FirstName { get; set; }
    public int? YearOfBirth { get; set; }
    public DateTime? ParticipationTime { get; set; }

    public IEnumerable<Session>? Sessions { get; set; }
    public IEnumerable<SessionGroup>? SessionGroups { get; set; }
    public IEnumerable<Teachable>? Teachables { get; set; }
    public IEnumerable<Enrollment>? Enrollments { get; set; }
    public IEnumerable<GameHistory>? GameHistories { get; set; }
}
