using System.Windows;

namespace Leagueinator.Extensions {
    public static class ControlExtensions {
        public static List<FrameworkElement> Ancestors(this FrameworkElement element) {
            List<FrameworkElement> result = [];
            DependencyObject parent = element.Parent;

            while (parent != null) {
                if (parent is not FrameworkElement parentElement) return result;
                result.Add(parentElement);
                parent = parentElement.Parent;
            }           

            return result;
        }

        public static List<T> Ancestors<T>(this FrameworkElement element) where T : FrameworkElement {
            List<T> result = [];
            DependencyObject parent = element.Parent;

            while (parent != null) {
                if (parent is not FrameworkElement parentElement) return result;
                if (parent is T t) result.Add(t);
                parent = parentElement.Parent;
            }

            return result;
        }
    }
}
