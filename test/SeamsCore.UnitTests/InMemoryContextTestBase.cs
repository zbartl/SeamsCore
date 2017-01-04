using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SeamsCore.Domain;
using SeamsCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeamsCore.UnitTests
{
    public abstract class InMemoryContextTest : TestBase
    {
        /// <summary>
        /// Gets the in-memory database context.
        /// </summary>
        protected SeamsContext Context { get; private set; }

        protected InMemoryContextTest()
        {
            Context = ServiceProvider.GetService<SeamsContext>();
        }
    }
}
