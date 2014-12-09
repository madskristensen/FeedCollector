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
            SyndicationFeed feed = await DownloadFeed(config.AppSettings[key]);

            foreach (SyndicationItem item in feed.Items)
            {
                string content = item.Summary != null ? item.Summary.Text : ((TextSyndicationContent)item.Content).Text;
                content = _regex.Replace(content, string.Empty); // Strips out HTML

                item.Summary = new TextSyndicationContent(content.Substring(0, Math.Min(300, content.Length)) + "...");

                if (!list.Any(i => i.Title.Text.Equals(item.Title.Text, StringComparison.OrdinalIgnoreCase)))
                    list.Add(item);
            }
        }

        CreateFeed(list.OrderByDescending(i => i.PublishDate.Date));
    }

    private async Task<SyndicationFeed> DownloadFeed(string url)
    {
        using (WebClient client = new WebClient())
        {
            var stream = await client.OpenReadTaskAsync(url);
            return SyndicationFeed.Load(XmlReader.Create(stream));
        }
    }

    private void CreateFeed(IEnumerable<SyndicationItem> list)
    {
        SyndicationFeed rss = new SyndicationFeed(config.AppSettings["title"], config.AppSettings["description"], null, list);

        using (XmlWriter writer = XmlWriter.Create(_file, new XmlWriterSettings { Indent = true }))
        {
            rss.SaveAsRss20(writer);
        }
    }

    public IEnumerable<SyndicationItem> GetData()
    {
        using (XmlReader reader = XmlReader.Create(_file))
        {
            return SyndicationFeed.Load(reader).Items;
        }
    }
}