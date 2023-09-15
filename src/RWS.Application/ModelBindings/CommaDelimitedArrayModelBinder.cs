using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RWS.Application.ModelBindings
{
    public class CommaDelimitedArrayModelBinder : IModelBinder
    {
        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var value = bindingContext.ActionContext.RouteData.Values[bindingContext.FieldName] as string;

            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            var guids = value?.Split(',').Select(Guid.Parse).ToArray();

            bindingContext.Result = ModelBindingResult.Success(guids);

            if (bindingContext.ModelType == typeof(List<Guid>))
            {
                bindingContext.Result = ModelBindingResult.Success(guids.ToList());
            }

            return;
        }
    }
}
