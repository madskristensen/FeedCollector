using System;

public class FeedItem
{
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
}