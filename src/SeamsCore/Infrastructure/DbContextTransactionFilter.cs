using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SeamsCore.Infrastructure
{
    /// <summary>
    /// Action Filter executed on each request that wraps request in an Entity Framework transaction.
    /// </summary>
    public class DbContextTransactionFilter : IActionFilter
    {
        private readonly SeamsContext _dbContext;

        public DbContextTransactionFilter(SeamsContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _dbContext.BeginTransaction();
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
            {
                _dbContext.CloseTransaction(context.Exception);
                return;
            }

            try
            {
                _dbContext.CloseTransaction();
            }
            catch (Exception ex)
            {
                _dbContext.CloseTransaction(ex);

                throw;
            }
        }
    }
}
