using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Internal;
using Refit;

namespace Jincod.AspNetCore.ActionModelConvention.Refit
{
    public class RefitAttributeConvention : IActionModelConvention
    {
        private readonly string _routePrefix;

        public RefitAttributeConvention(string routePrefix = "")
        {
            _routePrefix = routePrefix;
        }

        public void Apply(ActionModel action)
        {
            var refitAttrs = action.ActionMethod.DeclaringType
                .GetInterfaces()
                .SelectMany(t => GetCustomAttributes(action, t));

            foreach (var refitAttr in refitAttrs)
            {
                var httpMethodActionConstraint = new HttpMethodActionConstraint(new[]
                {
                    refitAttr.Method.Method
                });

                var attributeRouteModel =
                    new AttributeRouteModel(new RouteAttribute($"{_routePrefix}{refitAttr.Path}"));

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

        private IEnumerable<HttpMethodAttribute> GetCustomAttributes(ActionModel action, Type implementedInterface)
        {
            var map = action.ActionMethod.DeclaringType
                .GetInterfaceMap(implementedInterface);

            var index = Array.IndexOf(map.TargetMethods, action.ActionMethod);

            if (index == -1)
                return new HttpMethodAttribute[] { };

            return map.InterfaceMethods[index].GetCustomAttributes(typeof(HttpMethodAttribute), true)
                .Cast<HttpMethodAttribute>();
        }
    }
}
