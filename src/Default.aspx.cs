using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.UI;
using System.Xml;
using config = System.Configuration.ConfigurationManager;

public partial class _Default : Page
{
    private static readonly int _items = int.Parse(config.AppSettings["visibleitems"]);
    private string _file = HostingEnvironment.MapPath("~/feed.rss");

    protected void Page_Load(object sender, EventArgs e)
    {
        if (File.Exists(_file))
        {
            using (XmlReader reader = XmlReader.Create(_file))
            {
                SyndicationFeed feed = SyndicationFeed.Load(reader);
                Bind(feed);
            }

            // Download feeds in the background if "_file" is older than 1 hour
            if (File.GetLastWriteTimeUtc(_file) < DateTime.UtcNow.AddHours(-1))
                Task.Run(() => DownloadFeeds());
        }
        else
        {
            RegisterAsyncTask(new PageAsyncTask(async () => {
                var feed = await DownloadFeeds();
                Bind(feed);
            }));
        }
    }

    private async Task<SyndicationFeed> DownloadFeeds()
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

        return CreateFeed(list);
    }

    private SyndicationFeed CreateFeed(IEnumerable<SyndicationItem> list)
    {
        SyndicationFeed rss = new SyndicationFeed(config.AppSettings["title"], config.AppSettings["description"], null, list);

        using (XmlWriter writer = XmlWriter.Create(_file, new XmlWriterSettings { Indent = true }))
        {
            rss.SaveAsRss20(writer);
        }

        return rss;
    }

    private void Bind(SyndicationFeed feed)
    {
        rep.DataSource = feed.Items.Take(_items).Select(i => FeedItem.FromSyndicationItem(i));
        rep.DataBind();
    }
}