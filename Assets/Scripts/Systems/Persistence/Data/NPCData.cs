using System;
using UnityEngine;

[Serializable]
public class NPCData : SaveData
{
    [SerializeField] public SerializableDictionary<string, bool> npcQuestTurnedInMap;

    public NPCData(SerializableDictionary<string, bool> npcQuestTurnedInMap)
    {
        this.npcQuestTurnedInMap = npcQuestTurnedInMap;
    }
    
    public void ResetBeforeSave()
    {
        npcQuestTurnedInMap = new SerializableDictionary<string, bool>();
    }
    
}
