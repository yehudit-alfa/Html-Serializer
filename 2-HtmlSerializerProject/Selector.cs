using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions; // חשוב לוודא שזה קיים!
using System.Threading.Tasks;

namespace _2_HtmlSerializerProject
{
    internal class Selector
    {
        // מאפייני החיפוש הבסיסיים
        public string TagName { get; set; }
        public string Id { get; set; }
        public List<string> Classes { get; set; } = new List<string>();

        // מאפייני מבנה השרשרת
        public Selector Parent { get; set; }
        public Selector Child { get; set; }

        public static Selector Parse(string query)
        {
            var parts = query.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            Selector current = null;
            Selector rootSelector = null;

            // Regex כדי לזהות את כל תבניות החיפוש (#id, .class)
            var regex = new Regex(@"(?:#(?<id>[^\. #]+))|(?:\.(?<class>[^\. #]+))");

            foreach (var part in parts)
            {
                var newSelector = new Selector();
                string remainingPart = part;

                // 1. חילוץ שם התגית (TagName)
                if (part.Length > 0 && char.IsLetter(part[0]))
                {
                    int firstSpecialCharIndex = part.IndexOfAny(new char[] { '#', '.' });

                    if (firstSpecialCharIndex != -1)
                    {
                        newSelector.TagName = part.Substring(0, firstSpecialCharIndex).ToLower();
                        remainingPart = part.Substring(firstSpecialCharIndex);
                    }
                    else
                    {
                        newSelector.TagName = part.ToLower();
                        remainingPart = string.Empty;
                    }
                    if (!string.IsNullOrEmpty(newSelector.TagName) &&
                        !HtmlHelper.instance.AllTags.Contains(newSelector.TagName))
                    {
                        newSelector.TagName = null; // מנקים אם זה לא תגית אמיתית
                    }
                }
                else
                {
                    // אם מתחיל ב-# או ., אין שם תגית
                    remainingPart = part;
                }

                // 2. חילוץ ID ו-Classes באמצעות Regex
                if (!string.IsNullOrEmpty(remainingPart))
                {
                    var matches = regex.Matches(remainingPart);

                    foreach (Match match in matches)
                    {
                        if (match.Groups["id"].Success)
                        {
                            newSelector.Id = match.Groups["id"].Value;
                        }
                        else if (match.Groups["class"].Success)
                        {
                            newSelector.Classes.Add(match.Groups["class"].Value);
                        }
                    }
                }

                // 3. בניית הקישור ל-Parent/Child
                if (rootSelector == null)
                {
                    rootSelector = newSelector;
                }
                if (current != null)
                {
                    current.Child = newSelector;
                    newSelector.Parent = current;
                }
                current = newSelector;
            }

            return rootSelector;
        }
    }
}