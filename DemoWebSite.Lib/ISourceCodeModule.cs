using System;
namespace DemoWebSite
{
	public interface ISourceCodeModule : xrc.Modules.IModule
	{
		string GetGitLink();
	}
}
