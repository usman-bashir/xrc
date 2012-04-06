using System;

namespace xrc.Modules
{
    public interface ISlotModule
    {
		string Include();
		string Include(string slotName);
    }
}
