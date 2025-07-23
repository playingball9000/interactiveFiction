using System.Collections.Generic;

[System.Serializable]
public abstract class Portal : ILockable
{
    public ExitDirection exitDirection;

    // I found it easier for now, to put the key code on Exit instead of vice versa
    public bool isLocked { get; set; } = false;
    public string lockedText { get; set; }
    public string keyInternalCode { get; set; } = "";

    public bool isTargetAccessible()
    {
        return !isLocked;
    }

    public abstract string getNotAccessibleReason();

    public string getNotAccessibleTag()
    {
        if (isLocked)
        {
            return $"-Locked-";
        }
        return $"-Inaccessible-";
    }
}
