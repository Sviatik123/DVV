using SubChoice.Core.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SubChoice.Core.Data.Dto
{
    public class TeacherDto: BaseEntity
    {
        public Guid UserId { get; set; }
    }
}
