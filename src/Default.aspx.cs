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
    private static string _file = HostingEnvironment.MapPath(config.AppSettings["file"]);

    protected void Page_Load(object sender, EventArgs e)
    {
        Task task = Task.Run(() => DownloadFeeds());

        if (!File.Exists(_file))
            task.Wait();
    }

    private async Task DownloadFeeds()
    {
        var list = new List<SyndicationItem>();

        foreach (var key in config.AppSettings.AllKeys.Where(key => key.StartsWith("feed:")))
        {
            SyndicationFeed feed = await DownloadFeed(config.AppSettings[key]);
            list.AddRange(feed.Items);
        }

        // Dedupe and order list of items
        var ordered = list.GroupBy(i => i.Title.Text).Select(i => i.First()).OrderByDescending(i => i.PublishDate.Date);

        SyndicationFeed rss = new SyndicationFeed(config.AppSettings["title"], config.AppSettings["description"], null, ordered);

        using (XmlWriter writer = XmlWriter.Create(_file))
            rss.SaveAsRss20(writer);
    }

    private async Task<SyndicationFeed> DownloadFeed(string url)
    {
        try
        {
            using (WebClient client = new WebClient())
            {
                var stream = await client.OpenReadTaskAsync(url);
                return SyndicationFeed.Load(XmlReader.Create(stream));
            }
        }
        catch (Exception ex)
        {
            Trace.Warn("Feed", "Couldn't download: " + url, ex);
            return new SyndicationFeed();
        }
    }

    public IEnumerable<SyndicationItem> GetData()
    {
        using (XmlReader reader = XmlReader.Create(_file))
        {
            var items = SyndicationFeed.Load(reader).Items.Take(_items);
            return items.Select(item =>
            {
                string content = item.Summary != null ? item.Summary.Text : ((TextSyndicationContent)item.Content).Text;
                content = Regex.Replace(content, "<[^>]*>", string.Empty); // Strips out HTML
                item.Summary = new TextSyndicationContent(content.Substring(0, Math.Min(300, content.Length)) + "...");
                return item;
            });
        }
    }
}