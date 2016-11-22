using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HtmlTags;
using HtmlTags.Conventions;

namespace SeamsCore.Infrastructure.Tags
{
    public class TagConventions : HtmlConventionRegistry
    {
        public TagConventions()
        {
            this.Defaults();
        }
    }
}
