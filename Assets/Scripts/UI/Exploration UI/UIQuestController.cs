using System.Collections.Generic;
using UnityEngine;

public class UIQuestController : Singleton<UIQuestController>
{
    [SerializeField] private Transform questContainer;
    [SerializeField] private UIQuestSlot questSlotPrefab;
    private QuestManager questManager;
    private Dictionary<Quest, UIQuestSlot> questMap = new Dictionary<Quest, UIQuestSlot>();
    
    private void OnEnable()
    {
        questManager = FindObjectOfType<QuestManager>();
        InitQuestUI();
    }

    public void InitQuestUI()
    {
        questManager.ActiveQuests.ForEach(quest =>
        {
            CreateOrUpdateSlot(quest);
        });
    }

    public void CreateOrUpdateSlot(Quest quest)
    {
        if (!questMap.ContainsKey(quest))
        {
            var slot = CreateSlot(quest);
            questMap.Add(quest, slot);
        }
        else
        {
            UpdateSlot(quest);
        }
    }

    private UIQuestSlot CreateSlot(Quest quest)
    {
        var slot = Instantiate(questSlotPrefab, questContainer);
        slot.Init(quest);
        slot.AssignRemoveCallback(() => QuestManager.Instance.RemoveQuest(quest));
        return slot;
    }

    private void UpdateSlot(Quest quest)
    {
        questMap[quest].UpdateProgressOnUI(quest);
    }

    public void RemoveQuestFromUI(Quest quest)
    {
        Destroy(questMap[quest].gameObject);
        questMap.Remove(quest);
    }
    
}
