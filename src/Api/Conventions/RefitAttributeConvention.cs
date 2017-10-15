using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Internal;
using Models;
using Refit;

namespace Api.Conventions
{
    public class RefitAttributeConvention : IActionModelConvention
    {
        public void Apply(ActionModel action)
        {
            var refitAttributeType = typeof(RefitInterfaceAttribute);
            var type = action.Controller.ControllerType.ImplementedInterfaces
                .FirstOrDefault(x => x.CustomAttributes.Any(a => a.AttributeType == refitAttributeType));

            if (type == null)
                return;

            MethodInfo mInfo = type.GetMethods().FirstOrDefault(x => x.Name == action.ActionName);

            HttpMethodAttribute refitAttr =
                (HttpMethodAttribute)Attribute.GetCustomAttributes(mInfo)
                    .First(x => x is HttpMethodAttribute);

            foreach (var selector in action.Selectors)
            {
                var path = refitAttr.Path.Substring(1);

                if (path.StartsWith(action.Controller.ControllerName, true, CultureInfo.InvariantCulture))
                    path = path.Substring(action.Controller.ControllerName.Length);

                if (path.StartsWith("/"))
                    path = path.Substring(1);

                if (!string.IsNullOrEmpty(path))
                    selector.AttributeRouteModel = new AttributeRouteModel(new RouteAttribute(path));

                selector.ActionConstraints.Add(new HttpMethodActionConstraint(new[] { refitAttr.Method.Method }));
            }
        }
    }
}
