
using System.Collections.Generic;

public interface IActor
{ }

//TODO: Probably should be an abstract class
public interface NPC : IExaminable, IActor
{
    public string dialogueFile { get; set; }
    public string internalCode { get; set; }
    ILocation currentLocation { get; set; }
    List<IWearable> clothes { get; set; }
    public Memory memory { get; set; }

    bool GetGiveReaction(IItem giftedItem)
    {
        // default implementation
        return false; // default: no reaction
    }
    bool GetTickleReaction(string part);

}
