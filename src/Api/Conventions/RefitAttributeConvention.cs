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

            var mInfo = type.GetMethods().FirstOrDefault(x => x.Name == action.ActionName);

            var refitAttr = (HttpMethodAttribute)Attribute.GetCustomAttributes(mInfo)
                .First(x => x is HttpMethodAttribute);

            var httpMethodActionConstraint = new HttpMethodActionConstraint(new[]
            {
                refitAttr.Method.Method
            });

            var attributeRouteModel = GetAttributeRouteModel(refitAttr.Path, action.Controller.ControllerName);

            if (action.Selectors.Count == 1 && action.Selectors.First().AttributeRouteModel == null)
            {
                var selector = action.Selectors.First();

                if (!string.IsNullOrEmpty(attributeRouteModel.Template))
                    selector.AttributeRouteModel = attributeRouteModel;

                selector.ActionConstraints.Add(httpMethodActionConstraint);
            }
            else
            {
                var selectorModel = new SelectorModel
                {
                    ActionConstraints = { httpMethodActionConstraint }
                };

                if (!string.IsNullOrEmpty(attributeRouteModel.Template))
                    selectorModel.AttributeRouteModel = attributeRouteModel;

                action.Selectors.Add(selectorModel);
            }
        }

        private static AttributeRouteModel GetAttributeRouteModel(string refitAttrPath, string controllerControllerName)
        {
            var path = refitAttrPath.Substring(1);

            if (path.StartsWith(controllerControllerName, true, CultureInfo.InvariantCulture))
                path = path.Substring(controllerControllerName.Length);

            if (path.StartsWith("/"))
                path = path.Substring(1);

            return new AttributeRouteModel(new RouteAttribute(path));
        }
    }
}
