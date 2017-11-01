using System;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Internal;
using Refit;

namespace Jincod.AspNetCore.ActionModelConvention.Refit
{
    public class RefitAttributeConvention : IActionModelConvention
    {
        public void Apply(ActionModel action)
        {
            var types = action.Controller.ControllerType.ImplementedInterfaces;

            foreach (var type in types)
            {
                var mInfo = type.GetMethods()
                    .SingleOrDefault(x => x.Name == action.ActionName);

                if (mInfo == null) continue;

                var refitAttr = (HttpMethodAttribute) Attribute.GetCustomAttributes(mInfo)
                    .Single(x => x is HttpMethodAttribute);

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
                        ActionConstraints = {httpMethodActionConstraint}
                    };

                    if (!string.IsNullOrEmpty(attributeRouteModel.Template))
                        selectorModel.AttributeRouteModel = attributeRouteModel;

                    action.Selectors.Add(selectorModel);
                }
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
