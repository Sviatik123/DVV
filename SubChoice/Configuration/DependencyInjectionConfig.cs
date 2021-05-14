using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SubChoice.Core.Interfaces.DataAccess;
using SubChoice.Core.Interfaces.Services;
using SubChoice.DataAccess.Repositories;
using SubChoice.Services;

namespace SubChoice.Configuration
{
    public class DependencyInjectionConfig
    {
        public static void Init(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Services
            services.AddScoped<ILoggerService, LoggerService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ISubjectService, SubjectService>();

            services.AddTransient<SubjectService>();

            // Repositories
            services.AddScoped<IRepoWrapper, RepoWrapper>();
        }
    }
}
