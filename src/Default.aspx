<%@ Page Language="C#" AutoEventWireup="true" Async="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>
<%@ OutputCache CacheProfile="default" %>

<!doctype html>
<html itemscope itemtype="http://schema.org/Blog">
<head>
    <title><%:ConfigurationManager.AppSettings["title"]%></title>
    <link rel="stylesheet" href="site.css" />
    <link rel="alternate" type="application/rss+xml" href="<%:ConfigurationManager.AppSettings["file"]%>" />
    <meta charset="utf-8" />
    <meta name="description" content="<%:ConfigurationManager.AppSettings["description"]%>" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
</head>
<body>

    <header role="banner">
        <h1 itemprop="name"><%:ConfigurationManager.AppSettings["title"]%></h1>
        <span itemprop="description"><%:ConfigurationManager.AppSettings["description"]%></span>
    </header>

    <div role="main">
        <a href="<%:ConfigurationManager.AppSettings["file"]%>" title="Subscribe" class="feed">Subscribe to the RSS feed</a>

        <asp:Repeater runat="server" ID="rep" ItemType="System.ServiceModel.Syndication.SyndicationItem" SelectMethod="GetData">
            <ItemTemplate>
                <article itemscope itemtype="http://schema.org/BlogPosting" itemprop="blogPost">
                    <time itemprop="datePublished" datetime="<%# Item.PublishDate.ToString("yyyy-MM-dd HH:mm") %>">
                        <span class="month"><%# Item.PublishDate.ToString("MMM") %></span>
                        <span class="day"><%# Item.PublishDate.Day %></span>
                    </time>

                    <h2 itemprop="name"><%# Item.Title.Text %></h2>
                    <p itemprop="articleBody"><%# Item.Summary.Text %></p>

                    <a itemprop="url" href="<%# Item.Links[0].Uri %>" title="<%#: Item.Title.Text %>">Read the article</a>
                </article>
            </ItemTemplate>
        </asp:Repeater>
    </div>

    <footer>
        <p>
            <a href="http://github.com/madskristensen/webdevblogs">Contribute on GitHub</a><br />
            Copyright &copy; <%=DateTime.Now.Year %> <a href="http://madskristensen.net" itemprop="accountablePerson" rel="me">Mads Kristensen</a>
        </p>
    </footer>
</body>
</html>
