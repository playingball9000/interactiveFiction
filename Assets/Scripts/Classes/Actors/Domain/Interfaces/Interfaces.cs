using System.Collections.Generic;

public interface IWearable : IExaminable, IItem
{
    bool isWearable { get; set; }
    List<EquipmentSlot> slotsTaken { get; set; }
    ClothingLayer layer { get; set; }
    List<Attribute> attributes { get; set; }
}

public interface IExaminable
{
    string referenceName { get; set; }
    string adjective { get; set; }
    string description { get; set; }
    string GetDisplayName();
    string GetDescription();
}

public interface IItem : IExaminable
{
    bool isGettable { get; set; }
    string pickUpNarration { get; }
    public string internalCode { get; set; }
    //TODO: probably should contain a list of disambiguations, but priority check 1) exact match 2) partial match 3) disambiguation match
}

public interface IPlayerAction
{
    public PlayerAction playerActionCode { get; }
    public string tooFewMessage { get; }
    public string tooManyMessage { get; }
    public int minInputCount { get; }
    public int maxInputCount { get; }
    public abstract void Execute(ActionInput actionInput);
}

public interface IStorage
{
    List<IItem> contents { get; set; }

    void RemoveItem(IItem item);
    List<IItem> GetContents();
    string ContentsToString();
    bool isAccessible();
}

public interface ILockable
{
    bool isLocked { get; set; }
    public string lockedText { get; set; }
    public string keyInternalCode { get; set; }
}
public interface IOpenable
{
    bool isOpen { get; set; }
}

public interface ILocation
{
    string displayName { get; }
    string internalCode { get; }
}
