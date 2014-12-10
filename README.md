# Feed Collector website

[![Build status](https://ci.appveyor.com/api/projects/status/l9ylstqm84xkjl3f?svg=true)](https://ci.appveyor.com/project/madskristensen/webdevblogs)

Demo: [webdevblogs.azurewebsites.net](http://webdevblogs.azurewebsites.net/)

Keep yourself up to speed with everything ASP.NET and Visual Studio web tooling.

This website aggregates the activities such as blog posts, Youtube videos an more from the
ASP.NET and Web Tools team.

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