using UnityEngine;

public class LevelUpManager : MonoBehaviour
{
    private GameManager gameManager;

    [SerializeField] private int statPointsPrLevel = 3;
    [SerializeField] private int capBaseGrowth = 15;
    [SerializeField] private int capGrowthRate = 10;
    [SerializeField] private float capMultiplier = 1.1f;
    
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public bool PlayerShouldLevelUp(int currentExp, int nextLvlExp)
    {
        return currentExp >= nextLvlExp;
    }

    public void LevelUp()
    {
        gameManager.PlayerData.remainingStatPoints += statPointsPrLevel;
        gameManager.PlayerData.nextLvLExp += capBaseGrowth;

        capGrowthRate = Mathf.RoundToInt(capGrowthRate * capMultiplier);
        capBaseGrowth += capGrowthRate;
    }
}
