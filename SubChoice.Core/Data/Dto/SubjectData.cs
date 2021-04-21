using System;
using SubChoice.Core.Data.Entities;

namespace SubChoice.Core.Data.Dto
{
    public class SubjectData : BaseEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int StudentsLimit { get; set; }

        public Guid TeacherId { get; set; }
    }
}
