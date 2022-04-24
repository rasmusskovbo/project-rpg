using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIStatDisplay : MonoBehaviour
{
    [SerializeField] private GameObject activeEffectPrefab;
    [SerializeField] private GameObject activeEffectsContainer;
    [SerializeField] private List<ActiveEffectSO> activeEffectTypes;
    
    private CombatUnit connectedUnit;
    private Slider hpSlider;
    private TextMeshProUGUI[] infoText;
    private Image unitPortrait;
    private List<CombatEffect> activeEffectsReferences;
    private bool isConnectedUnitActive;
    
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
        List<CombatEffect> expiredReferences = new List<CombatEffect>();
        
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
                    if (effectReferenceFromUnit.HasTurnDuration)
                    {
                        if (effectReferenceFromUnit.DurationTracker.isEffectActive())
                        {
                            effectTransform.gameObject.GetComponentInChildren<TextMeshProUGUI>().text
                                = (effectReferenceFromUnit.DurationTracker.GetRemainingDuration()).ToString();
                        }
                        else
                        {
                            expiredObjects.Add(effectTransform.gameObject);
                            expiredReferences.Add(effectReferenceFromUnit);
                        }    
                    }
                    // Separate update logic for effects without duration - Display current power instead.
                    else 
                    {
                        if (effectReferenceFromUnit.CombatEffectType == CombatEffectType.Block)
                        {
                            if (connectedUnit.CurrentPhysicalBlock > 0)
                            {
                                effectTransform.gameObject.GetComponentInChildren<TextMeshProUGUI>().color = Color.yellow;
                                effectTransform.gameObject.GetComponentInChildren<TextMeshProUGUI>().text
                                    = connectedUnit.CurrentPhysicalBlock.ToString();
                            }
                            else
                            {
                                expiredObjects.Add(effectTransform.gameObject);
                                expiredReferences.Add(effectReferenceFromUnit);    
                            }
                        }
                        // Current cant display current mitigation here. Maybe non duration type effects should be displayed
                        // in another way
                        if (effectReferenceFromUnit.CombatEffectType == CombatEffectType.PhysMitigation)
                        {
                            if (connectedUnit.CurrentPhysicalMitigation <= 0)
                            {
                                expiredObjects.Add(effectTransform.gameObject);
                                expiredReferences.Add(effectReferenceFromUnit);
                            }
                        }
                        if (effectReferenceFromUnit.CombatEffectType == CombatEffectType.MagicMitigation)
                        {
                            if (connectedUnit.CurrentMagicalMitigation <= 0)
                            {
                                expiredObjects.Add(effectTransform.gameObject);
                                expiredReferences.Add(effectReferenceFromUnit);
                            }
                        } 
                    }
                }
            });
        });

        // Remove the expired combat effect from the reference list & the UI.
        for (int i = expiredObjects.Count; i > 0; i--)
        {
            expiredReferences.RemoveAt(i-1);
            Destroy(expiredObjects[i - 1]);
        }
        
    }

    private void UpdateDisplayForEffectsWithoutDuration(CombatEffect effectReferenceFromUnit, Transform effectTransform)
    {
        if (effectReferenceFromUnit.CombatEffectType == CombatEffectType.Block)
        {
            if (connectedUnit.CurrentPhysicalBlock > 0)
            {
                effectTransform.gameObject.GetComponentInChildren<TextMeshProUGUI>().color = Color.yellow;
                effectTransform.gameObject.GetComponentInChildren<TextMeshProUGUI>().text
                    = connectedUnit.CurrentPhysicalBlock.ToString();
            }
            else
            {
                
            }
        }
        if (effectReferenceFromUnit.CombatEffectType == CombatEffectType.PhysMitigation)
        {
            if (connectedUnit.CurrentPhysicalMitigation > 0)
            {
                effectTransform.gameObject.GetComponentInChildren<TextMeshProUGUI>().color = Color.yellow;
                effectTransform.gameObject.GetComponentInChildren<TextMeshProUGUI>().text
                    = connectedUnit.CurrentPhysicalMitigation.ToString() + "%";
            }
        }
        
        if (effectReferenceFromUnit.CombatEffectType == CombatEffectType.MagicMitigation)
        {
            if (connectedUnit.CurrentMagicalMitigation > 0)
            {
                effectTransform.gameObject.GetComponentInChildren<TextMeshProUGUI>().color = Color.yellow;
                effectTransform.gameObject.GetComponentInChildren<TextMeshProUGUI>().text
                    = connectedUnit.CurrentMagicalMitigation.ToString() + "%";
            }
        }
        
        /* Not yet implemented
        if (effectReferenceFromUnit.CombatEffectType == CombatEffectType.AllMitigation)
        {
            if (connectedUnit.CurrentPhysicalMitigation > 0 && connectedUnit.CurrentMagicalMitigation > 0)
            {
                effectTransform.gameObject.GetComponentInChildren<TextMeshProUGUI>().color = Color.yellow;
                effectTransform.gameObject.GetComponentInChildren<TextMeshProUGUI>().text
                    = connectedUnit.CurrentPhysicalMitigation.ToString() + "%";
            }
        }
        */
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
        var matchingTypeSprite = activeEffectTypes.Find(effectType => effectType.Type == effect.CombatEffectType).Icon;
        return matchingTypeSprite;
    }
}
