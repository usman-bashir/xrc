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

Xrc files define what the page must do. A page is composed by:

- parameters
- actions
	- method: GET, PUT, POST, ...
	- layout
	- views

The view define how to render the page. You can select between many different view engines, xslt, razor, xml, html, ... 
Each view accepts a custom set of parameters that can be customized within the page.

Here an example of a xrc file using a static file content:

	<?xml version="1.0" encoding="utf-8" ?>
	<xrc:page xmlns:xrc="urn:xrc">
		<xrc:parameters>
			<xrc:add key="title" value="home" />
			<xrc:add key="activeMenu" value="home" />
		</xrc:parameters>
		<xrc:action layout="~/shared/_layout">
			<xrc:HtmlView>
				<ContentFile>index.html</ContentFile>
			</xrc:HtmlView>
		</xrc:action>
	</xrc:page>

In the example above the .xrc file contains the layout to use, some custom parameters, the `HtmlView` view engine that reads the content from a file.
Here another example using xslt:

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
- Extensible architecture (page providers, modules, views, ...)
- TODO Caching
- TODO Authentication


Getting started
-----------------------

XRC is available on [NuGet]. You can install the package using:

	PM> Install-Package xrc.Site

Source code and symbols (.pdb files) for debugging are available on [Symbol Source].

Views
-----

TODO general description

### HtmlView

	<?xml version="1.0" encoding="utf-8" ?>
	<xrc:page xmlns:xrc="urn:xrc">
		<xrc:action>
			<xrc:HtmlView>
				<Content>
					<![CDATA[
					<h3>Html</h3>
					<div>
						This is the an html content
					</div>
					]]>
				</Content>
			</xrc:HtmlView>
		</xrc:action>
	</xrc:page>

Html content can be also specified using the `ContentFile` property by passing a static html file to load. The file can point to a relative file from the xrc site root (~/assets/myfile.html), absolute (c:\myfile.html) or relative from the current page (myfile.html).

### XsltView

	<?xml version="1.0" encoding="utf-8" ?>
	<xrc:page xmlns:xrc="urn:xrc">
		<xrc:action>
			<xrc:XsltView>
				<XsltFile>xslt_sample.xslt</XsltFile>
				<DataFile>xslt_sample.xml</DataFile>
			</xrc:XsltView>
		</xrc:action>
	</xrc:page>

Xslt and data xml can be also specified directly using the `Xslt` and `Data` properties that accept an `XDocument` class.

Below an example of an xslt and xml files. Xslt:

	<?xml version="1.0" encoding="utf-8" ?>
	<xsl:stylesheet version="1.0"
					xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
		<xsl:output omit-xml-declaration="yes"/>
		<xsl:template match="bookstore">
		<h3>Xslt</h3>
		<p>This is a xslt view</p>    
			<table>
				<thead>
					<TR>
						<th>ISBN</th>
						<th>Title</th>
						<th>Price</th>
						<th>Genre</th>
					</TR>
				</thead>
				<tbody>
					<xsl:apply-templates select="book"/>
				</tbody>
			</table>
		</xsl:template>

		<xsl:template match="book">
			<TR>
				<TD>
					<xsl:value-of select="@ISBN"/>
				</TD>
				<TD>
					<xsl:value-of select="title"/>
				</TD>
				<TD>
					<xsl:value-of select="price"/>
				</TD>
				<TD>
					<xsl:value-of select="@genre"/>
				</TD>
			</TR>
		</xsl:template>
	</xsl:stylesheet>

Xml:

	<?xml version="1.0" encoding="utf-8" ?>
	<!-- This file represents a fragment of a book store inventory database -->
	<bookstore>
	  <book genre="autobiography" publicationdate="1981" ISBN="1-861003-11-0">
		<title>The Autobiography of Benjamin Franklin</title>
		<author>
		  <first-name>Benjamin</first-name>
		  <last-name>Franklin</last-name>
		</author>
		<price>8.99</price>
	  </book>
	  <book genre="novel" publicationdate="1967" ISBN="0-201-63361-2">
		<title>The Confidence Man</title>
		<author>
		  <first-name>Herman</first-name>
		  <last-name>Melville</last-name>
		</author>
		<price>11.99</price>
	  </book>
	  <book genre="philosophy" publicationdate="1991" ISBN="1-861001-57-6">
		<title>The Gorgias</title>
		<author>
		  <name>Plato</name>
		</author>
		<price>9.99</price>
	  </book>
	  <book genre="novel" publicationdate="1924" ISBN="2-551001-57-6">
		<title>La coscienza di Zeno</title>
		<author>
		  <first-name>Italo</first-name>
		  <last-name>Svevo</last-name>
		</author>
		<price>15</price>
	  </book>
	</bookstore>

Parameters and extensions can be passed at the xslt engine. Parameters are defined as xslt parameters:

	<xsl:param name="title" />
	<xsl:param name="activeMenu" />

If defined these parameters will be automatically populated when the transformation is executed.
Extensions can be defined as xml namespace:

	<xsl:stylesheet version="1.0"
				xmlns="http://www.w3.org/1999/xhtml"
				xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				xmlns:Url="xrc:UrlModule"
				xmlns:Slot="xrc:SlotModule"
				xmlns:SourceCode="xrc:SourceCodeModule"
				exclude-result-prefixes="Url Slot">
	...

In the example above I have imported 3 modules: SlotModule, UrlModule and SourceCodeModule. These modules can be called as xslt extensions:

	<script type="text/javascript" src="{Url:Content('~/scripts/jquery-1.7.2.min.js')}">//</script>

or

	<xsl:value-of disable-output-escaping="yes" select="Slot:IncludeChild()" />

Any .NET class can be registered and used as a xslt extension (if compatible with the xslt extension specification).

### RazorView

	<?xml version="1.0" encoding="utf-8" ?>
	<xrc:page xmlns:xrc="urn:xrc">
		<xrc:action>
			<xrc:RazorView>
				<ViewUrl>razor_sample.cshtml</ViewUrl>
				<Model>@DateTime.Now</Model>
			</xrc:RazorView>
		</xrc:action>
	</xrc:page>

An example of a razor file:

	@model DateTime
	<h3>Razor</h3>
	<p>Hello from Razor, the current time is: @Model</p>

Inside a razor file (cshtml file) you can specify parameter or other service dependencies by using the `functions` keyword and by deriving from `xrc.Razor.XrcWebViewPage`. An example:

	@inherits xrc.Razor.XrcWebViewPage
	@functions
	{
		public string Country { get; set; }
		public IOlympicsModule OlympicsModule {get; set;}
	}

In the example above I expect that there is a `Country` parameter defined on the page and that a `IOlympicsModule` is registered as an extension. The properties will be loaded when the view is executed.

### MarkdownView

	<?xml version="1.0" encoding="utf-8" ?>
	<xrc:page xmlns:xrc="urn:xrc">
		<xrc:action>
			<xrc:MarkdownView>
				<ContentFile>_markdownFile.md</ContentFile>
			</xrc:MarkdownView>
		</xrc:action>
	</xrc:page>

You can set the markdown content by using the `Content` property or by specify a file to load, `ContentFile`, that can be relative of absolute.
An example of a markdown file:

<pre>
### Markdown file

Paragraphs are separated by a blank line.

2nd paragraph. *Italic*, **bold**, `monospace`. Itemized lists
look like:

  * this one
  * that one
  * the other one

> Block quotes are
> written like so.
>
> They can span multiple paragraphs,
> if you like.
</pre>

### RawView

RawView is used to insert in the response a series of bytes that can be passed reading a file (`ContentFile` property) or by directly specify the byte array (`Content` property).

	<?xml version="1.0" encoding="utf-8" ?>
	<xrc:page xmlns:xrc="urn:xrc">
		<xrc:action>
			<xrc:RawView>
				<ContentFile>~/bin/Castle.Core.dll</ContentFile>
				<FileDownloadName>Test.dll</FileDownloadName>
			</xrc:RawView>
		</xrc:action>
	</xrc:page>

For the `ContentFile` you can also use absolute path like C:\Users\davide\My Projects\github\xrc\src\xrc\bin\Release\Castle.Core.dll .
If `FileDownloadName` is specified the browser try to download the file (the `Content-Disposition` header is added).

### SlotView

SlotView can be used to include a partial page from another page.

	<?xml version="1.0" encoding="utf-8" ?>
	<xrc:page xmlns:xrc="urn:xrc">
		<xrc:action>
			<xrc:SlotView>
				<SlotUrl>_anotherPageUrl</SlotUrl>
			</xrc:SlotView>
		</xrc:action>
	</xrc:page>

The `SlotUrl` parameter can be absolute from the xrc site root (~/partials/_mysection) or can be relative from the current page (_mysection). By convention partial page should starts with an underscore (`_`).

Url and parameters
-----------------------

Parameters are usually passed using a segment of the url (REST like urls). For example a url `http://yoursite/players/maradona/photos` can point to a page that show all the photos of the player Maradona.
In this case the page using the file system page provider can be defined as follow:

	/players/{name}/photos

The `{name}` segment identify a parameter that will be available during the page processing. If you want to use this parameter inside an xrc file you can just define it as follow:

	<xrc:page xmlns:xrc="urn:xrc">

		<xrc:parameters>
			<!-- this parameter is readed using the url segment value {name} -->
			<xrc:add key="name" />
		</xrc:parameters>
	</xrc:page>

Using this code the `name` parameter is automatically populated from the segment and can be used inside xrc script block (see *Xrc script blocks* section) or inside actual views. For example all parameters are automatically injected inside razor or xslt pages.

Layout and page composition
-----------------------

Layout pages can be defined as xslt or razor pages. Layout pages are defined like any other pages but can embed the execution of child pages. Here an example.
A page that reference a specific layout pages called _mylayout:

	<?xml version="1.0" encoding="utf-8" ?>
	<xrc:page xmlns:xrc="urn:xrc">
		<xrc:parameters>
			<xrc:add key="title" value="my Page" />
		</xrc:parameters>
		<xrc:action layout="_mylayout">
			<xrc:XHtmlView>
				<Content>
					<div>
						This is the child content.
					</div>
				</Content>
			</xrc:XHtmlView>
		</xrc:action>
	</xrc:page>

The layout page `_mylayout.xrc` is defined as:

	<?xml version="1.0" encoding="utf-8" ?>
	<xrc:page xmlns:xrc="urn:xrc">
		<xrc:action method="GET">
			<xrc:RazorView>
				<ViewUrl>_layoutRazor.cshtml</ViewUrl>
			</xrc:RazorView>
		</xrc:action>
	</xrc:page>

_layourRazor.cshtml 

	@inherits xrc.Razor.XrcWebViewPage
	@functions
	{
		public string title { get; set; }
		public xrc.Modules.ISlotModule SlotModule { get; set; }
	}
	<!DOCTYPE html>
	<html>
	<head>
		<title>
			@title
		</title>
	</head>
	<body>
		@Html.Raw(SlotModule.IncludeChild())
	</body>
	</html>

Plese note the call to `SlotModule.IncludeChild()` that it's used to include the child content and the use of the `title` parameter defined in the page content.
The same layout can be defined also using an xslt page:

	<?xml version="1.0" encoding="utf-8" ?>
	<xrc:page xmlns:xrc="urn:xrc">
		<xrc:action method="GET">
			<xrc:XsltView>
				<XsltFile>_layout.xslt</XsltFile>
			</xrc:XsltView>
		</xrc:action>
	</xrc:page>

_layout.xslt file:

	<xsl:stylesheet version="1.0"
					xmlns="http://www.w3.org/1999/xhtml"
					xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
					xmlns:Slot="xrc:SlotModule"
					exclude-result-prefixes="Slot">
		<xsl:output method="html" encoding="utf-8" />
		<xsl:param name="title" />
		<xsl:template match="/">
			<xsl:text disable-output-escaping='yes'>&lt;!DOCTYPE html></xsl:text>
			<html>
				<head>
					<title>
						<xsl:value-of select="$title" />
					</title>
				</head>
				<body>
					<xsl:value-of disable-output-escaping="yes" select="Slot:IncludeChild()" />
				</body>
			</html>
		</xsl:template>
	</xsl:stylesheet>

The `IncludeChild` method can also accept a string parameter that specify the slot to include. In this case the name must match the one defined in the page. 
For example the following page has 3 views (`left`, `center`, `right`):

	<?xml version="1.0" encoding="utf-8" ?>
	<xrc:page xmlns:xrc="urn:xrc">
		<xrc:action layout="_layout3cols">
			<xrc:SlotView slot="left">
				<SlotUrl>~/widgets/_ranking</SlotUrl>
			</xrc:SlotView>
			<xrc:XHtmlView slot="center">
				<Content>
					<div>
						Lorem ipsum dolor sit amet, consectetur adipisicing elit.
					</div>
				</Content>
			</xrc:XHtmlView>
			<xrc:SlotView slot="right">
				<SlotUrl>_latestPhotos</SlotUrl>
			</xrc:SlotView>
		</xrc:action>
	</xrc:page>

Here the corresponding layout (`_layout3cols.xrc`):

	<?xml version="1.0" encoding="utf-8" ?>
	<xrc:page xmlns:xrc="urn:xrc">
		<xrc:action layout="_baseLayout">
			<xrc:XsltView>
				<XsltFile>_layout3cols.xslt</XsltFile>
			</xrc:XsltView>
		</xrc:action>
	</xrc:page>

_layout3cols.xslt:

	<xsl:stylesheet version="1.0"
					xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
					xmlns:Slot="xrc:SlotModule" 
					exclude-result-prefixes="Slot">
		<xsl:output method="html" encoding="utf-8" />
		<xsl:template match="/">
		<div class="row-fluid">
		  <div class="span4">
			<xsl:value-of disable-output-escaping="yes" select="Slot:IncludeChild('left')" />
		  </div>
		  <div class="span4">
			<xsl:value-of disable-output-escaping="yes" select="Slot:IncludeChild('center')" />
		  </div>
		  <div class="span4">
			<xsl:value-of disable-output-escaping="yes" select="Slot:IncludeChild('right')" />
		  </div>
		</div>
	  </xsl:template>
	</xsl:stylesheet>

Layout can be nested, for example in this case `_layout3cols.xrc` it is a child of the `_baseLayout`.

Another method that can be used to compose pages is to include other pages. In this case you can use the `SlotModule.Include(string url, [object parameters])` method that accepts another page to include.

Xrc script blocks
------------------

Some elements inside xrc files can be scriptable using a C# style syntax. Here an example:

	<xrc:page xmlns:xrc="urn:xrc"
			 xmlns:File="xrc:FileModule">
		<xrc:parameters>
			<xrc:add key="genre" value="adventure" />
			<xrc:add key="title" value='@string.Format("books - {0}", genre)' />
		</xrc:parameters>

		<xrc:action method="GET">
			<xrc:XsltView>
				<Xslt>@File.Xml("books_genre.xslt")</Xslt>
				<DataFile>../../books.xml</DataFile>
			</xrc:XsltView>
		</xrc:action>
	</xrc:page>

The elements or attributes that start with a `@` are script blocks. In this text you can write a subset of C# code that will be evaluated during execution.
The library used for parsing the code is [Dynamic Expresso](https://github.com/davideicardi/DynamicExpresso).

In the above example the `file` variable is an instance of the `FileModule` class, a standard .NET class registered as an extension.

ASP.NET MVC integration
----------------------

There are several points where xrc can be integrated with ASP.NET MVC:

- part of the site can be created using standard MVC controllers/views
- a controller can output an xrc page:

<pre>
public class ContactController : Controller
{
	readonly xrc.IXrcService _xrc;
	public ContactController(xrc.IXrcService xrc)
	{
		_xrc = xrc;
	}
	public ActionResult Login(string user, string pwd)
	{
		var xrcResult = _xrc.Page(new xrc.XrcUrl("~/loginSuccess"), new { LoggedUser = user });
		return xrcResult.AsActionResult();
	}
}
</pre>

- any MVC action can be called by just pointing to the right url. You can use the `UrlModule.Content` method to construct a valid url.


Custom errors and exceptions
---------------------------

TODO

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