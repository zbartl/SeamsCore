using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features.Authentication;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SeamsCore.Domain;
using SeamsCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeamsCore.UnitTests
{
    public abstract class TestBase
    {
        protected IServiceProvider ServiceProvider { get; }

        protected TestBase()
        {
            if (ServiceProvider == null)
            {
                var services = new ServiceCollection();

                // set up empty in-memory test db
                services
                  .AddEntityFrameworkInMemoryDatabase()
                  .AddDbContext<SeamsContext>(options => options.UseInMemoryDatabase().UseInternalServiceProvider(services.BuildServiceProvider()));

                // add http context
                var context = new DefaultHttpContext();
                context.Features.Set<IHttpAuthenticationFeature>(new HttpAuthenticationFeature());
                services.AddSingleton<IHttpContextAccessor>(h => new HttpContextAccessor { HttpContext = context });

                // add automapper (for query handlers)
                services.AddAutoMapper(typeof(SeamsContext));

                // Setup hosting environment
                IHostingEnvironment hostingEnvironment = new HostingEnvironment();
                hostingEnvironment.EnvironmentName = "Development";
                services.AddSingleton(x => hostingEnvironment);

                // set up service provider for tests
                ServiceProvider = services.BuildServiceProvider();
            }
        }
    }
}
