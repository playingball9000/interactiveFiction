public class SimpleNPC : INPC
{
    public LocationCode currentLocation { get; set; }
    public string BasicDialogue { get; set; }
    public string displayName { get; set; }
    public string description { get; set; }
    public string adjective { get; set; } = "";

    public static SimpleNPC Create()
    {
        return new SimpleNPC();
    }

    //TODO: Maybe can do a query runner here? think about it later
    public bool GetGiveReaction(IItem giftedItem) => false;
    public bool GetTickleReaction(string part) => false;
    public bool GetInsultReaction() => false;

    public string GetBasicDialogue() => BasicDialogue;
}
