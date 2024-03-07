namespace Leagueinator.Printer {
    public class ElementList : List<Element> {

        public ElementList this[string id] => this.QueryAll(id);

        public ElementList() { }

        public ElementList(IEnumerable<Element> collection) : base(collection) { }

        public ElementList QueryAll(string query) {
            ElementList result = new();
            Queue<ElementList> queue = new();
            queue.Enqueue(this);

            while (queue.Count > 0) {
                ElementList current = queue.Dequeue();

                if (query == "*") {
                    result.AddRange(current);
                }
                else if (query.StartsWith(".")) {
                    foreach (Element element in current) {
                        if (element.ClassList.Contains(query[1..])) result.Add(element);
                    }
                }
                else if (query.StartsWith("#")) {
                    foreach (Element element in current) {
                        if (element.Attributes.ContainsKey("id")) {
                            if (element.Attributes["id"].Equals(query[1..])) result.Add(element);
                        }
                    }
                }
                else {
                    foreach (Element element in current) {
                        if (element.TagName == query) result.Add(element);
                    }
                }

                foreach (Element child in current) {
                    queue.Enqueue(child.Children);
                }
            }
            return result;
        }

        public Element? Query(string query) {
            Queue<ElementList> queue = new();
            queue.Enqueue(this);

            while (queue.Count > 0) {
                ElementList current = queue.Dequeue();

                if (query.StartsWith(".")) {
                    foreach (Element element in current) {
                        if (element.ClassList.Contains(query[1..])) return element;
                    }
                }
                if (query.StartsWith("#")) {
                    foreach (Element element in current) {
                        if (element.Attributes.ContainsKey("id")) {
                            if (element.Attributes["id"].Equals(query[1..])) return element;
                        }
                    }
                }
                else {
                    foreach (Element element in current) {
                        if (element.TagName == query) return element;
                    }
                }

                foreach (Element child in current) {
                    queue.Enqueue(child.Children);
                }
            }

            return null;
        }
    }
}
