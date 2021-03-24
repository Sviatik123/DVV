using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SubChoice.Core.Configuration;
using SubChoice.Core.Data.Entities;
using SubChoice.Core.Interfaces.DataAccess.Base;
using SubChoice.DataAccess;

namespace SubChoice.Configuration
{
    public class DatabaseConfig
    {
        public static void Init(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(connectionString));

            services.AddIdentity<User, Role>()
                .AddRoleManager<RoleManager<Role>>()
                .AddDefaultUI()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<DatabaseContext>();

            services.AddScoped<IUnitOfWork, DatabaseContext>();
        }
    }
}
