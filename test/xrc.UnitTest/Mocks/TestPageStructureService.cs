using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xrc.Pages.Providers.Common;

namespace xrc
{
	class TestPageStructure : IPageStructureService
	{
		public XrcItem GetRoot()
		{
			return XrcItem.NewRoot("~/",
					XrcItem.NewXrcFile("index.xrc"),
					XrcItem.NewXrcFile("_layout.xrc"),
					XrcItem.NewConfigFile(),
					XrcItem.NewXrcFile("about.xrc"),
					XrcItem.NewXrcFile("ConTact.xrc"),
					XrcItem.NewDirectory("news",
						XrcItem.NewXrcFile("index.xrc"),
						XrcItem.NewXrcFile("_slot1.xrc"),
						XrcItem.NewDirectory("{id_catch-all}",
							XrcItem.NewXrcFile("index.xrc")
						)
					),
					XrcItem.NewDirectory("docs",
						XrcItem.NewXrcFile("{page_CATCH-ALL}.xrc")
					),
					XrcItem.NewDirectory("athletes",
						XrcItem.NewXrcFile("index.xrc"),
						XrcItem.NewConfigFile(),
						XrcItem.NewDirectory("{athleteid}",
							XrcItem.NewXrcFile("index.xrc"),
							XrcItem.NewXrcFile("bio.xrc")
						)
					),
					XrcItem.NewDirectory("teams",
						XrcItem.NewXrcFile("index.xrc"),
						XrcItem.NewDirectory("{teamid}",
							XrcItem.NewDirectory("{playerid}",
								XrcItem.NewConfigFile(),
								XrcItem.NewXrcFile("_layout.xrc"),
								XrcItem.NewXrcFile("index.xrc"),
								XrcItem.NewXrcFile("stats.xrc")
							),
							XrcItem.NewXrcFile("index.xrc"),
							XrcItem.NewXrcFile("matches.xrc"),
							XrcItem.NewXrcFile("_layout.xrc")
						)
					),
					XrcItem.NewDirectory("photos",
						XrcItem.NewDirectory("{id}sport",
							XrcItem.NewXrcFile("index.xrc")
							),
						XrcItem.NewDirectory("photo{id}",
							XrcItem.NewXrcFile("index.xrc")
							)
						)
				);
		}
	}

	class TestPageStructure_With_VirtualDir : IPageStructureService
	{
		public XrcItem GetRoot()
		{
			return XrcItem.NewRoot("~/xrcroot",
					XrcItem.NewXrcFile("index.xrc"),
					XrcItem.NewXrcFile("about.xrc")
				);
		}
	}
}
