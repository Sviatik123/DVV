using System;
using System.Collections.Generic;
using System.Text;

namespace SubChoice.Core.Data.Entities
{
    public class Teacher : BaseEntity<Guid>
    {
        public virtual User User { get; set; }

        public Guid UserId { get; set; }

        public virtual ICollection<Subject> Subjects { get; set; }
    }
}
