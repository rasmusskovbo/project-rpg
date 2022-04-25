using System.Collections.Generic;
using UnityEngine;

/*
 * Should be a singleton, to keep track of ability cooldowns in exploring mode as well.
 */
public class SkillManager : MonoBehaviour
{
    [SerializeField] private List<CombatMoveBase> combatMoveBases; // A comprehensive list of all combat moves in the game to draw from
    [SerializeField] private CombatMoveBase testMove;
    private List<CombatMove> activeCombatMoves;
    
    void Start()
    {
        // Temporary loader:
        activeCombatMoves = new List<CombatMove>();
        combatMoveBases.ForEach(baze => activeCombatMoves.Add(new CombatMove(baze)));
    }

    public CombatMove GetTestMove()
    {
        /*
        Random rnd = new Random(10);
        return activeCombatMoves[rnd.NextInt(0, activeCombatMoves.Count)];
        */
        return new CombatMove(testMove);
    }

    public void PutCombatMoveOnCooldown(CombatMove move)
    {
        Debug.Log("Trying to put move on CD: " + move.GetName());
        activeCombatMoves.Find(combatMove => combatMove.Equals(move)).GetCooldownTracker().PutMoveOnCooldown(move.GetCooldown());
    }

    public void DecreaseCooldowns()
    {
        activeCombatMoves.ForEach(move => move.GetCooldownTracker().DecreaseCooldownCounter());
    }

    public List<CombatMove> GetActiveCombatMoves()
    {
        return activeCombatMoves;
    }

}
