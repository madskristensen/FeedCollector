# Feed Collector

[![Build status](https://ci.appveyor.com/api/projects/status/l9ylstqm84xkjl3f?svg=true)](https://ci.appveyor.com/project/madskristensen/webdevblogs)

Demo: [webdevblogs.azurewebsites.net](http://webdevblogs.azurewebsites.net/)

Feed Collector is a single-page feed aggregating website that allows you to combine
multiple RSS/Atom feeds into a single feed and display them nicely on a web page.

The features include:

* Add as many RSS/Atom feeds as you'd like
* A combined RSS feed is generated and exposed
* Paging support
* Themes lets you customize the web page
* Easy to deploy to any IIS server
* Best practices for performance and accessibility
* Optimized for both mobile and desktop

## Theming

You can customize the look and feel of the website using themes. The `default` theme
is selected by default, but you can add others by dropping in a CSS file to the `themes`
folder.

## Configurable

Every single setting is set through configuration in the web.config file, including:

* The RSS feeds the app consumes
* The name and description
* The output caching policy
* The number of posts to display
* Tracing for debugging purposes

## Deploy this app

You can easily fork the code on GitHub and deploy the app yourself using FTP, WebDeploy etc.

But instead, you can deploy straigt to Azure by clicking the button below and using the
Azure Portal's configuration UI to modify the config. It couldn't be easier to host your own
feed aggregation service.

[![Deploy to Azure](http://azuredeploy.net/deploybutton.png)](https://azuredeploy.net/)
