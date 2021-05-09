using SubChoice.Core.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SubChoice.Core.Data.Dto
{
    public class ApproveUserDto : BaseEntity
    { 
        public bool IsApproved
        {
            get
            {
                return true;
            }
            set
            {
                return;
            }
        }
    }
}
