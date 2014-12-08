using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.UI;
using System.Xml;
using config = System.Configuration.ConfigurationManager;

public partial class _Default : Page
{
    private static int _items = int.Parse(config.AppSettings["visibleitems"]);
    private static string _file = HostingEnvironment.MapPath("~/feed.rss");
    private static Regex _regex = new Regex("<[^>]*>", RegexOptions.Compiled);

    protected void Page_Load(object sender, EventArgs e)
    {
        Task task = Task.Run(() => DownloadFeeds());

        if (!File.Exists(_file))
            task.Wait();
    }

    private async Task DownloadFeeds()
    {
        var keys = config.AppSettings.AllKeys.Where(key => key.StartsWith("feed:"));
        var list = new List<SyndicationItem>();

        foreach (var key in keys)
        {
            using (WebClient client = new WebClient())
            {
                var stream = await client.OpenReadTaskAsync(config.AppSettings[key]);
                SyndicationFeed feed = SyndicationFeed.Load(XmlReader.Create(stream));
                list.AddRange(feed.Items);
            }
        }

        // Dedupe and order list of items
        list = list.GroupBy(i => i.Title.Text).Select(i => i.First()).OrderByDescending(i => i.PublishDate.Date).ToList();

        foreach (SyndicationItem item in list)
        {
            string author = item.Authors.Any() ? item.Authors[0].Name : string.Empty;
            string content = item.Summary != null ? item.Summary.Text : item.Content.ToString();
            content = _regex.Replace(content, string.Empty); // Strips out HTML

            item.Authors.Add(new SyndicationPerson(null, author, null));
            item.Summary = new TextSyndicationContent(content.Substring(0, Math.Min(300, content.Length)) + "...");
        }

        CreateFeed(list);
    }

    private void CreateFeed(IEnumerable<SyndicationItem> list)
    {
        SyndicationFeed rss = new SyndicationFeed(config.AppSettings["title"], config.AppSettings["description"], null, list);

        using (XmlWriter writer = XmlWriter.Create(_file, new XmlWriterSettings { Indent = true }))
        {
            rss.SaveAsRss20(writer);
        }
    }

    public IEnumerable<SyndicationItem> rep_GetData()
    {
        using (XmlReader reader = XmlReader.Create(_file))
        {
            return SyndicationFeed.Load(reader).Items.Take(_items);
        }
    }
}