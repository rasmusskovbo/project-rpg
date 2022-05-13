
public class CombatGoal : QuestGoal
{
    public string enemyType; // Maybe ENUM
    public int amount;

    public override string Description()
    {
        string enemyDescript = amount > 1 ? "enemies" : "enemy";

        return $"Defeat {amount} {enemyType} {enemyDescript}";
    }
    
}
