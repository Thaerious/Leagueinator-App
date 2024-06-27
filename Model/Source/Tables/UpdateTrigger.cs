using AspectInjector.Broker;
using System.Reflection;

namespace Leagueinator.Model.Tables {
    [Aspect(Scope.Global)]
    [Injection(typeof(UpdateTrigger))]

    public class UpdateTrigger : Attribute {
        private bool preventReentry = false;

        [Advice(Kind.Before, Targets = Target.AnyMember)]
        public void Enter([Argument(Source.Instance)] object instance, [Argument(Source.Name)] string propertyName, [Argument(Source.Arguments)] object[] arguments) {
            if (preventReentry) return;
            preventReentry = true;

            propertyName = propertyName.Substring(4);
            if (instance is not CustomRow customRow) throw new NotSupportedException("Update Trigger must be on properties of a CustomRow");
            PropertyInfo propertyInfo = instance.GetType().GetProperty(propertyName) ?? throw new NullReferenceException($"Property not found {propertyName}");

            object? oldValue = propertyInfo.GetValue(instance);
            object? newValue = arguments[0];

            customRow.InvokeRowUpdated(propertyName, oldValue, newValue);

            preventReentry = false;
        }
    }
}
