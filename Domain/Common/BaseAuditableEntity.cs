namespace Domain.Common;

public abstract class BaseAuditableEntity : BaseEntity
{
    public DateTime Created { get; set; } = DateTime.Now.AddHours(14);

    public string? CreatedBy { get; set; } = "Admin";   

    public DateTime LastModified { get; set; }

    public string? LastModifiedBy { get; set; }
    public bool IsDeleted { get; set; } = false;

    public DateTime? DeletedDate { get; set; }
    public string? DeletedBy { get; set; }
}
