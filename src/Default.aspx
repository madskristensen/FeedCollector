<%@ Page Language="C#" AutoEventWireup="true" Async="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>
<%@ OutputCache CacheProfile="default" %>

<!doctype html>
<html lang="en" itemscope itemtype="http://schema.org/Blog">
<head>
	<title><%:ConfigurationManager.AppSettings["title"]%></title>
	<meta charset="utf-8" />
	<meta name="description" content="<%:ConfigurationManager.AppSettings["description"]%>" />
	<meta name="viewport" content="width=device-width, initial-scale=1" />
	<link rel="stylesheet" href="themes/<%:ConfigurationManager.AppSettings["theme"]%>/site.css" />
	<link rel="alternate" type="application/rss+xml" href="feed.xml" />
</head>
<body>

	<header role="banner">
		<a href="~/" runat="server">
			<h1 itemprop="name"><%:ConfigurationManager.AppSettings["title"]%></h1>
		</a>
		<span itemprop="description"><%:ConfigurationManager.AppSettings["description"]%></span>
	</header>

	<div role="main">
		<a href="feed.xml" title="Subscribe to the RSS feed" class="feed">Subscribe</a>

		<asp:Repeater runat="server" ID="rep" ItemType="System.ServiceModel.Syndication.SyndicationItem" SelectMethod="GetData">
			<ItemTemplate>
				<article itemscope itemtype="http://schema.org/BlogPosting" itemprop="blogPost">
					<time itemprop="datePublished" datetime="<%# Item.PublishDate.ToString("yyyy-MM-dd HH:mm") %>">
						<span class="month"><%# Item.PublishDate.ToString("MMM") %></span>
						<span class="day"><%# Item.PublishDate.Day %></span>
					</time>

					<h2 itemprop="name"><%# Item.Title.Text %></h2>
					<p itemprop="articleBody"><%# Item.Summary.Text %></p>

					<a itemprop="url" href="<%#: Item.Links[0].Uri %>" title="<%#: Item.Title.Text %>">Read the article</a>
				</article>
			</ItemTemplate>
		</asp:Repeater>

		<div id="paging">
			<a href="<%=_page + 1 %>" rel="previous">&lt; Older</a>
			<%if (_page > 1){%>
			<a href="<%=_page == 2 ? "/" : _page - 1 + "" %>" rel="next">Newer &gt;</a>
			<%}%>
		</div>
	</div>

	<footer role="contentinfo">
		<p>Powered by <a href="https://github.com/madskristensen/FeedCollector">Feed Collector</a></p>
	</footer>
</body>
</html>