public static class SetupCards
{
    public static void initialize()
    {
        new CardBuilder("Find the key", CardCode.card1, 1f)
            .WithTooltip("")
            .WithCompletionLog("Yay found key")
            .AsLimitedRepeats(2)
            .Build();

        new CardBuilder("Unlock Door", CardCode.card2, 2f)
            .WithTooltip("")
            .WithCompletionLog("door unlock")
            .AsRepeatable()
            .Build();


        new CardBuilder("Open Door", CardCode.card3, 3f)
            .WithTooltip("")
            .WithCompletionLog("")
            .Build();


        new CardBuilder("Rescue", CardCode.card4, 1f)
            .WithTooltip("")
            .WithCompletionLog("")
            .Build();
    }
}