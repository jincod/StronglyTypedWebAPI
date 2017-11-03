# Strongly Typed Web API Example

[![Build status](https://ci.appveyor.com/api/projects/status/suiawclecetsby2r?svg=true)](https://ci.appveyor.com/project/jincod/stronglytypedwebapi)

Using interfaces with [Refit](https://github.com/paulcbetts/refit) attributes.

### Usage

```csharp
using Jincod.AspNetCore.ActionModelConvention.Refit;
// ...

public void ConfigureServices(IServiceCollection services)
{
    // ...
    services.AddMvc(mvc =>
    {
        mvc.Conventions.Add(new RefitAttributeConvention("/api"));
    });
    // ...
}
```
