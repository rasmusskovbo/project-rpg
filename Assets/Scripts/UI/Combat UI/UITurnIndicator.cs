using UnityEngine;
using UnityEngine.UI;

public class UITurnIndicator : MonoBehaviour
{
    [SerializeField] private Image leftIndicator;
    [SerializeField] private Image rightIndicator;
    [SerializeField] private Sprite greyIndicator;
    [SerializeField] private Sprite greenIndicator;
    private CombatSystem combatSystem;

    private void Awake()
    {
        combatSystem = FindObjectOfType<CombatSystem>();
    }

    private void Update()
    {
        switch (combatSystem.RemainingPlayerActions)
        {
            case 0:
                leftIndicator.sprite = greyIndicator;
                rightIndicator.sprite = greyIndicator;
                break;
            case 1:
                leftIndicator.sprite = greyIndicator;
                rightIndicator.sprite = greenIndicator;
                break;
            case 2:
                leftIndicator.sprite = greenIndicator;
                rightIndicator.sprite = greenIndicator;
                break;
        }
    }
}
