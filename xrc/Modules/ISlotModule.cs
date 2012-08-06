using System;
namespace xrc.Modules
{
    public interface ISlotModule : IModule
    {
        string IncludeChild();
        string IncludeChild(string slotName);

        string Include(string url);
        string Include(string url, object parameters);
    }
}
