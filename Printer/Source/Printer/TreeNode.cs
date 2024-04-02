using Leagueinator.Printer.Aspects;
using System.Collections.ObjectModel;

namespace Leagueinator.Printer {
    class InvalidType {
        public bool IsInvalid { get; set; }
    }

    public class TreeNode<T> where T : TreeNode<T> {
        public ReadOnlyCollection<T> Children { get => new(this._children); }
        public T? Parent { get => (T?)this._parent; }
        public bool IsRoot { get => this.Parent == null; }
        public bool IsLeaf { get => this.Children.Count == 0; }
        public bool Invalid {
            get => this._invalid.IsInvalid;
            set => this._invalid.IsInvalid = value;
        }

        /// <summary>
        /// Add a single child to this node.
        /// If the child current already has a _parent an exception will be thrown.
        /// be updated to this child.
        /// </summary>
        /// <param name="child"></param>
        /// <returns></returns>
        [Validated]
        public virtual void AddChild(T child) {
            if (child.Parent is not null) throw new InvalidOperationException("Child TreeNode<T> already has a parent");
            this._children.Add(child);
            child._parent = this;
            child._invalid = this._invalid;
        }

        /// <summary>
        /// Remove all child TreeNode<T>s from this child.
        /// </summary>
        [Validated]
        public virtual void ClearChildren() {
            foreach (T child in new List<TreeNode<T>>(this.Children).Cast<T>()) {
                this.Detach(child);
            }
        }

        /// <summary>
        /// Remove a child current from this current.
        /// </summary>
        /// <param name="child"></param>
        /// <exception cref="Exception">If the child does not belong to this _parent.</exception>
        [Validated]
        public virtual void Detach(T child) {
            this._children.Remove(child);
            child._parent = null;
            child._invalid = new() { IsInvalid = true };
            this.Invalid = true;
        }

        /// <summary>
        /// Retrieve the topmost parent node recursivly from this node.
        /// </summary>
        public T Root {
            get {
                TreeNode<T> current = this;
                while (current.Parent != null) {
                    current = current.Parent;
                }
                return (T)current;
            }
        }

        /// <summary>
        /// Retrieves a non-reflective list of all TreeNode<T>s, starting with this TreeNode<T> including all 
        /// descendants recursivly.
        /// </summary>
        /// <remarks>
        /// This method performs a depth-first search to collect all TreeNode<T>s in the hierarchy, starting with 
        /// the current TreeNode<T> as the root. The result includes the current TreeNode<T> followed by its descendants,
        /// where each child's descendants are added before moving to the next sibling.
        /// </remarks>
        /// <returns>
        /// A <see cref="List{TreeNode<T>}"/> containing the current TreeNode<T> followed by all descendant 
        /// TreeNode<T>s in the order they were encountered.
        /// </returns>
        public List<T> AllDecendents() {
            List<T> all = [];
            all.Add((T)this);

            foreach (TreeNode<T> child in this.Children) {
                all.AddRange(child.AllDecendents());
            }

            return all;
        }

        private readonly List<T> _children = [];
        private TreeNode<T>? _parent;
        private InvalidType _invalid = new() { IsInvalid = true };
    }
}
