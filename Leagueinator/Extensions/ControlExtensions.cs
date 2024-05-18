using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace Leagueinator.Extensions {
    public static class ControlExtensions {


        public static bool HasTag(this FrameworkElement source, string tag) {
            if (source is null) return false;
            if (source.Tag is null) return false;
            if (source.Tag is not string allTags) return false;

            List<string> split = [.. allTags.Split(" ")];
            return split.Contains(tag);
        }

        public static FrameworkElement? FindElementByTag(this UIElement source, string tag) {
            if (source is null) return null;
            DependencyObject currentObject = source;

            while (currentObject is not null) {
                if (currentObject is not FrameworkElement currentElement) return null;
                if (currentElement.Tag is not null) {
                    if (currentElement.Tag is not string allTags) return null;
                    List<string> split = [.. allTags.Split(" ")];
                    if (split.Contains(tag)) return currentElement;
                }

                if (currentElement.Tag is not null && currentElement.Tag.Equals(tag)) return currentElement;
                currentObject = VisualTreeHelper.GetParent(currentObject);
            }

            return null;
        }

        public static T? FindChildByTag<T>(this DependencyObject parent, string tag) where T : FrameworkElement {
            if (parent is null) return null;
            Queue<DependencyObject> queue = [];
            queue.Enqueue(parent);

            while (queue.Count > 0) {
                DependencyObject current = queue.Dequeue();

                // check each child, if one passes return it, else queue it
                int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
                for (int i = 0; i < childrenCount; i++) {
                    var child = VisualTreeHelper.GetChild(parent, i);
                    if (child is not T childElement) continue;
                    if (childElement.Tag is null) continue;
                    if (childElement.Tag is not string allTags) continue;
                    List<string> split = [.. allTags.Split(" ")];

                    if (split.Contains(tag)) return childElement;
                    queue.Enqueue(child);
                }
            }

            return null;
        }

        public static T? FindChild<T>(this DependencyObject parent) where T : DependencyObject {
            if (parent is null) return null;
            int childCount = VisualTreeHelper.GetChildrenCount(parent);

            // Loop through each child
            for (int i = 0; i < childCount; i++) {
                var child = VisualTreeHelper.GetChild(parent, i);

                // Check if the child is of the specified type
                if (child is T t) return t;

                // Recursively call this method on the child
                var childOfChild = FindChild<T>(child);
                if (childOfChild != null) return childOfChild;
            }

            // If no child of the specified type is found, return null
            return null;
        }

        public static List<T> Descendents<T>(this DependencyObject parent) where T : DependencyObject {
            if (parent is null) return [];
            List<T> descendents = [];
            int childCount = VisualTreeHelper.GetChildrenCount(parent);

            // Loop through each child
            for (int i = 0; i < childCount; i++) {
                var child = VisualTreeHelper.GetChild(parent, i);

                // Check if the child is of the specified type
                if (child is T t) descendents.Add(t);

                // Recursively call this method on the child
                descendents = [.. descendents, .. Descendents<T>(child)];
            }

            // If no child of the specified type is found, return null
            return descendents;
        }

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
