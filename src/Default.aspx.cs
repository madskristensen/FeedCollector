using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.UI;
using System.Xml;

public partial class _Default : Page
{
    private static readonly int _items = int.Parse(ConfigurationManager.AppSettings.Get("visibleitems"));
    private static Regex _regex = new Regex("<[^>]*>", RegexOptions.Compiled);

    protected void Page_Load(object sender, EventArgs e)
    {
        RegisterAsyncTask(new PageAsyncTask(GetFeedItems));
    }

    private async Task GetFeedItems()
    {
        var keys = ConfigurationManager.AppSettings.AllKeys.Where(key => key.StartsWith("feed:"));
        var list = new List<SyndicationItem>();

        foreach (var key in keys)
        {
            using (WebClient client = new WebClient())
            {
                var stream = await client.OpenReadTaskAsync(ConfigurationManager.AppSettings.Get(key));
                SyndicationFeed feed = SyndicationFeed.Load(XmlReader.Create(stream));
                list.AddRange(feed.Items);
            }
        }

        var sorted = list.OrderByDescending(i => i.PublishDate.Date);

        rep.DataSource = ConvertToFeedItem(sorted).Take(_items);
        rep.DataBind();
    }

    public static IEnumerable<FeedItem> ConvertToFeedItem(IEnumerable<SyndicationItem> items)
    {
        items = items.GroupBy(i => i.Title.Text).Select(i => i.First()); // Dedupes based on Title

        foreach (var item in items)
        {
            string author = item.Authors.Any() ? item.Authors[0].Name : string.Empty;
            string content = item.Summary != null ? item.Summary.Text : item.Content.ToString();

            content = _regex.Replace(content, string.Empty);
            content = content.Substring(0, Math.Min(300, content.Length)) + "...";

            yield return new FeedItem(item.Title.Text, content, author, item.Links[0].Uri, item.PublishDate.Date);
        }
    }
}