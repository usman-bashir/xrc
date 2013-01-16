
Introduction
------------

DynamicExpresso parse C# style expression and create an expression tree that can be executed with dynamic invoke. It doesn't generate assembly but it creates dynamic expression/delegate on the fly.

TODO Some examples of supported expressions:


Quick start
===========


Usages and examples
===================

TODO Counter Catch (filter counters and transform values)
TODO xrc (page declaration)
TODO ZenoSetup (for setup commands)
TODO Application console (for diagnostic or advanced features) (desktop/web)

Configurations
==============


Features
========

- Supported operators and constructors
- Unit tests
- Performance
- Small footprint
- Easy to use
- 100 % managed code



History
=======

This project is based on two past works:
- "Converting String expressions to Funcs with FunctionFactory by Matthew Abbott" (http://www.fidelitydesign.net/?p=333) 
- DynamicQuery - Dynamic LINQ - Visual Studio 2008 sample:
	- http://msdn.microsoft.com/en-us/vstudio/bb894665.aspx 
	- http://weblogs.asp.net/scottgu/archive/2008/01/07/dynamic-linq-part-1-using-the-linq-dynamic-query-library.aspx


Other resources or similar projects
===================================

- Roslyn Project - compilar as a services - http://msdn.microsoft.com/en-us/vstudio/roslyn.aspx
	- If Roslyn will be available in the future this project can directly use the Roslyn compiler/interpreter.
- Jint - Javascript interpreter for .NET - http://jint.codeplex.com/
- Jurassic - Javascript compiler for .NET - http://jurassic.codeplex.com/
- Javascrpt.net - javascript V8 engine - http://javascriptdotnet.codeplex.com/
- CS-Script - http://www.csscript.net/
- IronJS, IronRuby, IronPython


License
=======

MIT 
