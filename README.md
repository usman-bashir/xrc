XRC
===

Version: 0.6 Alpha

Introduction
------------

XRC is a rendering framework for ASP.NET that can be used in combination with ASP.NET MVC. 
Can be used for rendering (query) dynamic or static content inside an existing ASP.NET site.

Why XRC?
--------



Architecture
------------



Features
-------------------------

- Integrated and compatible with any ASP.NET MVC web site
- Multilevel layout pages can be written using Razor or Xslt
- Partial
- Multi site configuration (dev, test, staging, prod, ...)
- Built-in view engines for:
	- Html
	- XHtml
	- Xstl
	- Razor
	- Markdown
	- Json
	- Xml
- On the same page you can mix content using different languages
- Url segment parameters, query parameters or static parameters 
- Easy setup and deploy
- Azure ready
- Any .NET class can be used as data provider
- Extensible infrastructure
- IoC friendly with native Windsor Castle support
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