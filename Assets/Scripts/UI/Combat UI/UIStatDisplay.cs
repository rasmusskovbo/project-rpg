using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIStatDisplay : MonoBehaviour
{
    [SerializeField] private GameObject activeEffectPrefab;
    [SerializeField] private GameObject activeEffectsContainer;
    
    private CombatUnit connectedUnit;
    private Slider hpSlider;
    private TextMeshProUGUI[] infoText;
    private Image unitPortrait;

    private List<CombatEffect> activeEffectsReferences;
    
    // SOs
    [SerializeField] private List<ActiveEffectSO> activeEffectTypes;

    private void Start()
    {
        infoText = GetComponentsInChildren<TextMeshProUGUI>();
        infoText[0].text = ConnectedUnit.UnitName;
        infoText[1].text = "Lv" + ConnectedUnit.Level;

        GetComponentsInChildren<Image>()[0].sprite = connectedUnit.IdleSprite;
        
        hpSlider = GetComponentInChildren<Slider>();
        hpSlider.minValue = 0;

        activeEffectsReferences = new List<CombatEffect>();
    }
    
    void Update()
    {
        hpSlider.maxValue = connectedUnit.MaxHp;
        hpSlider.value = connectedUnit.CurrentHp;
        UpdateActiveEffects();
    }
    
    public CombatUnit ConnectedUnit
    {
        get => connectedUnit;
        set => connectedUnit = value;
    }
    
    public void UpdateActiveEffects()
    {
        List<GameObject> expiredObjects = new List<GameObject>();
        
        List<Transform> activeEffectTransforms = new List<Transform>();
        foreach (Transform child in activeEffectsContainer.transform)
        {
            activeEffectTransforms.Add(child);
        }
        
        // Loop through all displayed effects in the UI, for this unit.
        activeEffectTransforms.ForEach(effectTransform =>
        {
            CombatEffect activeEffectInUI = effectTransform.gameObject.GetComponent<ActiveEffectRef>().Reference;

            // Loop through all effects _actually_ active on the unit.
            activeEffectsReferences.ForEach(effectReferenceFromUnit =>
            {
                // if the effect displayed in the ui matches any of the active effects on the unit then update it.
                if (effectReferenceFromUnit == activeEffectInUI)
                {
                    if (effectReferenceFromUnit.DurationTracker.isEffectActive())
                    {
                        Debug.Log("EFFECT REF TYPE: " + effectReferenceFromUnit.CombatEffectType + ", DUR: " +effectReferenceFromUnit.DurationTracker.GetRemainingDuration());
                        effectTransform.gameObject.GetComponentInChildren<TextMeshProUGUI>().text
                            = (effectReferenceFromUnit.DurationTracker.GetRemainingDuration()).ToString();
                    }
                    else
                    {
                        expiredObjects.Add(effectTransform.gameObject);
                    }
                }
            });
   
        });

        Debug.Log("EFFECT REF: Expired Obj size: " + expiredObjects.Count);
        for (int i = expiredObjects.Count; i > 0; i--)
        {
            Destroy(expiredObjects[i - 1]);
        }
        
    }

    public void AddActiveEffect(CombatEffect effect)
    {
        GameObject activeEffectDisplay = Instantiate(activeEffectPrefab, activeEffectsContainer.transform);
        activeEffectDisplay.GetComponent<ActiveEffectRef>().Reference = effect;
        activeEffectDisplay.GetComponentInChildren<Image>().sprite = GetMatchingTypeSprite(effect);
        activeEffectsReferences.Add(effect);
    }

    public Sprite GetMatchingTypeSprite(CombatEffect effect)
    {
        return activeEffectTypes.Find(effectType => effectType.Type == effect.CombatEffectType).Icon;
    }
}
