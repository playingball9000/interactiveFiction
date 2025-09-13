
using System.Collections.Generic;

public interface IActor
{ }

public interface INPC : IExaminable, IActor
{
    LocationCode currentLocation { get; set; }

    bool GetGiveReaction(IItem giftedItem);
    bool GetTickleReaction(string part);
    bool GetInsultReaction();
}

public interface ComplexNPC : INPC
{
    public string dialogueFile { get; set; }
    public NpcCode internalCode { get; set; }
    List<IWearable> clothes { get; set; }
    public Memory memory { get; set; }


}