public abstract class GameEvent
{
    public string EventDescription;
}

public class UnitDefeatedEvent : GameEvent
{
    public string unitName;

    public UnitDefeatedEvent(string name)
    {
        unitName = name;
    }
}
