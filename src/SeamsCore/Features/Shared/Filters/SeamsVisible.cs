using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeamsCore.Features.Shared.Filters
{
    public class SeamsVisible : ActionFilterAttribute
    {
        //This attribute only serves to notify the Page Management that the given action exists.

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
