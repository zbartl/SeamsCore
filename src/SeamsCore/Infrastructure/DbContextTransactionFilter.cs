using System;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace SeamsCore.Infrastructure
{
    /// <summary>
    /// Action Filter executed on each request that wraps request in an Entity Framework transaction.
    /// </summary>
    public class DbContextTransactionFilter : IAsyncActionFilter
    {
        private readonly SeamsContext _dbContext;

        public DbContextTransactionFilter(SeamsContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            try
            {
                _dbContext.BeginTransaction();

                await next();

                await _dbContext.CommitTransactionAsync();
            }
            catch (Exception)
            {
                _dbContext.RollbackTransaction();
                throw;
            }
        }
    }
}
