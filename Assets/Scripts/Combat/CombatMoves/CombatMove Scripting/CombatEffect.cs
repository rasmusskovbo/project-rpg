public class CombatEffect
{
    private float power;
    private bool expiresAtStartOfTurn;
    private bool hasTurnDuration;
    private DurationTracker durationTracker;
    private CombatEffectType combatEffectType;

    public CombatEffect(CombatMove move)
    {
        power = move.GetPower();
        expiresAtStartOfTurn = move.GetExpiresAtStartOfTurn();
        durationTracker = new DurationTracker(move.GetDuration());
        hasTurnDuration = durationTracker.GetRemainingDuration() > 0;
        combatEffectType = move.GetEffectType();
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

    public bool ExpiresAtStartOfTurn
    {
        get => expiresAtStartOfTurn;
        set => expiresAtStartOfTurn = value;
    }

    public bool HasTurnDuration
    {
        get => hasTurnDuration;
        set => hasTurnDuration = value;
    }
}
