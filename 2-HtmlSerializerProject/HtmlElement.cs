using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace _2_HtmlSerializerProject
{
    internal class HtmlElement
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<HtmlAttribute> Attributes { get; set; } = new List<HtmlAttribute>();
        public List<string> Classes { get; set; } = new List<string>();
        public string InnerHtml { get; set; }
        public HtmlElement Parent { get; set; }
        public List<HtmlElement> Children { get; set; } = new List<HtmlElement>();
        public HtmlElement() { }
        public IEnumerable<HtmlElement> Descendants()
        {
            var queue = new Queue<HtmlElement>();

            foreach (var child in Children)
            {
                queue.Enqueue(child);
            }

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                yield return current;
                foreach (var child in current.Children)
                {
                    queue.Enqueue(child);
                }
            }
        }
        public IEnumerable<HtmlElement> Ancestors()
        {
            HtmlElement currentParent = Parent;

            while (currentParent != null)
            {
                yield return currentParent;

                currentParent = currentParent.Parent;
            }
        }

    }
}
