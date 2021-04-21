using System;
using System.Threading.Tasks;
using SubChoice.Core.Interfaces.Services;

namespace SubChoice.Core.Services
{
    public abstract class BaseService : IBaseService
    {
        protected async Task<TResult> ExecuteAsync<TResult>(Func<TResult> executor)
        {
            return await Task.Run(() => executor());
        }

        protected async Task ExecuteAsync(Action executor)
        {
            await Task.Run(() => executor());
        }
    }
}