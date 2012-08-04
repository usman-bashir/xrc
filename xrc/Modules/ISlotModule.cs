using System;
namespace xrc.Modules
{
    public interface ISlotModule : IModule
    {
        string Include();
        string Include(string slotName);
    }
}
