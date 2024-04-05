using AspectInjector.Broker;
using Leagueinator.Printer.Styles;

namespace Leagueinator.Printer.Aspects {
    [Aspect(Scope.Global)]
    [Injection(typeof(DebugTrace))]
    
    ///
    /// Set the 'Invalid' (bool) property to true when the method is called.
    ///
    public class DebugTrace : Attribute{
        [Advice(Kind.Before, Targets = Target.Any)]
        public void Enter([Argument(Source.Name)] string name, [Argument(Source.Arguments)] object[] arguments) {
            if (arguments.Length > 0) {
                TabbedDebug.StartBlock($"{name}({arguments[0]?.ToString()})");
            }
            else {
                TabbedDebug.StartBlock($"{name}()");
            }            
        }

        [Advice(Kind.After, Targets = Target.Any)]
        public void Exit([Argument(Source.Name)] string name, [Argument(Source.Instance)] object instance) {
            TabbedDebug.EndBlock();
        }
    }
}
