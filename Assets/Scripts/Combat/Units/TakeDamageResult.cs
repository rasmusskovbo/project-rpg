public class TakeDamageResult
{
    private CombatUnit unit;
    private bool isUnitDead;
    private int damageTaken;

    public TakeDamageResult(CombatUnit unit, bool isUnitDead, int damageTaken)
    {
        this.unit = unit;
        this.isUnitDead = isUnitDead;
        this.damageTaken = damageTaken;
    }
    
    public CombatUnit Unit => unit;
    public bool IsUnitDead => isUnitDead;
    public int DamageTaken => damageTaken;
    
}