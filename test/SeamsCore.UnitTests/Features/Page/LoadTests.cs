using SeamsCore.Features.Page;

namespace SeamsCore.UnitTests.Features.Page
{
    using SeamsCore.Domain;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Shouldly;
    using Xunit;

    /// <summary>
    /// Unit Tests for Page.Load Query and Handler
    /// </summary>
    public class LoadTests : InMemoryContextTest
    {
        private readonly Page page;
        private readonly Load.Handler handler;

        public LoadTests()
        {
            page = new Page
            {
                Primary = "Foo",
                Secondary = "Bar",
                Tertiary = "Baz",
                Priority = 0,
                Columns = 0,
                IsUserCreated = true,
                Template = new PageTemplate { Name = "Basic", View = "Basic.cshtml" },
                Slots = new List<PageSlot>
                {
                    new PageSlot
                    {
                        SeaId = "foobar-1",
                        PageColumn = 0,
                        Versions = new List<PageSlotHtml>
                        {
                            new PageSlotHtml { Html = "<p>This is html!</p>" }
                        }
                    },
                    new PageSlot
                    {
                        SeaId = "foobar-2",
                        PageColumn = 0,
                        Versions = new List<PageSlotHtml>
                        {
                            new PageSlotHtml { Html = "<p>This is html also!</p>" }
                        }
                    }
                }
            };

            Context.Add(page);
            Context.SaveChanges();

            handler = new Load.Handler(Context);
        }

        [Fact]
        public async Task Should_have_same_routing()
        {
            var query = new Load.Query { Primary = "Foo", Secondary = "Bar", Tertiary = "Baz" };
            var result = await handler.Handle(query);

            result.ShouldNotBeNull();
            result.Primary.ShouldBe(query.Primary);
            result.Secondary.ShouldBe(query.Secondary);
            result.Tertiary.ShouldBe(query.Tertiary);
        }

        [Fact]
        public async Task Should_have_two_slots()
        {
            var query = new Load.Query { Primary = "Foo", Secondary = "Bar", Tertiary = "Baz" };
            var result = await handler.Handle(query);

            result.ShouldNotBeNull();
            result.Slots.ShouldNotBeNull();
            result.Slots.ShouldNotBeEmpty();
            result.Slots.Count.ShouldBe(2);
        }

        [Fact]
        public async Task Should_not_return_page_for_nonexistent()
        {

            var query = new Load.Query { Primary = "Definitely", Secondary = "Not", Tertiary = "APage" };
            var result = await handler.Handle(query);

            result.ShouldBeNull();
        }
    }
}
