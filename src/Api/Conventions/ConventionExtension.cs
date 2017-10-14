using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Conventions
{
    public static class ConventionExtension
    {
        public static void AddConventions(MvcOptions mvc)
        {
            mvc.Conventions.Add(new DefaultRoutePrefixConvention());
            mvc.Conventions.Add(new RefitAttributeConvention());
        }
    }
}
