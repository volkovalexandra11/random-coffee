using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace RandomCoffeeServer.Domain.Hosting;

public class ControllersApplicationPart : ApplicationPart, IApplicationPartTypeProvider
{
    public ControllersApplicationPart(string name, Type[] controllerTypes)
    {
        Name = name;
        Types = controllerTypes.Select(t => t.GetTypeInfo()).ToArray();
    }

    public override string Name { get; }
    public IEnumerable<TypeInfo> Types { get; }
}