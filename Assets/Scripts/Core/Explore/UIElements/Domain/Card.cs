[System.Serializable]
public class Card
{
    public string title;
    public float timeToComplete; // In seconds (or minutes, up to you)

    public Card(string title, float timeToComplete)
    {
        this.title = title;
        this.timeToComplete = timeToComplete;
    }
}
