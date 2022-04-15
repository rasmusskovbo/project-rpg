public class CombatEffect
{
    private float power;
    private DurationTracker durationTracker;
    private CombatEffectType combatEffectType;

    public CombatEffect(CombatMove move, CombatEffectType effectType)
    {
        power = move.GetPower();
        durationTracker = new DurationTracker(move.GetDuration());
        combatEffectType = effectType;
    }

    public float Power
    {
        get => power;
        set => power = value;
    }

    public DurationTracker DurationTracker
    {
        get => durationTracker;
        set => durationTracker = value;
    }

    public CombatEffectType CombatEffectType
    {
        get => combatEffectType;
        set => combatEffectType = value;
    }
}
