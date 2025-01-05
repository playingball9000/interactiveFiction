public static class DebugUtil
{
    public static void printPlayer()
    {
        LoggingUtil.Log($@" DEBUGUTIL - {System.Reflection.MethodBase.GetCurrentMethod().Name}
            {WorldState.GetInstance().player.ToString()}
            {WorldState.GetInstance().player.currentLocation.ToString()}
            ");

    }
}
