<%@ Page Language="C#" AutoEventWireup="true" Async="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>
<%@ OutputCache Duration="7200" VaryByParam="none" %>
<%@ Import Namespace="System.Configuration" %>

<!doctype html>
<html>
<head>
    <title><%:ConfigurationManager.AppSettings["title"]%></title>
    <link href="site.css" rel="stylesheet" />
    <link rel="alternate" type="application/rss+xml" href="/rss.xml" />
    <meta charset="utf-8" />
    <meta name="description" content="News articles and videos from the ASP.NET and Visual Studio Web Team" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
</head>
<body>

    <header role="banner">
        <h1><%:ConfigurationManager.AppSettings["title"]%></h1>
    </header>

    <div role="main">
        <a href="/feed.rss">
            <img src="rss.png" alt="Subscibe to the RSS feed" />
        </a>
        <asp:Repeater runat="server" ID="rep" ItemType="System.ServiceModel.Syndication.SyndicationItem" SelectMethod="rep_GetData">
            <ItemTemplate>
                <article>
                    <time datetime="<%# Item.PublishDate.ToString("yyyy-MM-dd HH:mm") %>">
                        <span class="month"><%# Item.PublishDate.ToString("MMM") %></span>
                        <span class="day"><%# Item.PublishDate.Day %></span>
                    </time>

                    <h2><%# Item.Title.Text %></h2>
                    <p><%# Item.Summary.Text %></p>

                    <a href="<%# Item.Links[0].Uri %>">Read the article</a>
                    <em style="background-image: url('<%#Item.Links[0].Uri.Scheme + "://" + Item.Links[0].Uri.Host + "/favicon.ico" %>')"></em>
                </article>
            </ItemTemplate>
        </asp:Repeater>
    </div>

    <footer>
        <p>
            <a href="http://github.com/madskristensen/webdevblogs">Contribute on GitHub</a><br />
            Copyright &copy; <%=DateTime.Now.Year %> <a href="http://madskristensen.net" rel="me">Mads Kristensen</a>
        </p>
    </footer>
</body>
</html>
