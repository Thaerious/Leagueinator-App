using Leagueinator.Utility;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Leagueinator.Printer.Query {
    public class QueryEngine {
        private readonly HashSet<Element> elements = [];
        private readonly HashTable<Element> tagName = new();
        private readonly HashTable<Element> className = new();
        private readonly HashTable<Element> id = new();

        public int Count => this.elements.Count;

        public readonly static int SPECIFICITY_SIZE = 5;

        public List<Element> this[string query] {
            get {
                List<Element> result = new();

                foreach (string subq in query.Split(",")) {
                    string q = subq.Trim();
                    Queue<string> split = new(q.Split(' ').Reverse());
                    List<Element> elements = this.Lookup(split.Dequeue());
                    var part = elements.Where(e => this.Query(new(split), e)).ToList();
                    result.AddRange(part);
                }

                return result;
            }
        }

        /// <summary>
        /// Return true is the queue would return the element.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        private bool Query(Queue<string> queue, Element element) {
            Element? current = element.Parent;
            if (queue.Count == 0) return true;

            while (current is not null && queue.Count > 0) {
                switch (queue.Peek()) {
                    case ">":
                        // continue if immediate parent matches, otherwise terminate
                        queue.Dequeue();
                        if (!Match(current, queue.Dequeue())) return false;
                        if (queue.Count == 0) return true;
                        current = current.Parent;
                        break;
                    default:
                        // continue until any parent matches
                        if (Match(current, queue.Peek())) {
                            queue.Dequeue();
                            if (queue.Count == 0) return true;
                        }
                        current = current.Parent;
                        break;
                }
            }

            return false;
        }

        public void Add(Element element) {
            if (element.TagName.StartsWith("@")) return;
            this.tagName.Add(element.TagName, element);

            if (element.Attributes.TryGetValue("id", out string? value)) {
                if (!this.id.Has(value)) this.id.Add(value, element);
            }

            foreach (string className in element.ClassList) {
                this.className.Add(className, element);
            }

            this.elements.Add(element);
        }

        public void AddAll(Element root) {
            Queue<Element> queue = [];
            queue.Enqueue(root);

            while (queue.Count > 0) {
                Element next = queue.Dequeue();
                this.Add(next);
                foreach (Element element in next.Children) {
                    queue.Enqueue(element);
                }
            }
        }

        public bool Contains(Element element) {
            return this.elements.Contains(element);
        }

        /// <summary>
        /// Return all elements that match the simple query.
        /// A simple query consists of only a single term.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<Element> Lookup(string query) {
            switch (query.ToCharArray()[0]) {
                case '*':
                    return new(elements);
                case '#':
                    return this.id.Get(query[1..]);
                case '.':
                    return this.className.Get(query[1..]);
                default:
                    if (query.Contains('.')) return LookupTagAndClass(query);
                    return this.tagName.Get(query);
            }
        }

        private List<Element> LookupTagAndClass(string query) {
            string[] split = query.Split(".");
            return this.tagName.Get(split[0])
                       .Where(e => e.ClassList.Contains(split[1]))
                       .ToList();
        }

        private static bool Match(Element? element, string query) {
            if (element is null) return false;

            switch (query.ToCharArray()[0]) {
                case '*':
                    return true;
                case '#':
                    return element.Identifier.Equals(query[1..]);
                case '.':
                    return element.ClassList.Contains(query[1..]);
                default:
                    if (query.Contains('.')) return MatchTagAndClass(element, query);
                    return element.TagName.Equals(query);
            }
        }

        private static bool MatchTagAndClass(Element element, string query) {
            string[] split = query.Split(".");
            if (!split[0].Equals(element.TagName)) return false;
            return element.ClassList.Contains(split[1]);
        }

        public static int[] Specificity(string query, int rank = 0) {
            int[] specificity = new int[SPECIFICITY_SIZE];
            specificity[4] = rank;

            string pattern = @"[ >]";
            string[] split = Regex.Split(query, pattern);

            foreach (string s in split) {
                if (s.IsEmpty()) continue;
                switch (s.ToCharArray()[0]) {
                    case '*':
                        break;
                    case '#':
                        specificity[1]++;
                        break;
                    case '.':
                        specificity[2]++;
                        break;
                    default:
                        if (query.Contains('.')) {
                            specificity[2]++;
                            specificity[3]++;
                        }
                        else specificity[3]++;
                        break;
                }
            }

            return specificity;
        }
    }
}
