public static class ExploreControl
{
    private static bool timeRunning = true;

    public static bool IsTimeRunning
    {
        get => timeRunning;
        set => timeRunning = value;
    }

    public static void ToggleTimeRunning()
    {
        timeRunning = !timeRunning;
    }
}
