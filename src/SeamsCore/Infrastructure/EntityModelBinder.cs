﻿using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeamsCore.Infrastructure
{
    public class EntityModelBinder //: IModelBinder
    {
        //public async Task BindModelAsync(ModelBindingContext bindingContext)
        //{
        //    var original = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
        //    if (original != ValueProviderResult.None)
        //    {
        //        var originalValue = original.FirstValue;
        //        int id;
        //        if (int.TryParse(originalValue, out id))
        //        {
        //            var dbContext = bindingContext.HttpContext.RequestServices.GetService<SeamsContext>();
        //            var entity = await dbContext.Set<bindingContext.ModelType>().FindAsync(id);

        //            bindingContext.Result = ModelBindingResult.Success(entity);
        //        }
        //    }
        //}
    }
}
