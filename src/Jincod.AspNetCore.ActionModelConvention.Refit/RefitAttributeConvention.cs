using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            var refitAttrs = GetRefitAttributes(action.ActionMethod);

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

        private IEnumerable<HttpMethodAttribute> GetRefitAttributes(MethodInfo action)
        {
            IEnumerable<HttpMethodAttribute> GetCustomAttributes(MethodInfo methodInfo) =>
                methodInfo.GetCustomAttributes(typeof(HttpMethodAttribute), true)
                    .Cast<HttpMethodAttribute>();

            IEnumerable<HttpMethodAttribute> Map(InterfaceMapping mapping) => 
                mapping.InterfaceMethods
                    .Zip(mapping.TargetMethods, (im, tm) => (interfaceMethod: im, targetMethod: tm))
                    .Where(x => x.targetMethod == action)
                    .Select(x => x.interfaceMethod)
                    .SelectMany(GetCustomAttributes);

            return action.DeclaringType.GetInterfaces()
                .Select(interfaceType => action.DeclaringType.GetInterfaceMap(interfaceType))
                .SelectMany(Map);
        }
    }
}
