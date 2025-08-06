[System.Serializable]
public class ThresholdStage
{
    public int minCompletionCount;
    public float scalingFactor; // e.g., 0.9 = 10% faster per stage

    public ThresholdStage(int min, float scale)
    {
        minCompletionCount = min;
        scalingFactor = scale;
    }
}
