<%@ Page Language="C#" AutoEventWireup="true" Async="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>
<%@ OutputCache Duration="86400" VaryByParam="none" %>

<!doctype html>
<html>
<head>
    <title>ASP.NET Blogs and Videos</title>
    <link href="site.css" rel="stylesheet" />
    <meta name="description" content="News articles and videos from the ASP.NET and Visual Studio Web Team" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
</head>
<body>

    <header role="banner">
        <h1>ASP.NET and Web Tools News</h1>
    </header>

    <div role="main">
        <asp:Repeater runat="server" ID="rep" ItemType="FeedItem">
            <ItemTemplate>
                <article>
                    <time datetime="<%# Item.Published.ToString("yyyy-MM-dd HH-mm") %>">
                        <span class="month"><%# Item.Published.ToString("MMM") %></span>
                        <span class="day"><%# Item.Published.Day %></span>
                    </time>

                    <h2><%# Item.Title %></h2>
                    <p><%# Item.Content %></p>

                    <a href="<%# Item.Url %>">Read the article</a>
                    <em style="background-image: url('<%#Item.Url.Scheme + "://" + Item.Url.Host + "/favicon.ico" %>')"></em>
                </article>
            </ItemTemplate>
        </asp:Repeater>
    </div>

    <footer>
        <p>
            <a href="http://github.com/madskristensen/webdevblogs">Contribute on GitHub</a><br />
            Copyright &copy; 2014 <a href="http://madskristensen.net" rel="me">Mads Kristensen</a>
        </p>
    </footer>
</body>
</html>
