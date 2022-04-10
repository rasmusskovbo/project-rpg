public class TakeDamageResult
{
    private bool isUnitDead;
    private int damageTaken;

    public TakeDamageResult(bool isUnitDead, int damageTaken)
    {
        this.isUnitDead = isUnitDead;
        this.damageTaken = damageTaken;
    }
    
    public bool IsUnitDead => isUnitDead;
    public int DamageTaken => damageTaken;
}