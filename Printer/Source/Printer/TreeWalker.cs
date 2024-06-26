﻿namespace Leagueinator.Printer.Elements {
    /// <summary>
    /// An node queue that will add all child elements to the queue whenever an node is dequeued.
    /// </summary>
    public class TreeWalker<T> : Queue<T> where T : TreeNode<T> {

        public static void Walk<U>(U root, Action<U> action) where U : TreeNode<U> {
            new TreeWalker<U>(root).Walk(action);
        }

        public TreeWalker(T node) {
            this.Enqueue(node);
        }

        /// <summary>
        /// Remove the first element of the queue and push all of it's child nodes onto the queue.
        /// </summary>
        /// <returns>The element removed</returns>
        /// <exception cref="InvalidOperationException">If the queue is empty</exception>
        public new T Dequeue() {
            if (this.Count == 0) throw new InvalidOperationException("Can not dequeue an empty queue");
            T node = base.Dequeue();
            foreach (T child in node.Children) this.Enqueue(child);
            return node;
        }

        /// <summary>
        /// Remove the first element of the queue, does not push child nodes onto the queue.
        /// </summary>
        /// <returns>The element removed</returns>
        /// <exception cref="InvalidOperationException">If the queue is empty</exception>
        public T Unqueue() {
            if (this.Count == 0) throw new InvalidOperationException("Can not dequeue an empty queue");
            T node = base.Dequeue();
            return node;
        }

        /// <summary>
        /// Processes each node in the queue by applying a specified action. Elements are dequeued and the action is 
        /// applied in their order of appearance until the queue is empty.  As elements are dequeued the child elements
        /// are enqueued.
        /// </summary>
        /// <param name="action">An action to be applied to each dequeued node. This action takes a 
        /// single parameter of type <see cref="Element"/>, which represents the dequeued node.
        /// </param>
        public void Walk(Action<T> action) {
            while (this.Count > 0) {
                action(this.Dequeue());
            }
        }

        /// <summary>
        /// Processes each node in the queue by applying a specified action. Elements are dequeued and the action is 
        /// applied in their order of appearance until the queue is empty.  As elements are dequeued the child elements
        /// are enqueued.
        /// </summary>
        /// <param name="action">An action to be applied to each dequeued node. This action takes a 
        /// single parameter of type <see cref="Element"/>, which represents the dequeued node.
        /// </param>
        public void Walk(Func<T, bool> func) {
            while (this.Count > 0) {
                if (func(this.Peek())) this.Dequeue();
                else this.Unqueue();
            }
        }
    }
}
