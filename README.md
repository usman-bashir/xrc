XRC
===

Version: 0.10 Alpha

Introduction
-----------------------

XRC is a rendering framework for ASP.NET that can be used in combination with ASP.NET MVC 
for rendering dynamic or static content inside an existing ASP.NET site.

Different view engines are supported (Razor, XSLT, ...) and any .NET class can be used as a data provider.

XRC is easy to use also for front-end developers who don't know .NET. Adding a page is as simple as writing 
an xml and the corresponding view page.

XRC offers a common set of features that can be used by all view engines. In this way each view engine is 
responsable of only the rendering of the actual content. All the infrastruture are handled by XRC.
Most important features are:

- Url routing (on top of ASP.NET routing)
- Layout
- Page composition
- Parameters

XRC also offers a clear separation between presentation and business layer. On the presentation layer 
you can concentrate on the view and simply invoke data retrivial methods. 
On the business layer on the other hand you must simply provide standard .NET classes so you can use 
your preferred framework or data access strategy.

Why XRC?
-----------------------


Architecture
-----------------------

The following figure shows the platform stack of xrc.

![xrc stack](https://raw.github.com/davideicardi/xrc/master/docs/xrc_stack.png "xrc stack")


How XRC works
-----------------------

On every web requests XRC checks if there is a corresponding .xrc page to handle the request.

- *http://yoursite/* maps to `~/xrc/index.xrc`
- *http://yoursite/about* maps to `~/xrc/about.xrc`

Here an example of a xrc file:

	<?xml version="1.0" encoding="utf-8" ?>
	<xrc:page xmlns:xrc="urn:xrc">
		<xrc:parameters>
			<xrc:add key="title" value="home" />
			<xrc:add key="activeMenu" value="home" />
		</xrc:parameters>
		<xrc:action layout="~/shared/_layout">
			<xrc:XHtmlView>
				<ContentFile>index.xhtml</ContentFile>
			</xrc:XHtmlView>
		</xrc:action>
	</xrc:page>

In the example above the .xrc file contains the layout to use, some custom parameters, the `XHtmlView` view engine (ie. Razor) with a cshtml view file.
Here another example:

	<?xml version="1.0" encoding="utf-8" ?>
	<xrc:page xmlns:xrc="urn:xrc" xmlns:Books="xrc:BooksModule">
		<xrc:parameters>
			<xrc:add key="genre" />
			<xrc:add key="title" value='@string.Format("books - {0}", genre)' />
		</xrc:parameters>
		<xrc:action method="GET">
			<xrc:XsltView>
				<XsltFile>../../books.xsl</XsltFile>
				<DataFile>@Books.GetByGenre(genre)</DataFile>
			</xrc:XsltView>
		</xrc:action>
	</xrc:page>

In this .xrc file I use `XsltView` view engine and I load the xml using a custom module (a .NET class that was previous registered). 
Note also that the `title` parameter is generated using a simple C# script.

Requirements
------------

XRC requires .NET 4.5 and ASP.NET MVC 4. The solution can be compiled using Visual Studio Express 2012 for Web or compatible environment.

Url and parameters
-----------------------


Layout and page composition
-----------------------


Features
-----------------------

- Integrated and compatible with any ASP.NET MVC web site. Usually you write your command operations with MVC controller and query operations with XRC.
- Multilevel layout pages can be written using Razor or Xslt.
- You can use your existing knowledge and all the views can be easily used for other framework because are based on well known technologies (razor, xslt, ...).
- Built-in view engines for:
	- Html
	- XHtml
	- Xstl
	- Razor
	- Markdown
	- Json
	- Xml
	- Raw (byte[])
- Mix content using different view engines or standard ASP.NET MVC.
- Partial page rendering.
- Friendly urls
- Url segment parameters, query parameters or static parameters.
- Any .NET class can be used as data provider.
- Extensible infrastructure.
- IoC friendly with native Windsor Castle support.
- Friendly error (with custom errors and http status codes)
- Easy setup and deploy.
- Azure ready.
- New pages can be added without recompilation
- Extensible architecture
- TODO Caching
- TODO Authentication


Getting started
-----------------------

XRC is available on [NuGet]. You can install the package using:

	PM> Install-Package xrc.Site

Source code and symbols (.pdb files) for debugging are available on [Symbol Source].


Extensions
-----------------------

Extensions can be used to add custom view engines, services, modules or parsers.
Currently these extensions are available:

- xrc.FileSystemPages (deployed as a dependency of xrc.Site)
- xrc.MVC4 (deployed as a dependency of xrc.Site)
- xrc.Markdown

You can install any extensions by installing the corresponding nuget package and then in the CustomXrcInstaller.cs file add a code like:

	xrc.XrcWindsor.InstallExtension(container, System.Reflection.Assembly.Load("xrc.Markdown"));

You can create your own extensions and installing it using this same code.

License
-----------------------

*[MIT License]* 

Copyright (c) 2012 Davide Icardi

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
- The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
- THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.



[CQRS]: http://martinfowler.com/bliki/CQRS.html
[MIT License]: http://opensource.org/licenses/mit-license.php
[NuGet]: https://nuget.org/packages/xrc.Site
[Symbol Source]: http://www.symbolsource.org/