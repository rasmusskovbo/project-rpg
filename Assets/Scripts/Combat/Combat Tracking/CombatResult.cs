public class CombatResult
{
    private int xpGained;
    private float playerCurrentHp;

    public CombatResult(int xpGained, float playerCurrentHp)
    {
        this.xpGained = xpGained;
        this.playerCurrentHp = playerCurrentHp;
    }

    public int XpGained
    {
        get => xpGained;
        set => xpGained = value;
    }

    public float PlayerCurrentHp
    {
        get => playerCurrentHp;
        set => playerCurrentHp = value;
    }
}
