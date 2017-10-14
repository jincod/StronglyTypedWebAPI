using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Api.Conventions
{
    public class DefaultRoutePrefixConvention : IApplicationModelConvention
    {
        public void Apply(ApplicationModel application)
        {
            foreach (var applicationController in application.Controllers)
            {
                var selector = applicationController.Selectors.FirstOrDefault();
                if (selector != null && selector.AttributeRouteModel == null)
                {
                    selector.AttributeRouteModel = new AttributeRouteModel(new RouteAttribute("api/[controller]"));
                }
            }
        }
    }
}
