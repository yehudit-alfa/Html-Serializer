using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace _2_HtmlSerializerProject
{
    internal class HtmlHelper
    {

        private static readonly HtmlHelper _instance = new HtmlHelper();
        public static HtmlHelper instance => _instance;
        public string[] AllTags { get; }
        public string[] SelfClosingTags { get; }
        private HtmlHelper()
        {
            string allTagsJson = File.ReadAllText("HtmlTags.json");
            string selfClosingTagsJson = File.ReadAllText("HtmlVoidTags.json");

            AllTags = JsonSerializer.Deserialize<string[]>(allTagsJson);
            SelfClosingTags = JsonSerializer.Deserialize<string[]>(selfClosingTagsJson);
        }
    }
}
