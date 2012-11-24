using System;
namespace xrc.Modules
{
    public interface ISlotModule
    {
        string IncludeChild();
        string IncludeChild(string slotName);

        // TODO Valutare se aggiungere questi metodi per performance, farebbero il render direttamente sul response stream
        //void RenderInclude(string url);
        //void RenderInclude(string url, object parameters);

        string Include(string url);
        string Include(string url, object parameters);
    }
}
