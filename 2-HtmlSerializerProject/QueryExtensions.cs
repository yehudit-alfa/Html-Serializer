using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2_HtmlSerializerProject
{
    static class QueryExtensions
    {
        public static bool IsMatch(this HtmlElement element, Selector selector)
        {
            if (selector.TagName != null && element.Name.ToLower() != selector.TagName)
                return false;
            if (selector.Id != null && element.Id != selector.Id)
                return false;
            foreach (var className in selector.Classes)
            {
                if (!element.Classes.Contains(className))
                    return false;
            }

            return true;
        }
        public static IEnumerable<HtmlElement> QuerySelector(this HtmlElement rootElement, Selector selector)
        {
            return QueryRecursive(rootElement, selector);
        }
        private static IEnumerable<HtmlElement> QueryRecursive(HtmlElement currentRoot, Selector currentSelector)
        {
            var descendants = currentRoot.Descendants().ToList();
            var matchingElements = descendants.Where(e => IsMatch(e, currentSelector)).ToList();
            if (currentSelector.Child == null)
            {
                return matchingElements;
            }

            var results = new HashSet<HtmlElement>();
            foreach (var match in matchingElements)
            {
                var childResults = QueryRecursive(match, currentSelector.Child);
                foreach (var el in childResults)
                    results.Add(el);
            }

            return results;
        }
    }
}
