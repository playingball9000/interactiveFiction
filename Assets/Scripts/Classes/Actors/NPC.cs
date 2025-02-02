using System.Collections.Generic;

public interface NPC : IExaminable
{
    public string dialogueFile { get; set; }
    public string internalCode { get; set; }
    Room currentLocation { get; set; }
    List<IClothing> clothes { get; set; }
    public Memory memory { get; set; }

    bool GetGiveReaction(IItem giftedItem);

}
