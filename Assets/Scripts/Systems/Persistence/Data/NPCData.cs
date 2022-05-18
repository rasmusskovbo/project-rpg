using System;
using UnityEngine;

[Serializable]
public class NPCData
{
    [SerializeField] public SerializableDictionary<string, bool> npcQuestTurnedInMap;

    public NPCData(SerializableDictionary<string, bool> npcQuestTurnedInMap)
    {
        this.npcQuestTurnedInMap = npcQuestTurnedInMap;
    }
    
}
