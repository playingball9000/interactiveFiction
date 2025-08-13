using System.Collections.Generic;

public class CardCodeComparer : IEqualityComparer<Card>
{
    public bool Equals(Card x, Card y)
    {
        return x?.internalCode == y?.internalCode;
    }

    public int GetHashCode(Card obj)
    {
        return obj.internalCode.GetHashCode();
    }
}
