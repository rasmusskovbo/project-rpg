public class DurationTracker
{
    private int remainingDuration;

    public DurationTracker(int duration)
    {
        SetDuration(duration);
    }

    public int GetRemainingDuration()
    {
        return remainingDuration;
    }

    public void SetDuration(int amountOfTurns)
    {
        remainingDuration = amountOfTurns + 1;
    }

    public bool isEffectActive()
    {
        return remainingDuration > 0;
    }

    public void DecreaseDuration()
    {
        if (remainingDuration > 0) remainingDuration--;
    }
}