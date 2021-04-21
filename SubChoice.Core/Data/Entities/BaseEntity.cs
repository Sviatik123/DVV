using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using SubChoice.Core.Interfaces.DataAccess.Base;

namespace SubChoice.Core.Data.Entities
{
    public interface IBaseEntity
    {
    }

    public interface IBaseEntity<TEntity> : IBaseEntity
    {
    }

    public abstract class BaseEntity : IBaseEntity
    {
    }

    public class EmptyEntity : BaseEntity
    {
    }

    public abstract class BaseEntity<T> : BaseEntity, IBaseEntity<T>, IIdentifiable<T>, IRowVersionable, ISaveTrackable, IInactivebleAt
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual T Id { get; set; }

        [Timestamp]
        [Column(Order = 100)]
        public virtual byte[] RowVersion { get; set; }

        [Column(TypeName = "datetime2", Order = 101)]
        public DateTime? InactiveAt { get; set; }

        [Required]
        [Column(Order = 102)]
        public Guid CreatedBy { get; set; }

        [Required]
        [Column(TypeName = "datetime2", Order = 103)]
        public DateTime CreatedOn { get; set; }

        [Required]
        [Column(Order = 104)]
        public Guid ModifiedBy { get; set; }

        [Required]
        [Column(TypeName = "datetime2", Order = 105)]
        public DateTime ModifiedOn { get; set; }
    }
}
