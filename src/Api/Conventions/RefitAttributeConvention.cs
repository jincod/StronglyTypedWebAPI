using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Internal;
using Refit;

namespace Api.Conventions
{
    public class RefitAttributeConvention : IActionModelConvention
    {
        public void Apply(ActionModel action)
        {
            // TODO
            var type = action.Controller.ControllerType.ImplementedInterfaces.Last();

            MethodInfo mInfo = type.GetMethods().First(x => x.Name == action.ActionName);

            HttpMethodAttribute refitAttr =
                (HttpMethodAttribute)Attribute.GetCustomAttributes(mInfo)
                    .First(x => x is HttpMethodAttribute);

            foreach (var selector in action.Selectors)
            {
                var path = refitAttr.Path.StartsWith(action.Controller.ControllerName, true, CultureInfo.InvariantCulture)
                    ? refitAttr.Path.Substring(action.Controller.ControllerName.Length)
                    : refitAttr.Path;

                if (path.StartsWith("/"))
                    path = path.Substring(1);

                if (!string.IsNullOrEmpty(path))
                    selector.AttributeRouteModel = new AttributeRouteModel(new RouteAttribute(path));

                selector.ActionConstraints.Add(new HttpMethodActionConstraint(new[] { refitAttr.Method.Method }));
            }
        }
    }
}
