using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace _2_HtmlSerializerProject
{
    internal class HtmlParser
    {
        public HtmlElement BuildTree(IEnumerable<string> htmlLines)
        {
            HtmlElement root = new HtmlElement { Name = "html" };
            HtmlElement currentElement = root;
            foreach (string line in htmlLines)
            {
                if (line.StartsWith("<") && line.EndsWith(">"))
                {
                    string cleanTag = line.Substring(1, line.Length - 2);

                    string tagName = cleanTag.Split(' ', 2)[0].Replace("/", "");

                    // בדיקת תנאי עצירה (HTML/)
                    if (tagName == "html" && cleanTag.StartsWith("/"))
                    {
                        break;
                    }
                    if (cleanTag.StartsWith("/"))
                    {
                        if (currentElement.Parent != null)
                        {
                            currentElement = currentElement.Parent;
                        }
                        continue;
                    }

                    if (HtmlHelper.instance.AllTags.Contains(tagName))
                    {
                        HtmlElement newElement = new HtmlElement { Name = tagName };
                        newElement.Parent = currentElement;
                        currentElement.Children.Add(newElement);
                        string attributesString = cleanTag.Contains(" ") ? cleanTag.Split(' ', 2)[1] : string.Empty;
                        var attributesRegex = new Regex(@"([\s\S]+?)=(""[^""]*""|'[^']*'|\S+)");
                        var matches = attributesRegex.Matches(attributesString);
                        foreach (Match match in matches)
                        {
                            string name = match.Groups[1].Value.Trim();
                            string value = match.Groups[2].Value.Trim().Trim('"').Trim('\''); // מנקים גרשיים
                            newElement.Attributes.Add(new HtmlAttribute { Name = name, Value = value });
                            // עדכון ID:
                            if (name.ToLower() == "id")
                            {
                                newElement.Id = value;
                            }
                            // עדכון Classes: 
                            if (name.ToLower() == "class")
                            {
                                newElement.Classes.AddRange(value.Split(' ', StringSplitOptions.RemoveEmptyEntries));
                            }
                        }
                        bool isSelfClosing = line.EndsWith("/>");

                        bool isVoidTag = HtmlHelper.instance.SelfClosingTags.Contains(tagName);

                        if (!isSelfClosing && !isVoidTag)
                        {
                            currentElement = newElement;
                        }
                    }
                }
                else 
                {
                    string innerText = line.Trim();
                    if (!string.IsNullOrWhiteSpace(innerText))
                    {
                        currentElement.InnerHtml += innerText;
                    }
                }
            }
            return root;
        }
    }
}
