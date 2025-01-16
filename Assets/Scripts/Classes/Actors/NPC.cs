using System.Collections.Generic;

public interface NPC : IExaminable
{
    public string dialogueFile { get; set; }
    Room currentLocation { get; set; }
    List<IClothing> clothes { get; set; }

    public void GetGiveReaction(IItem giftedItem) { }

}
