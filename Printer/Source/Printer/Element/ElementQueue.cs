using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leagueinator.Printer.Elements {
    /// <summary>
    /// An element queue that will add all child elements to the queue whenever an element is dequeued.
    /// </summary>
    internal class ElementQueue : Queue<Element> {

        public ElementQueue(Element element) {
            this.Enqueue(element);
        }

        public new Element Dequeue() {
            if (this.Count == 0) throw new InvalidOperationException("Can not dequeue an empty queue");
            Element element = this.Dequeue();
            foreach (Element child in element.Children) this.Enqueue(child);
            return element;
        }

        /// <summary>
        /// Processes each element in the queue by applying a specified action. Elements are dequeued and the action is 
        /// applied in their order of appearance until the queue is empty.  As elements are dequeued the child elements
        /// are enqueued.
        /// </summary>
        /// <param name="action">An action to be applied to each dequeued element. This action takes a 
        /// single parameter of type <see cref="Element"/>, which represents the dequeued element.
        /// </param>
        public void Process(Action<Element> action) {
            while (this.Count > 0) {
                action(this.Dequeue());
            }
        }
    }
}
