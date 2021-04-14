using System;
using System.ComponentModel.DataAnnotations;

namespace SubChoice.Core.Interfaces.DataAccess.Base
{
    public interface IIdentifiable<T> // where T : struct
    {
        [Key]
        T Id { get; set; }
    }

    public interface IRowVersionable
    {
        [Timestamp]
        byte[] RowVersion { get; set; }
    }

    public interface ISaveTrackable
    {
        Guid CreatedBy { get; set; }

        DateTime CreatedOn { get; set; }

        Guid ModifiedBy { get; set; }

        DateTime ModifiedOn { get; set; }
    }

    public interface IInactivebleAt
    {
        DateTime? InactiveAt { get; set; }
    }
}
