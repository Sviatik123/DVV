using System;
using System.Collections.Generic;
using System.Text;

namespace SubChoice.Core.Data.Entities
{
    public class StudentSubject
    {
        public Guid StudentId { get; set; }

        public virtual Student Student { get; set; }

        public int SubjectId { get; set; }

        public virtual Subject Subject { get; set; }
    }
}
