using _2_HtmlSerializerProject;
using System.Text.RegularExpressions;
var html = await Load("https://www.w3schools.com");
//var html = await Load("https://forum.netfree.link:20907/category/1/%D7%94%D7%9B%D7%A8%D7%96%D7%95%D7%AA");

var cleanHtml = new Regex(@"\s*").Replace(html, "");
var htmlLines = new Regex(@"(<.+?>)").Split(cleanHtml).Where(s => s.Length > 0);
var htmlElement = @"<div id=""my-id"" class=""my-class-1 my-class-2"" width=""100%"">text</div>";
var attributes = new Regex(@"([\s\S]+?)=(""[^""]*""|'[^']*'|\S+)").Matches(htmlElement);
HtmlParser parser = new HtmlParser();
HtmlElement root = parser.BuildTree(htmlLines);
//string query = "h1"; // לדוגמה
string query = "div"; // לדוגמה

Selector selector = Selector.Parse(query);


// 5. בצעי חיפוש
var results = root.QuerySelector(selector);

// 6. הדפיסי תוצאות
Console.WriteLine($"אלמנטים שנמצאו עבור הסלקטור '{query}':");
foreach (var el in results)
{
    Console.WriteLine($"Tag: {el.Name}, Id: {el.Id}, Classes: {string.Join(",", el.Classes)}");
}
static async Task<string> Load(string url)
{
    HttpClient client = new HttpClient();
    var response = await client.GetAsync(url);
    var html = await response.Content.ReadAsStringAsync();
    return html;
}

