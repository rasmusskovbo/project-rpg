using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// https://www.youtube.com/watch?v=gx0Lt4tCDE0
public class QuestEventSystem : PersistentSingleton<QuestEventSystem>
{
    public event Action onQuestCompleteTrigger;

    public void QuestCompleteTrigger()
    {
        if (onQuestCompleteTrigger != null)
        {
            onQuestCompleteTrigger();
        }
    }
}
