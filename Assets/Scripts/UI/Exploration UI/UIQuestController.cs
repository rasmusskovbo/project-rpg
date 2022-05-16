using System.Collections.Generic;
using UnityEngine;

public class UIQuestController : MonoBehaviour
{
    [SerializeField] private Transform questContainer;
    [SerializeField] private UIQuestSlot questSlotPrefab;
    private QuestManager questManager;
    private Dictionary<Quest, UIQuestSlot> questMap = new Dictionary<Quest, UIQuestSlot>();

    private void OnEnable()
    {
        questManager = QuestManager.Instance;
        InitQuestUI();
    }

    public void InitQuestUI()
    {
        questManager.ActiveQuests.ForEach(quest =>
        {
            CreateOrDeleteSlot(quest);
        });
    }

    public void CreateOrDeleteSlot(Quest quest)
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
        slot.AssignRemoveCallback(() => RemoveQuest(quest));
        return slot;
    }

    private void UpdateSlot(Quest quest)
    {
        questMap[quest].UpdateProgressOnUI(quest);
    }

    private void RemoveQuest(Quest quest)
    {
        Destroy(questMap[quest].gameObject);
        questMap.Remove(quest);
        questManager.RemoveQuest(quest);
    }
}
