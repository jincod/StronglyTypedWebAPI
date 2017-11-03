# Strongly Typed Web API Example

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
