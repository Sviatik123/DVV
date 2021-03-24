using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SubChoice.Core.Configuration;
using SubChoice.Core.Data.Entities;
using SubChoice.Core.Interfaces.DataAccess;
using SubChoice.Core.Interfaces.DataAccess.Base;
using SubChoice.DataAccess;
using SubChoice.DataAccess.Repositories;

namespace SubChoice.Configuration
{
    public class DependencyInjectionConfig
    {
        public static void Init(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Services
            

            // Repositories
            services.AddTransient<IUserRepository, UserRepository>();
        }
    }
}
