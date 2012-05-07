﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoWebSite
{
    public class PlayerModule : xrc.Modules.IModule
	{
		public Player GetPlayer()
		{
			return new Player() { FirstName = "John", LastName = "Wayne" };
		}
	}
}