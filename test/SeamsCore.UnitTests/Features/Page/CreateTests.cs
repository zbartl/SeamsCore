using SeamsCore.Features.Page;

namespace SeamsCore.UnitTests.Features.Page
{
    using SeamsCore.Domain;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Shouldly;
    using Xunit;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Unit Tests for Page.Load Query and Handler
    /// </summary>
    public class CreateTests : InMemoryContextTest
    {
        private readonly List<PageTemplate> availableTemplates; 
        private readonly Create.QueryHandler queryHandler;
        private readonly Create.CommandHandler commandHandler;

        public CreateTests()
        {
            availableTemplates = new List<PageTemplate>
            {
                new PageTemplate
                {
                    Id = 1,
                    Name = "A",
                    View = "A.cshtml"
                },
                new PageTemplate
                {
                    Id = 2,
                    Name = "B",
                    View = "B.cshtml"
                }
            };
            foreach (var template in availableTemplates)
            {
                Context.PageTemplates.Add(template);
            }
            Context.SaveChanges();

            queryHandler = new Create.QueryHandler(Context);
            commandHandler = new Create.CommandHandler(Context);
        }

        [Fact]
        public async Task Should_have_correct_template_view()
        {
            // query returns the list of available templates for the requested new tertiary page
            var query = new Create.Query { Primary = "Foo", Secondary = "Bar" };
            var command = await queryHandler.Handle(query);

            // set the command's template to the first created template above
            command.TemplateId = availableTemplates.FirstOrDefault().Id;
            // set the tertiary page to whatever
            command.Tertiary = "Baz";
            await commandHandler.Handle(command);
            // no transaction filter here, so need to call save changes appropriately
            await Context.SaveChangesAsync();

            // our new page should be the only page available
            var page = await Context.Pages.Include(p => p.Template).FirstOrDefaultAsync();

            page.ShouldNotBeNull();
            page.Primary.ShouldBe(query.Primary);
            page.Secondary.ShouldBe(query.Secondary);
            page.Template.View.ShouldBe(availableTemplates.FirstOrDefault().View);
        }
    }
}
