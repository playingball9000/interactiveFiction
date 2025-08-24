public interface IExaminable
{
    string displayName { get; set; }
    string adjective { get; set; }
    string description { get; set; }
}

public static class ExaminableExtensions
{
    public static string GetDisplayName(this IExaminable examinable)
    {
        return string.IsNullOrEmpty(examinable.adjective)
            ? examinable.displayName
            : examinable.adjective + " " + examinable.displayName;
    }

    public static string GetDescription(this IExaminable examinable)
    {
        return examinable.description;
    }
}