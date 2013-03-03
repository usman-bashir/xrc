using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc.Pages.TreeStructure
{
	class TestPageStructure : IPageStructureService
	{
        public PageRoot GetRoot()
		{
            return new PageRoot("~/",
					new PageFile("index.xrc"),
                    new PageFile("_layout.xrc"),
					new ConfigFile(),
                    new PageFile("about.xrc"),
                    new PageFile("ConTact.xrc"),
					new PageDirectory("news",
                        new PageFile("index.xrc"),
                        new PageFile("_slot1.xrc"),
                        new PageDirectory("{id...}",
                            new PageFile("index.xrc")
						)
					),
                    new PageDirectory("docs",
                        new PageFile("{page...}.xrc")
					),
                    new PageDirectory("athletes",
                        new PageFile("index.xrc"),
						new ConfigFile(),
                        new PageDirectory("{athleteid}",
                            new PageFile("index.xrc"),
                            new PageFile("bio.xrc")
						)
					),
                    new PageDirectory("teams",
                        new PageFile("index.xrc"),
                        new PageDirectory("{teamid}",
                            new PageDirectory("{playerid}",
								new ConfigFile(),
                                new PageFile("_layout.xrc"),
                                new PageFile("index.xrc"),
                                new PageFile("stats.xrc")
							),
                            new PageFile("index.xrc"),
                            new PageFile("matches.xrc"),
                            new PageFile("_layout.xrc")
						)
					),
                    new PageDirectory("photos",
                        new PageDirectory("{id}sport",
                            new PageFile("index.xrc")
							),
                        new PageDirectory("photo{id}",
                            new PageFile("index.xrc")
							)
					),
                    new PageDirectory("downloads",
                        new PageDirectory("{page...}2012",
                            new PageFile("file1.xrc")
							),
                        new PageDirectory("{page...}",
                            new PageFile("file2.xrc")
							)
					)
				);
		}
	}

	class TestPageStructure_With_VirtualDir : IPageStructureService
	{
        public PageRoot GetRoot()
		{
            return new PageRoot("~/xrcroot",
                    new PageFile("index.xrc"),
                    new PageFile("about.xrc")
				);
		}
	}
}
