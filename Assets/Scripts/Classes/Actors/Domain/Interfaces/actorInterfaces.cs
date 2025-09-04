
using System.Collections.Generic;

public interface IActor
{ }

//TODO: Probably should be an abstract class
public interface NPC : IExaminable, IActor
{
    public string dialogueFile { get; set; }
    public NpcCode internalCode { get; set; }
    ILocation currentLocation { get; set; }
    List<IWearable> clothes { get; set; }
    public Memory memory { get; set; }

    bool GetGiveReaction(IItem giftedItem)
    {
        return false;
    }
    bool GetTickleReaction(string part)
    {
        return false;
    }
    bool GetInsultReaction()
    {
        return false;
    }

}
