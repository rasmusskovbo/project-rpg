using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISkillsController : MonoBehaviour
{
    // Load skills into the ui from skillmanager
    [SerializeField] private GameObject skillItemPrefab;
    [SerializeField] private RectTransform contentRectTransform;
    
    private SkillManager skillManager;

    private void Start()
    {
        skillManager = FindObjectOfType<SkillManager>();

        InitUI();
    }

    private void InitUI()
    {
        skillManager.GetActiveCombatMoves().ForEach(AddSkillToUI);
    }

    private void AddSkillToUI(CombatMove combatMove)
    {
        var item = Instantiate(skillItemPrefab, contentRectTransform);
        
        item.GetComponentsInChildren<Image>()[0].sprite = combatMove.getIconImage();
        item.GetComponentsInChildren<Image>()[1].sprite = combatMove.GetIcon();
        item.GetComponentsInChildren<TextMeshProUGUI>()[0].SetText(combatMove.GetName());
        item.GetComponentsInChildren<TextMeshProUGUI>()[1].SetText(combatMove.GetPower().ToString());
        item.GetComponentsInChildren<TextMeshProUGUI>()[2].SetText(combatMove.GetCooldown().ToString());
        
        UITooltipTrigger uiTooltipTrigger = item.AddComponent<UITooltipTrigger>();
        uiTooltipTrigger.title = combatMove.GetName();
        uiTooltipTrigger.subtitle = combatMove.GetType().ToString();
        uiTooltipTrigger.body = combatMove.GetDescription();
        
        var formattedDuration = combatMove.GetDuration() > 0 ? combatMove.GetDuration().ToString() : "-";
        item.GetComponentsInChildren<TextMeshProUGUI>()[3].SetText(formattedDuration);

        item.transform.localScale = Vector2.one;
    }
}
