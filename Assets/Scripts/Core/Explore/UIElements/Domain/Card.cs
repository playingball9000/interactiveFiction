[System.Serializable]
public class Card
{
    public string title;
    public float timeToComplete; // In seconds

    public bool isUnlocked = false;
    public bool isCompleted = false;

    public Card(string title, float timeToComplete)
    {
        this.title = title;
        this.timeToComplete = timeToComplete;
    }
}
