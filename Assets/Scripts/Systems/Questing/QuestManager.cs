using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : PersistentSingleton<QuestManager>
{
    [SerializeField] private List<Quest> activeQuests;

    private void Start()
    {
        activeQuests.ForEach(quest => quest.Initialize());
    }

    public void AddQuest(Quest quest)
    {
        activeQuests.Add(quest);
    }

    public void RemoveQuest(Quest quest)
    {
        activeQuests.Remove(quest);
    }
    
    public List<Quest> ActiveQuests
    {
        get => activeQuests;
        set => activeQuests = value;
    }
}
