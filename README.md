XRC
===

Version: 0.6 Alpha

Introduction
------------

XRC is a rendering framework for ASP.NET that can be used in combination with ASP.NET MVC. 
Can be used for rendering dynamic or static content inside an existing ASP.NET site.

Different view engines can be used (Razor, XSLT, ...) and can read data from different data sources (any .NET class can be used as a data provider).

Easy to use also for front-end developers who don't know .NET. Adding a page is simple as write a small xml and the corresponding view page using the language that you prefer (XSLT, Razor, ...).

XRC offers a set of features that can be used by all views. In this way each view engine is responsable of only the rendering of the actual content. All the infrastruture are handled in a common way by XRC.
Most important features are:

- Url bind
- Layout
- Composition
- Parameters

XRC also offers a clear separation between the presentation and the business layer. If you wrate the presentation layer you can concentrate on the view and simply invoke data retrivial methods of the business layer. On the business layer on the other hand you must simply provide standard .NET classes so you can use your preferred framework or data access strategy.

Why XRC?
--------



Architecture
------------

The following figure shows the platform stack of xrc.

![xrc stack](https://raw.github.com/davideicardi/xrc/master/docs/xrc_stack.png "xrc stack")


How XRC works
-------------





Features
-------------------------

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
- Mix content using different view engines.
- Partial page rendering.
- Friendly urls
- Url segment parameters, query parameters or static parameters.
- Any .NET class can be used as data provider.
- Extensible infrastructure.
- IoC friendly with native Windsor Castle support.
- Friendly error (with custom errors and http status codes)
- Easy setup and deploy.
- Azure ready.
- TODO Caching
- TODO Authentication

Installing and using XRC
------------------------

XRC is available on [NuGet]. You can install the package using:

	PM> Install-Package xrc.Site

Source code and symbols (.pdb files) for debugging are available on [Symbol Source].

License
-------

*[MIT License]* 

Copyright (c) 2012 Davide Icardi

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
- The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
- THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.



[CQRS]: http://martinfowler.com/bliki/CQRS.html
[MIT License]: http://opensource.org/licenses/mit-license.php
[NuGet]: https://nuget.org/packages/xrc.Site
[Symbol Source]: http://www.symbolsource.org/