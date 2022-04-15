public class CooldownTracker
{
    private int remainingCooldown;

    public CooldownTracker(int remainingCooldown)
    {
        this.remainingCooldown = remainingCooldown;
    }

    public int GetRemainingCooldown()
    {
        return remainingCooldown;
    }

    public void PutMoveOnCooldown(int amountOfTurns)
    {
        remainingCooldown = amountOfTurns + 1;
    }

    public bool isMoveOnCooldown()
    {
        return remainingCooldown > 0;
    }

    public void DecreaseCooldownCounter()
    {
        if (remainingCooldown > 0) remainingCooldown--;
    }
}
