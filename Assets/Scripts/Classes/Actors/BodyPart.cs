public class BodyPart : IExaminable
{
    public string bodyPartName { get; set; }
    public string description { get; set; }
    public bool isTicklish { get; set; }
    public string referenceName { get; set; }


    public BodyPart(string bodyPartName, string description, bool isTicklish)
    {
        this.bodyPartName = bodyPartName;
        this.referenceName = bodyPartName;
        this.description = description;
        this.isTicklish = isTicklish;
    }

    public string GetDescription()
    {
        return description;
    }
}