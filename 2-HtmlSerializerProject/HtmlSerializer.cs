using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace _2_HtmlSerializerProject
{
    internal class HtmlSerializer
    {
        private readonly HtmlParser _parser;

        public HtmlSerializer()
        {
            _parser = new HtmlParser();
        }

        // פונקציה שמביאה HTML מכתובת אינטרנט
        public async Task<string> Load(string url)
        {
            using HttpClient client = new HttpClient();
            var response = await client.GetAsync(url);
            var html = await response.Content.ReadAsStringAsync();
            return html;
        }
        public HtmlElement Serialize(string html)
        {
            var cleanHtml = new Regex(@"\s*").Replace(html, "");
            var htmlLines = new Regex(@"(<.+?>)").Split(cleanHtml).Where(s => s.Length > 0);
            HtmlElement root = _parser.BuildTree(htmlLines);
            return root;
        }
    }

}
