using System;
using System.Collections.Generic;
using System.Text;
using SubChoice.Core.Data.Entities;

namespace SubChoice.Core.Data.Dto
{
    public class UserData : BaseEntity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
