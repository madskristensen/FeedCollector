using System;
using System.Linq;
using System.Collections.Generic;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml.XPath;

public partial class _Default : System.Web.UI.Page
{
    private string _feed = ConfigurationManager.AppSettings.Get("feed");
    private static XDocument _cache;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (_cache == null)
            _cache = XDocument.Load(_feed);
    }

    public IEnumerable<FeedItem> rep_GetData()
    {
        var source = new List<FeedItem>();
        var items = _cache.XPathSelectElements("//item");

        foreach (XElement item in items.Take(20))
        {
            string title = GetString(item, "title");
            string content = GetString(item, "description");
            Uri url = new Uri(GetString(item, "link"));
            DateTime published = DateTime.Parse(GetString(item, "pubDate"));
            string author = GetString(item, "author", "a community member");

            Regex regex = new Regex("<[^>]*>", RegexOptions.Compiled);
            content = regex.Replace(content, string.Empty);
            content = content.Substring(0, Math.Min(300, content.Length)) + "...";

            source.Add(new FeedItem(title, content, author, url, published));
        }

        return source;
    }

    private static string GetString(XElement element, string name, string fallback = "")
    {
        var item = element.XPathSelectElement(name);

        if (item != null && !string.IsNullOrEmpty(item.Value))
            return item.Value;

        return fallback;
    }
}