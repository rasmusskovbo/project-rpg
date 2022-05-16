using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// https://www.youtube.com/watch?v=gx0Lt4tCDE0
public class GameEvents : PersistentSingleton<GameEvents>
{
    /*
     * Dialogue
     */
    public event Action onShowDialog;
    public void OnShowDialogInvoke()
    {
        onShowDialog?.Invoke();
    }
    
    public event Action onCloseDialog;
    public void OnCloseDialogInvoke()
    {
        onCloseDialog?.Invoke();
    }
    
    /*
     * Questing
     */
    public event Action onCombatVictory;
    public void CombatVictoryInvoke()
    {
        onCombatVictory?.Invoke();
    }

    // Change to enum for type safety
    public event Action<string> onGatheredTrigger;
    public void GatheredInvoke(string name)
    {
        if (onGatheredTrigger != null)
        {
            onGatheredTrigger(name);
        }
    }
    
    // Change to enum for type safety
    public event Action<string> onInteractedWithTrigger;
    public void InteractedWithTrigger(string name)
    {
        if (onInteractedWithTrigger != null)
        {
            onInteractedWithTrigger(name);
        }
    }
}
