
using System.Collections.Generic;

public interface IActor
{ }

public interface INPC : IExaminable, IActor
{
    LocationCode currentLocation { get; set; }

    bool GetGiveReaction(IItem giftedItem);
    bool GetInsultReaction();
}

public interface ComplexNPC : INPC
{
    public string dialogueFile { get; set; }
    public NpcCode internalCode { get; set; }
    List<IWearable> clothes { get; set; }
    List<IExaminable> examinables { get; set; } // Stuff you can examine in the same room as NPC
    public Memory memory { get; set; }
}