using Microsoft.AspNetCore.Mvc.ModelBinding;
using SeamsCore.Domain;
using System.Reflection;

namespace SeamsCore.Infrastructure
{
    public class EntityModelBinderProvider
    {
        //public IModelBinder GetBinder(ModelBinderProviderContext context)
        //{
        //    return typeof(IEntity).IsAssignableFrom(context.Metadata.ModelType) ? new EntityModelBinder() : null;
        //}
    }
}
