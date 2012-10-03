﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace xrc.Pages.Providers.FileSystem
{
	//TODO Rivedere il codice di XrcFolder e XrcFile, probabilmente spostare parte del codice di questa classe in PageLocatorService
	// dovrei probabilmente rimuovere la dipendenza al FileSystem

    public class XrcFile
    {
		readonly UriSegmentParameter _parameter;

		public XrcFile(XrcFolder parent, string fileName)
        {
			if (string.IsNullOrWhiteSpace(fileName))
				throw new ArgumentNullException("fileName");
			if (parent == null)
				throw new ArgumentNullException("parent");

			Parent = parent;
			fileName = fileName.ToLowerInvariant();
			FullPath = Path.Combine(parent.FullPath, fileName);
			Name = XrcFileSystemHelper.GetFileLogicalName(fileName);
			Extension = XrcFileSystemHelper.GetFileExtension(fileName);
			VirtualPath = UriExtensions.Combine(parent.VirtualPath, fileName);
			FullName = UriExtensions.Combine(parent.FullName, Name);
			_parameter = new UriSegmentParameter(Name);
		}

		public string Name
		{
			get;
			private set;
		}

		public UriSegmentParameter Parameter
		{
			get { return _parameter; }
		}

		public string Extension
		{
			get;
			private set;
		}

		public string VirtualPath
		{
			get;
			private set;
		}

		public string FullPath
		{
			get;
			private set;
		}

		public string FullName
		{
			get;
			private set;
		}

		public string FileName
		{
			get { return Path.GetFileName(FullPath); }
		}

		public XrcFolder Parent
		{
			get;
			private set;
		}

		public bool IsIndex
		{
			get { return string.Equals(Name, XrcFileSystemHelper.INDEX_FILE, StringComparison.InvariantCultureIgnoreCase); }
		}

		public bool IsSlot()
		{
			return Name.StartsWith("_");
		}
    }
}
