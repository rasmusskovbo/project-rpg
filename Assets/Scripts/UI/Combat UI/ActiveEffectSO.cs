using UnityEngine;

[CreateAssetMenu(fileName = "ActiveEffectDisplay", menuName = "Combat/Create new Active Effect SO")]
public class ActiveEffectSO : ScriptableObject
{
    [SerializeField] private CombatEffectType type;
    [SerializeField] private Sprite icon;

    public CombatEffectType Type
    {
        get => type;
        set => type = value;
    }

    public Sprite Icon
    {
        get => icon;
        set => icon = value;
    }
}
