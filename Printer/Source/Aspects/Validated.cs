using AspectInjector.Broker;
using System.Reflection;

namespace Leagueinator.Printer.Aspects {
    [Aspect(Scope.Global)]
    [Injection(typeof(Validated))]

    ///
    /// Set the 'Invalid' (bool) property to true when the method is called.
    ///
    public class Validated : Attribute {
        [Advice(Kind.Before, Targets = Target.Any)]
        public void Enter([Argument(Source.Name)] string name, [Argument(Source.Instance)] object instance) {
            PropertyInfo? property = instance.GetType().GetProperty("Invalid");
            if (property is null) throw new NullReferenceException(nameof(property));
            if (property.PropertyType != typeof(bool)) throw new InvalidOperationException("Property 'Invalid' must be of type 'bool'.");
            property.SetValue(instance, true);
        }
    }
}
