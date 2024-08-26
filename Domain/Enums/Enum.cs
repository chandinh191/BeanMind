using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum Gender
    {
        Male,
        Female,
        Order
    }
    public enum SortBy
    {
        Ascending,
        Descending
    }
    public enum IsDeleted
    {
        All,
        Active,
        Inactive
    }
    public enum EnrollmentStatus
    {
        OnGoing = 1,
        Complete = 2
    }
    public enum WorksheetAttemptStatus
    {
        NotYet = 1,
        Complete = 2
    }
    public enum ParticipantStatus
    {
        NotYet = 1,
        Present = 2,
        Absent = 3
    }
    public enum OrderStatus
    {
        Pending,
        Completed,
        Cancelled,
    }
    public enum TransactionStatus
    {
        Success,
        Failed,
    }

}

