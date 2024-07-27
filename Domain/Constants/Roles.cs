namespace Domain.Constants;

public abstract class Roles
{
    public const string Administrator = "Administrator";
    public const string Manager = "Manager";
    public const string Teacher = "Teacher";
    public const string Parent = "Parent";
    public const string Student = "Student";

    public static readonly HashSet<string> AllRoles = new()
    {
        Administrator, Manager, Teacher, Parent, Student
    };
}
