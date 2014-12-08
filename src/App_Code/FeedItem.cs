using System;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;

public class FeedItem
{
    private static Regex _regex = new Regex("<[^>]*>", RegexOptions.Compiled);

    public FeedItem(string title, string description, string author, Uri url, DateTime published)
    {
        Title = title;
        Content = description;
        Author = author;
        Url = url;
        Published = published;
    }

    public string Title { get; set; }
    public string Author { get; set; }
    public Uri Url { get; set; }
    public string Content { get; set; }
    public DateTime Published { get; set; }

    public static FeedItem FromSyndicationItem(SyndicationItem item)
    {
        string author = item.Authors.Any() ? item.Authors[0].Name : string.Empty;
        string content = item.Summary != null ? item.Summary.Text : item.Content.ToString();

        content = _regex.Replace(content, string.Empty); // Strips out HTML
        content = content.Substring(0, Math.Min(300, content.Length)) + "...";

        return new FeedItem(item.Title.Text, content, author, item.Links[0].Uri, item.PublishDate.Date);
    }
}