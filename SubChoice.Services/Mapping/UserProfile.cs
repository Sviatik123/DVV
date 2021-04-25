using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using SubChoice.Core.Data.Dto;
using SubChoice.Core.Data.Entities;

namespace SubChoice.Services.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, RegisterDto>().ReverseMap();
        }
    }
}
