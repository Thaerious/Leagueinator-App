using AspectInjector.Broker;

namespace Leagueinator.Utility {
    [Aspect(Scope.Global)]
    [Injection(typeof(DebugTrace))]
    
    ///
    /// Set the 'Invalid' (bool) property to true when the method is called.
    ///
    public class DebugTrace : Attribute{
        [Advice(Kind.Before, Targets = Target.Any)]
        public void Enter([Argument(Source.Name)] string name, [Argument(Source.Arguments)] object[] arguments) {
            TabbedDebug.StartBlock($"{name}({arguments.DelString()})");
        }

        [Advice(Kind.After, Targets = Target.Any)]
        public void Exit([Argument(Source.Name)] string name, [Argument(Source.Instance)] object instance) {
            TabbedDebug.EndBlock();
        }
    }
}
