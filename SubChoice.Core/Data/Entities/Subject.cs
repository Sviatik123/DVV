using System;
using System.Collections.Generic;
using System.Text;

namespace SubChoice.Core.Data.Entities
{
    public class Subject : BaseEntity<int>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int StudentsLimit { get; set; }

        public Guid? TeacherId { get; set; }

        public virtual Teacher Teacher { get; set; }

        public virtual ICollection<StudentSubject> StudentSubjects { get; set; }
    }
}
